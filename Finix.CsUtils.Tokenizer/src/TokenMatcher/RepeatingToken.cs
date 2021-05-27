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
            status = OperationStatus.Done;

            if (data.Index > Max)
            {
                data.MayContinue = false;
                return false;
            }

            if (data.Consumed != 0)
                reader.Advance(data.Consumed - reader.Consumed);

            for (; data.Index <= Max; data.Index++)
            {
                data.MayContinue = false;

                var at = reader.Consumed;
                if (!BaseToken.TryMatch(data.GetIndexed(data.Index, revokeAuthority: data.Index >= Min), ref reader, out var match, out status) || reader.Consumed == at)
                    goto end;

                data.Consumed = reader.Consumed;
                data.ClearData(data.Index);
                data.AddMatch(data.Index, match);

                if (data.Index >= Min)
                {
                    data.MayContinue = true;
                    goto end;
                }
            }

        end:
            if (data.Index < Min)
            {
                status = OperationStatus.NeedMoreData;
                return false;
            }
            else
            {
                if (status != OperationStatus.NeedMoreData)
                    status = OperationStatus.Done;
                else
                    data.MayContinue = true;

                return true;
            }
        }
    }
}
