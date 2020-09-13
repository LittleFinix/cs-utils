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

        internal override bool TryMatchInternal(PartialExecutionData data, ref SequenceReader<byte> reader, out OperationStatus status)
        {
            if (data.Index > Max)
                throw new IndexOutOfRangeException("Index must be less than or equal to Max");

            status = OperationStatus.Done;

            for (; data.Index <= Max; data.Index++)
            {
                var at = reader.Consumed;
                if (!BaseToken.TryMatch(data.GetIndexed(data.Index, revokeAuthority: data.Index >= Min), ref reader, out var match, out status) || reader.Consumed == at)
                    goto end;

                data.AddMatch(data.Index, match);
            }

        end:
            if (data.Index < Min && status == OperationStatus.Done)
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
