using System.IO;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public abstract class Token
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

        // public bool DiscardValue { get; set; } = true;

        protected abstract bool TryMatchInternal(ReadOnlySpan<byte> bytes, out int tokenEnd, ICollection<TokenMatch>? values = null);

        public bool TryMatch(ReadOnlySpan<byte> bytes, out int tokenEnd, [MaybeNullWhen(false)] out TokenMatch match, bool noMatch = false)
        {
            Debug.Indent();
            Debug.WriteLine($"Running {this}"); // on { String.Join(' ', bytes.ToArray().Select(b => $"{b,2:X}")) }");

            var list = noMatch ? null : new List<TokenMatch>();
            var value = TryMatchInternal(bytes, out tokenEnd, list);

            // if (value && tokenEnd == 0) // && !(this is RepeatingToken) && !(this is IgnoreToken t && t.BaseToken is RepeatingToken))
            //     throw new InvalidOperationException("Can't return true and not advance.");

            if (value && list != null)
            {
                match = new TokenMatch(this, list);

                if (Combine)
                    match = match.Collapse();

                Debug.WriteLine($"Matched {this}: {match}");
            }
            else
            {
                match = null;
            }

            Debug.WriteLine($"Running {this}: {value}");
            Debug.Unindent();

            return value;
        }

        public void Execute(ReadOnlySpan<byte> bytes, out TokenMatch match)
        {
            if (!TryMatch(bytes, out var end, out match))
            {
                var line = 1;
                var col = 1;

                foreach (var b in bytes)
                {
                    if (end-- <= 0)
                        break;

                    if (b == '\n')
                    {
                        line++;
                        col = 1;
                    }
                }

                throw new FormatException($"Parsing failed on line {line} column {col}.");
            }

            if (match == null)
                throw new NullReferenceException($"The execution succeeded but failed to return a TokenMatch.");
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
            return new ExclusiveToken(lhs, rhs);
        }

        public static Token operator !(Token t)
        {
            return new IgnoreToken(t);
            // var clone = (Token) t.MemberwiseClone();
            // clone.DiscardValue = false;

            // return clone;
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
                case null:
                    return Name ?? GetSyntax();

                default:
                    throw new ArgumentException($"Unknown format: {format}");
            }
        }
    }
}
