using System.Linq;
using System.Numerics;
using System.Diagnostics;
using System.Buffers;
using System;
using System.Collections.Generic;

namespace Finix.CsUtils
{
    public sealed class StaticToken : Token
    {
        public ReadOnlyMemory<byte> Match { get; }

        public StaticToken(char match)
        {
            Match = BitConverter.GetBytes(match)
                .Reverse()
                .SkipWhile(b => b == 0)
                .Reverse()
                .ToArray();
        }

        public StaticToken(byte match)
        {
            Match = new[] { match };
        }

        public StaticToken(ReadOnlySpan<byte> match)
        {
            Match = match.ToArray();
        }

        public override string GetSyntax()
        {
            var match = new BigInteger(Match.Span);

            unchecked
            {
                if (Char.IsLetterOrDigit((char) match))
                    return $"{(char) match}";
            }

            return $"%x{match:X}";
        }

        protected override bool TryMatchInternal(ReadOnlySpan<byte> bytes, out int tokenEnd, ICollection<TokenMatch>? values = null)
        {

            tokenEnd = 0;

            if (bytes.Length < Match.Length || !bytes[..Match.Length].SequenceEqual(Match.Span))
                return false;

            tokenEnd = Match.Length;

            if (values != null)
                values.Add(new TokenMatch(this, bytes[..tokenEnd]));

            return true;
        }

        public static Token Invariant(char c)
        {
            return new CombinedToken {
                new StaticToken(Char.ToUpperInvariant(c)),
                new StaticToken(Char.ToLowerInvariant(c)),
            };
        }

        public static implicit operator StaticToken(char c)
        {
            return new StaticToken(c);
        }

        public static implicit operator StaticToken(byte b)
        {
            return new StaticToken(b);
        }
    }
}
