using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Reflection;
using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.IO.Compression;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Finix.CsUtils
{
    /// <summary>
    /// A Mark, marking a line and column in a file.
    /// </summary>
    public record Mark(string? Source, int Line, int Column) : IFormattable
    {
        public static readonly Mark Zero = new(null, 0, 0);

        /// <summary>
        /// Create a new mark that is moved in a specific <paramref name="direction"/> along
        /// <see cref="Column" /> or <see cref="Line"/> according to the value of <paramref name="rune"/>.
        ///
        /// If the value of <paramref name="rune"/> is <c>\n</c> <see cref="Line"/> is moved instead of <see cref="Column" />.
        /// If <see cref="Line"/> is moved, <see cref="Column" /> is reset to 1.
        /// </summary>
        /// <param name="rune">A rune according to which either <see cref="Column" /> or <see cref="Line"/> is moved.</param>
        /// <param name="direction">The number to add to either <see cref="Column" /> or <see cref="Line"/></param>
        /// <returns></returns>
        public Mark MoveMarkUp(Rune rune)
        {
            // If the rune's value is '\n' (0x0A) change the line and reset the column to 1,
            // otherwise the column is moved instead.
            if (rune.Value == '\n')
                return this with { Line = Math.Max(1, Line + 1), Column = 1 };
            else
                return this with { Column = Math.Max(1, Column + 1) };
        }

        public Mark MoveMarkUp(string str)
        {
            return MoveMarkUp(str.EnumerateRunes());
        }

        public Mark MoveMarkUp(IEnumerable<Rune> runes)
        {
            var pos = this;

            foreach (var c in runes)
                pos = pos.MoveMarkUp(c);

            return pos;
        }

        public Mark MoveMarkUp(ReadOnlySpan<Rune> runes)
        {
            var pos = this;

            foreach (var c in runes)
                pos = pos.MoveMarkUp(c);

            return pos;
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            switch (format?.Trim()?.ToUpperInvariant())
            {
                case "G":
                case null:
                    return ToString();

                case "F":
                case "FILE":
                    return $"{Source}:{Line}:{Column}";

                case "T":
                case "TEXT":
                    return $"line {Line}, column {Column}" + (Source is not null ? " of " + Source : String.Empty);

                default:
                    throw new FormatException($"The '{format}' format string is not supported.");
            }
        }

        public static string? SourceFrom(object obj)
        {
            var pi = obj.GetType().GetRuntimeProperty("UnderlyingStream")
                ?? obj.GetType().GetRuntimeProperty("BaseStream")
                ?? obj.GetType().GetRuntimeProperty("UnderlyingReader")
                ?? obj.GetType().GetRuntimeProperty("BaseReader");

            if (pi == null)
                return null;

            return pi.GetValue(obj) switch {
                TextReader r => SourceFrom(r),
                Stream s => SourceFrom(s),
                _ => null
            };
        }

        public static string? SourceFrom(TextReader reader)
        {
            return reader is StreamReader r ? SourceFrom(r.BaseStream) : SourceFrom((object) reader);
        }

        public static string? SourceFrom(Stream stream)
        {
            return stream switch {
                FileStream fs => fs.Name,
                NetworkStream ns => ns?.Socket?.RemoteEndPoint?.ToString(),
                _ => SourceFrom((object) stream)
            };
        }
    }

    /// <summary>
    /// An output generated by a parser, includes the <see cref="Start"/>-Mark (inclusive) and <see cref="End"/>-Mark (exclusive) of the output.
    /// </summary>
    public abstract record ParserOutput(Mark Start, Mark End)
    {
        /// <summary>
        /// Gets whether this token represents an EOF.
        /// </summary>
        /// <value></value>
        public bool EOF { get; init; }

        /// <summary>
        /// Gets the string representation of the output.
        /// </summary>
        public abstract string StringValue { get; }
    }

    /// <summary>
    /// A string that was read by a parser, see <see cref="ParserOutput"/>.
    ///
    /// The output can be sliced using the range operator: <code>parserString[1..^1]</code>
    /// </summary>
    public record ParserString(Mark Start, Mark End, ImmutableArray<Rune> Runes) : ParserOutput(Start, End), IEnumerable<Rune>
    {
        public ParserString(Mark start, Mark end, IEnumerable<Rune> runes)
            : this(start, end, runes.ToImmutableArray())
        {
        }

        public ParserString(Mark start, Mark end, string str)
            : this(start, end, str.EnumerateRunes())
        {
        }

        public ParserString(Mark start, Mark end, ReadOnlySpan<Rune> runes)
            : this(start, end, (IEnumerable<Rune>) runes.ToArray())
        {
        }

        /// <inheritdoc />
        public override string StringValue => String.Join(String.Empty, Runes);

        public int Length => Runes.Length;

        public ParserString WithString(string str)
        {
            return new(Start, End, str);
        }

        /// <summary>
        /// Gets a new <see cref="ParserString"/> from this one that is sliced according to the <paramref name="range"/> parameter.
        ///
        /// The sliced output will have its <see cref="Start"/> and <see cref="End"/> marks adjusted accordingly.
        /// </summary>
        public ParserString this[Range range]
        {
            get
            {
                // We take the strings up to range.Start and from range.End
                // and move the mark on either side according to the trimmed characters.

                var runes = Runes.AsSpan();

                var trimLeft = runes[..range.Start];
                var trimRight = runes[range.End..];

                var start = Start;
                var end = End;

                start = start.MoveMarkUp(trimLeft);

                var line = end.Line;
                var col = end.Column;

                foreach (var r in trimRight)
                {
                    if (r.Value == '\n')
                    {
                        line--;
                        col = 1;
                    }
                    else
                    {
                        col--;
                    }
                }

                var left = runes[range].ToArray().ToImmutableArray();
                col += left.Reverse().TakeWhile(r => r.Value != '\n').Count();

                if (!left.Contains(new Rune('\n')))
                    col += start.Column;

                // Finally create the new ParserString with the adjusted marks and value.
                return new(start, end with { Column = col, Line = line }, left);
            }
        }

        public static ParserString operator +(ParserString lhs, ParserString rhs)
        {
            return new ParserString(lhs.Start, rhs.End, lhs.Concat(rhs)) { EOF = rhs.EOF };
        }

        public IEnumerator<Rune> GetEnumerator()
        {
            return ((IEnumerable<Rune>) Runes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return StringValue;
        }
    }

    /// <summary>
    /// A single character that was read by a parser, see <see cref="ParserOutput"/>.
    /// </summary>
    public record ParserValue(Mark Start, Mark End, Rune Value) : ParserOutput(Start, End)
    {
        /// <inheritdoc />
        public override string StringValue
        {
            get
            {
                if (EOF)
                    return "EOF";

                return Value.Value == 0 ? String.Empty : Value.ToString();
            }
        }

        /// <summary>
        /// Turns this ParserValue into an equivalent ParserString
        /// </summary>
        public ParserString AsString()
        {
            return new(Start, End, StringValue) { EOF = EOF };
        }

        /// <summary>
        /// Creates an EOF output at the specified mark.
        /// </summary>
        /// <param name="mark">The mark at which the EOF occurred.</param>
        /// <returns>A parser output with EOF set to true and both Start and End marks set to <paramref name="mark"/>.</returns>
        public static ParserValue CreateEOF(Mark mark)
        {
            return new ParserValue(mark, mark, new(0)) { EOF = true };
        }
    }

}
