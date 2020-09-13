using System.Text;
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

        public StaticToken(string str)
        {
            Match = Encoding.UTF8.GetBytes(str);
        }

        public override string GetSyntax()
        {
            var match = new BigInteger(Match.Span, true);

            unchecked
            {
                if (Char.IsLetterOrDigit((char) match) || Char.IsSymbol((char) match))
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

        public static implicit operator StaticToken(string str)
        {
            return new StaticToken(str);
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
