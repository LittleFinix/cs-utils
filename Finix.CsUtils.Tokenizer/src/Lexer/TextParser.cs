using System.IO.Compression;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Linq;
using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Finix.CsUtils
{
    /// <summary>
    /// A parser that reads text from a TextReader and stores returned values in a buffer.
    /// </summary>
    public class TextParser : ITextParser
    {
        private readonly MarkingReaderBase reader;

        private readonly LinkedList<ParserString> buffer = new();

        /// <summary>
        /// Creates a new <see cref="TextParser"/>.
        /// </summary>
        /// <param name="reader">A marking reader.</param>
        public TextParser(MarkingReaderBase reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Creates a new <see cref="TextParser"/>.
        /// </summary>
        /// <param name="reader">A text reader.</param>
        public TextParser(TextReader reader)
            : this(new MarkingReader(reader))
        {

        }

        /// <summary>
        /// Creates a new <see cref="TextParser"/>.
        /// </summary>
        /// <param name="reader">A stream that supports reading.</param>
        public TextParser(Stream stream)
            : this(new StreamReader(stream))
        {

        }

        /// <summary>
        /// Creates a new <see cref="TextParser"/>.
        /// </summary>
        /// <param name="reader">A string.</param>
        public TextParser(string text)
            : this(new StringReader(text))
        {

        }

        /// <inheritdoc/>
        public bool IsBranch => false;

        /// <inheritdoc/>
        public Mark Position => buffer.First?.Value?.Start ?? reader.Position;

        private bool Expand()
        {
            // Read a line from the input and append it to the buffer.

            var line = reader.ReadLine(true);

            if (line.Length == 0 && line.EOF)
                return false;

            buffer.AddLast(line);

            return true;
        }

        private int BufferSize => buffer.Aggregate(0, (count, str) => count + str.Length);

        private bool TryPop([MaybeNullWhen(false)] out ParserString str)
        {
            str = null;

            if (buffer.First is null)
                return false;

            str = buffer.First.Value;
            buffer.RemoveFirst();

            return true;
        }

        // /// <inheritdoc/>
        // public IParsingBranch Branch()
        // {
        //     return new ParsingBranch(this);
        // }

        /// <inheritdoc/>
        public void Commit()
        {
            throw new InvalidOperationException("The current text parser is not a branch and therefore can't be joined.");
        }

        /// <inheritdoc />
        public ParserString Read(int count = 1)
        {
            var start = Position;
            var eof = false;

            while (BufferSize < count)
            {
                if (eof = !Expand())
                    break;
            }

            var str = String.Empty;
            var end = Position;

            while (TryPop(out var next))
            {
                if (next.Length > count)
                {
                    PutBack(next[count..]);
                    next = next[..count];
                }

                str += next.StringValue;
                end = next.End;

                count -= next.Length;

                if (count <= 0)
                    break;
            }

            return new ParserString(start, end, str) { EOF = eof };
        }

        /// <inheritdoc/>
        public void PutBack(ParserString str)
        {
            if (str.Length > 0)
                buffer.AddFirst(str);
        }

        /// <inheritdoc/>
        public ParserString Peek(int count = 1)
        {
            // Read the next rune and put it back.
            var r = Read(count);
            PutBack(r);

            return r;
        }

        /// <inheritdoc/>
        public ParserString ReadUntil(Predicate<Rune> matcher, Rune? escape, out ParserValue matched)
        {
            matched = new(Position, Position, new Rune(0));

            // If the buffer is empty and we're unable to expand it, return EOF instead of doing anything.
            if (buffer.Count == 0 && !Expand())
                return ParserValue.CreateEOF(Position).AsString();

            var start = Position;
            var end = Position;
            var str = new StringBuilder();
            var eof = false;

            do
            {
                var isEscaped = false;

                // If the buffer is empty continue, which calls Expand (see below.)
                if (buffer.Count == 0)
                    continue;

                // Iterate through each rune of each memory chunk of the buffer.
                // For each rune we test if the predicate is true and that the character is not escaped.
                while (TryPop(out var chunk))
                {
                    var i = 0;

                    foreach (var rune in chunk)
                    {
                        // Test if the predicate is true and we're not in 'escaped'-mode
                        if (matcher(rune) && rune != escape && !isEscaped)
                        {
                            matched = new(Position, Position.MoveMarkUp(rune), rune);

                            // Append the chunk up until the current rune to the local buffer and
                            // add the rest back to the class-wide buffer.
                            str.Append(chunk[..i].StringValue);
                            buffer.AddFirst(chunk[i..]);

                            goto found;
                        }

                        // Increase the counter according to the utf-16 sequence length.
                        i++;

                        // If we aren't already escaped and the current rune is the escape character, set isEscaped true (false otherwise.)
                        isEscaped = !isEscaped && rune == escape;
                    }

                    // Append the entire chunk and set the end of the ParserString.
                    str.Append(chunk.StringValue);
                    end = chunk.End;
                }
            }
            while (Expand()); // Continue as long as we can successfully expand the buffer.

            eof = true;

        found:

            return new(start, Position, str.ToString()) { EOF = eof };
        }

        /// <inheritdoc />
        public bool StartsWith(string expect, [MaybeNullWhen(false)] out string actual, IEqualityComparer<string?>? comparer = null)
        {
            // Set the default string comparer if needed.
            comparer ??= StringComparer.Ordinal;
            actual = String.Empty;

            // Ensure our buffer contains enough characters.
            while (BufferSize < expect.Length)
            {
                // If we can't expand further then the expected string can't possibly match.
                if (!Expand())
                    return false;
            }

            // Iterate through all chunks in the buffer.
            foreach (var chunk in buffer)
            {
                // Get the smaller length.
                var n = Math.Min(chunk.Length, expect.Length);

                // Get as much as possible of the chunk.
                var next = chunk[..n].StringValue;

                // Append the chunk onto our output buffer.
                actual += next;

                // If the chunk's has more characters than the rest of our string, we can finish
                // the comparison in this iteration.
                if (chunk.Length >= expect.Length)
                    return comparer.Equals(next, expect);

                // Otherwise, if the strings don't match, return false.
                else if (!comparer.Equals(next, expect[..n]))
                    return false;

                // Truncate the left side of our string as needed.
                expect = expect[n..];
            }

            // If there were no more chunks left in the buffer we return false.
            // This should technically never be reached, since we check with Expand
            // if there are enough characters in our buffer.
            return false;
        }

        /// <inheritdoc/>
        public ParserString ReadLine()
        {
            // Read until '\r' or '\n'.
            var line = this.ReadUntilAny(new[] { '\r', '\n' }, out var match);

            // Consume the matched character and check if it's '\r'
            if (Read().Runes[0].Value == '\r')
            {
                // If the next character is '\n' consume it as well (\r\n, DOS-lineending),
                // otherwise put it back.
                var r = Read();

                if (r.Runes[0].Value != '\n')
                    PutBack(r);
            }

            return line;
        }
    }
}
