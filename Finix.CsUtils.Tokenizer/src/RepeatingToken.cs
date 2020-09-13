using System.Diagnostics;
using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System;

namespace Finix.CsUtils
{
    public sealed class RepeatingToken : Token
    {
        public RepeatingToken(Token baseToken, int min = 0, int max = Int32.MaxValue, bool combine = false)
        {
            BaseToken = baseToken;
            Min = min; // > 0 ? min : 1;
            Max = max;
            Combine = combine;
        }

        public Token BaseToken { get; }

        public int Min { get; }

        public int Max { get; }

        public override string GetSyntax()
        {
            var min = Min == 0 ? "" : Min.ToString();
            var max = Max == Int32.MaxValue ? "" : Max.ToString();

            return $"{min}*{max}({BaseToken})";
        }

        protected override bool TryMatchInternal(ref SequenceReader<byte> reader, ICollection<TokenMatch>? values, out OperationStatus status)
        {
            status = OperationStatus.Done;

            var i = 0;
            var tempReader = reader;
            for (; i < Max; i++)
            {
                var at = reader.Consumed;
                if (!BaseToken.TryMatch(ref reader, out var match, values == null, out status) || reader.Consumed == at)
                    goto end;

                if (match != null)
                    values?.Add(match);
            }

        end:
            if (i < Min && status == OperationStatus.Done)
            {
                status = OperationStatus.NeedMoreData;
                return false;
            }
            else
            {
                if (status != OperationStatus.NeedMoreData)
                    status = OperationStatus.Done;

                return true;
            }
        }
    }
}
