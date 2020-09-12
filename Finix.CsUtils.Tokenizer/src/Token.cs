using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public abstract class Token
    {
        public static readonly Token ALPHA = (T(0x41, 0x5A) + T(0x61, 0x7A)).Named("ALPHA");

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

        public virtual string? Name { get; set; }

        public bool Combine { get; set; }

        protected abstract bool TryMatchInternal(ReadOnlySpan<byte> bytes, out int tokenEnd, ICollection<TokenMatch>? values = null);

        public bool TryMatch(ReadOnlySpan<byte> bytes, out int tokenEnd, [MaybeNullWhen(false)] out TokenMatch match, bool noMatch = false)
        {
            Debug.Indent();
            Debug.WriteLine($"Running {this} on { String.Join(' ', bytes.ToArray().Select(b => $"{b,2:X}")) }");

            var list = noMatch ? null : new List<TokenMatch>();
            var value = TryMatchInternal(bytes, out tokenEnd, list);

            if (value && tokenEnd == 0) // && !(this is RepeatingToken) && !(this is IgnoreToken t && t.BaseToken is RepeatingToken))
                throw new InvalidOperationException("Can't return true and not advance.");

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

        public static Token T(byte b)
        {
            return b;
        }

        public static Token T(char b)
        {
            return b;
        }

        public static Token T(byte low, byte high)
        {
            return new RangeToken(low, high);
        }

        public static Token T(char low, char high)
        {
            return new RangeToken(low, high);
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
