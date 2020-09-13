using System.Diagnostics;
using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System;

namespace Finix.CsUtils
{
    public sealed class CombinedToken : MultiToken
    {
        public CombinedToken(IEnumerable<Token> tokens)
            : base(tokens)
        {
        }

        public CombinedToken(params Token[] tokens)
            : base(tokens)
        {
        }

        public override string GetSyntax()
        {
            return "( " + String.Join(" + ", Tokens.Select(t => t.ToString())) + " )";
        }

        internal override bool TryMatchInternal(PartialExecutionData data, ref SequenceReader<byte> reader, out OperationStatus status)
        {
            status = OperationStatus.Done;

            data.ClearData(data.Index);
            foreach (var token in Tokens.Skip(data.Index))
            {
                if (!token.TryMatch(data.GetIndexed(data.Index), ref reader, out var match, out status))
                {
                    data.Index--;
                    return false;
                }

                data.AddMatch(data.Index, match);

                // if (status == OperationStatus.NeedMoreData)
                //     return true;

                data.Index++;
            }

            return true;
        }
    }
}
