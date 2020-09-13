using System.Text;
using System.Buffers;
using System.IO;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public abstract class Token : ICloneable
    {
        public static readonly Token ALPHA = (T(0x41, 0x5A) / T(0x61, 0x7A)).Named("ALPHA");

        public static readonly Token CHAR = T(0, 0x7F).Named("CHAR");

        public static readonly Token CR = T('\r').Named("CR");

        public static readonly Token LF = T('\n').Named("LF");

        public static readonly Token CRLF = (CR + LF).Named("CRLF");

        public static readonly Token LFCR = (LF + CR).Named("LFCR");

        public static readonly Token EOL = (CRLF / LFCR / CR / LF).Named("EOL");

        public static readonly Token CTL = (T(0x00, 0x1F) / 0x7F).Named("CTL");

        public static readonly Token DIGIT = T(0x30, 0x39).Named("DIGIT");

        public static readonly Token DQUOTE = T('"').Named("DQUOTE");

        public static readonly Token HEXDIG = (DIGIT / T('a', 'f') / T('A', 'F')).Named("HEXDIG");

        public static readonly Token HTAB = T(0x09).Named("HTAB");

        public static readonly Token SP = T(0x20).Named("SP");

        public static readonly Token WSP = (SP / HTAB).Named("WSP");

        public static readonly Token LWSP = (0 * (WSP / (CRLF + WSP))).Named("LWSP");

        public static readonly Token AWSP = (WSP / EOL).Named("AWSP");

        public static readonly Token VCHAR = T(0x21, 0x7E).Named("VCHAR");

        public static readonly Token OCTET = T(0x00, 0xFF).Named("OCTET");

        public static readonly Token UTF8_Tail = T(0x80, 0xBF).Named("UTF8-tail");

        public static readonly Token UTF8_1 = T(0, 0x7F).Named("UTF8-1");

        public static readonly Token UTF8_2 = (
            T(0xC2, 0xDF) + UTF8_Tail
        ).Named("UTF8-2");

        public static readonly Token UTF8_3 = (
            (T(0xE0) + T(0xA0, 0xBF) + UTF8_Tail) /
            (T(0xE1, 0xEC) + (2, 2) * UTF8_Tail) /
            (T(0xED) + T(0x80, 0x9F) + UTF8_Tail) /
            (T(0xEE, 0xEF) + (2, 2) * UTF8_Tail)
        ).Named("UTF8-3");

        public static readonly Token UTF8_4 = (
            (T(0xF0) + T(0x90, 0xBF) + (2, 2) * UTF8_Tail) /
            (T(0xF1, 0xF3) + (3, 3) * UTF8_Tail) /
            (T(0xF4) + T(0x80, 0x8F) + (2, 2) * UTF8_Tail)
        ).Named("UTF8-4");

        public static readonly Token UTF8 = (
            UTF8_1 / UTF8_2 / UTF8_3 / UTF8_4
        ).Named("UTF8-char");

        public static readonly Token UTF8_Octets = (0 * UTF8).Named("UTF8-octets");

        public virtual string? Name { get; set; }

        public bool Combine { get; set; }

        public bool Debug { get; set; } = false;

        public bool IsAuthoritative { get; set; } = false;

        // public bool DiscardValue { get; set; } = true;

        protected bool DoneWith(OperationStatus status)
        {
            return status == OperationStatus.Done;
        }

        internal abstract bool TryMatchInternal(PartialExecutionData ped, ref SequenceReader<byte> reader, out OperationStatus status);

        internal bool TryMatch(PartialExecutionData data, ref SequenceReader<byte> reader, [MaybeNullWhen(false)] out TokenMatch match, out OperationStatus status)
        {
#if TRACE
            var seq = reader.Sequence.Slice(reader.Consumed);
#endif

            Trace.Indent();
            Trace.WriteLineIf(Debug, $"Running {this} on {new TokenMatch(this, seq.ToArray())}");

            data.Authoritative |= IsAuthoritative;

            if (data.AuthoritativeSource == null || IsAuthoritative)
                data.AuthoritativeSource = this;

            var tempReader = reader;
            var ok = TryMatchInternal(data, ref tempReader, out status);

            if (ok)
                reader = tempReader;
            else if (data.Authoritative)
                ThrowParsingException(tempReader, data.AuthoritativeSource);

            if (ok && data.Matches is IEnumerable<TokenMatch> list)
            {
                match = new TokenMatch(this, list);

                if (Combine)
                    match = match.Collapse();

                Trace.WriteLineIf(Debug, $"Matched {this}: {match}");
            }
            else
            {
                match = null;
            }

            Trace.WriteLineIf(Debug, $"Running {this}: {status} (ok: {ok})");
            Trace.Unindent();

            return ok;
        }

        public bool Execute(ref SequenceReader<byte> reader, out TokenMatch? match, out OperationStatus status)
        {
            var ped = new object();
            var tempReader = reader;

            var ok = ExecutePartial(ref reader, out match, out status, ref ped);

            if (status == OperationStatus.Done)
                ok = true;

            if (!ok)
                reader = tempReader;

            // match = preMatch ?? throw new NullReferenceException($"The execution succeeded but failed to return a TokenMatch.");

            return ok;
        }

        public bool Execute(ReadOnlySequence<byte> bytes, out TokenMatch? match, out OperationStatus status)
        {
            var sr = new SequenceReader<byte>(bytes);

            return Execute(ref sr, out match, out status);
        }

        public bool Execute(byte[] bytes, out TokenMatch? match, out OperationStatus status)
        {
            var seq = new ReadOnlySequence<byte>(bytes);

            return Execute(seq, out match, out status);
        }

        internal bool ExecutePartial(ref SequenceReader<byte> reader, out TokenMatch? match, out OperationStatus status, ref object? partialData)
        {
            if (!(partialData is PartialExecutionData ped))
                partialData = ped = new PartialExecutionData();

            var start = reader.Position;
            var tempReader = reader;

            if (!TryMatch(ped, ref reader, out match, out status) && status != OperationStatus.NeedMoreData)
            {
                var errReader = reader;
                reader = tempReader;

                ThrowParsingException(errReader);
            }

            return status == OperationStatus.NeedMoreData;
        }

        public Token Named(string name)
        {
            Name = name;
            return this;
        }

        public Token Combined(bool combined = true)
        {
            Combine = combined;
            return this;
        }

        public Token Debugging(bool enabled = true, bool recurse = false)
        {
            var n = (Token) Clone();
            n.Debug = enabled;

            if (recurse && n is MultiToken mt)
            {
                mt.Tokens = mt.Tokens
                    .Select(t => t.Debugging(enabled, recurse))
                    .ToArray();
            }

            return n;
        }

        public Token Authoritative(bool enable = true)
        {
            IsAuthoritative = enable;
            return this;
        }

        public virtual string GetName()
        {
            return Name ?? GetType().Name;
        }

        public abstract string GetSyntax();

        public override string ToString()
        {
            return ToString("G");
        }

        public virtual string ToString(string? format)
        {
            switch (format?.ToUpperInvariant())
            {
                case "N":
                    return GetName();

                case "S":
                    return GetSyntax();

                case "NS":
                case "G":
                case "":
                case null:
                    return Name ?? GetSyntax();

                default:
                    throw new ArgumentException($"Unknown format: {format}");
            }
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        [DoesNotReturn]
        private static void ThrowParsingException(SequenceReader<byte> reader, Token? authoritativeSource = null)
        {
            var end = reader.Consumed;
            var line = 1;
            var col = 1;

            reader.Rewind(end);

            while (reader.TryRead(out var b))
            {
                if (end-- <= 0)
                    break;

                col++;

                if (b == '\n')
                {
                    line++;
                    col = 1;
                }
            }

            if (authoritativeSource != null)
                throw new FormatException($"Parsing failed on line {line} column {col}, expected '{authoritativeSource}'.");
            else
                throw new FormatException($"Parsing failed on line {line} column {col}.");
        }

        public static Token I(char c)
        {
            return (T(Char.ToUpperInvariant(c)) / Char.ToLowerInvariant(c)).Named($"\"{c}\"");
        }

        public static Token I(string str)
        {
            return new CombinedToken(str.Select(I)).Named($"\"{str}\"");
        }

        public static Token T(byte b)
        {
            return b;
        }

        public static Token T(char c)
        {
            return ((Token) c).Named($"\"{c}\"");
        }

        public static Token T(string str)
        {
            return ((Token) str).Named($"\"{str}\"");
        }

        public static Token T(byte low, byte high)
        {
            return new RangeToken(low, high);
        }

        public static Token T(char low, char high)
        {
            return new RangeToken(low, high);
        }

        public static Token R(Token token, byte replace)
        {
            return new ReplaceToken(token, new[] { replace });
        }

        public static Token R(Token token, byte[] replace)
        {
            return new ReplaceToken(token, replace);
        }

        public static Token R(Token token, string replace)
        {
            return new ReplaceToken(token, Encoding.UTF8.GetBytes(replace));
        }

        public static Token R(Token token, char replace)
        {
            return R(token, replace.ToString());
        }

        public static Token R(Token token, Func<TokenMatch, byte[]> replace)
        {
            return new ReplaceToken(token, replace);
        }

        public static implicit operator Token(string str)
        {
            return new StaticToken(str);
        }

        public static implicit operator Token(char c)
        {
            return new StaticToken(c);
        }

        public static implicit operator Token(byte b)
        {
            return new StaticToken(b);
        }

        public static Token operator *(int min, Token token)
        {
            return new RepeatingToken(token, min);
        }

        public static Token operator *((int min, int max) mm, Token token)
        {
            return new RepeatingToken(token, mm.min, mm.max);
        }

        public static Token operator *((int min, int max, bool combine) mm, Token token)
        {
            return new RepeatingToken(token, mm.min, mm.max, mm.combine);
        }

        public static Token operator *((int min, bool combine) mm, Token token)
        {
            return new RepeatingToken(token, mm.min, combine: mm.combine);
        }

        public static Token operator /(Token lhs, Token rhs)
        {
            return new AlternativesToken(lhs, rhs);
        }

        public static Token operator +(Token lhs, Token rhs)
        {
            return new CombinedToken(lhs, rhs);
        }

        public static Token operator -(Token lhs, Token rhs)
        {
            return new ExclusionToken(lhs, rhs);
        }

        public static Token operator !(Token t)
        {
            return new SilentToken(t);
            // var clone = (Token) t.MemberwiseClone();
            // clone.DiscardValue = false;

            // return clone;
        }
    }
}
