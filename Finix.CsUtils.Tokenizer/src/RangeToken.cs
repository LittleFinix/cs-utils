using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;
using System.Buffers;
using System;

namespace Finix.CsUtils
{
    public sealed class RangeToken : Token
    {
        public ReadOnlyMemory<byte> Low { get; }

        public ReadOnlyMemory<byte> High { get; }

        public RangeToken(ReadOnlySpan<byte> low, ReadOnlySpan<byte> high)
        {
            Low = low.ToArray();
            High = high.ToArray();

            if (Low.Length != High.Length)
                throw new ArgumentException("'low' and 'high' must be of the same length.");
        }

        public RangeToken(byte low, byte high)
            : this(new[] { low }, new[] { high })
        {
        }

        public RangeToken(uint low, uint high)
            : this(BitConverter.GetBytes(low), BitConverter.GetBytes(high))
        {
        }

        public override string GetSyntax()
        {
            var low = new BigInteger(Low.Span, true);
            var high = new BigInteger(High.Span, true);

            unchecked
            {
                if (Char.IsLetterOrDigit((char) low) && Char.IsLetterOrDigit((char) high))
                    return $"{(char) low}-{(char) high}";
            }

            return $"%x{low:X}-{high:X}";
        }

        protected override bool TryMatchInternal(ref SequenceReader<byte> reader, ICollection<TokenMatch>? values, out OperationStatus status)
        {
            if (reader.Remaining < Low.Length)
                return DoneWith(status = OperationStatus.NeedMoreData);

            var value = new byte[Low.Length];

            if (!reader.TryCopyTo(value))
                return DoneWith(status = OperationStatus.DestinationTooSmall);

            reader.Advance(value.LongLength);

            var lo = Low.Span.SequenceCompareTo(value);
            var hi = High.Span.SequenceCompareTo(value);

            if (lo > 0 || hi < 0)
                return DoneWith(status = OperationStatus.InvalidData);

            if (values != null)
                values.Add(new TokenMatch(this, value));

            return DoneWith(status = OperationStatus.Done);
        }
    }
}
