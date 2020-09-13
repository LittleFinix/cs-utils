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

        protected override bool TryMatchInternal(ref SequenceReader<byte> reader, ICollection<TokenMatch>? values, out OperationStatus status)
        {
            if (reader.Remaining < Match.Length)
                return DoneWith(status = OperationStatus.NeedMoreData);

            if (!reader.IsNext(Match.Span))
                return DoneWith(status = OperationStatus.InvalidData);

            var data = new byte[Match.Length];

            if (values != null && reader.TryCopyTo(data.AsSpan()))
                values.Add(new TokenMatch(this, data));

            reader.Advance(data.LongLength);

            return DoneWith(status = OperationStatus.Done);
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
