using System.Diagnostics;
using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System;

namespace Finix.CsUtils
{
    public sealed class AlternativesToken : MultiToken
    {
        public AlternativesToken(IEnumerable<Token> tokens)
            : base(tokens)
        {
        }

        public AlternativesToken(params Token[] tokens)
            : base(tokens)
        {
        }

        public override string GetSyntax()
        {
            return String.Join(" / ", Tokens.Select(t => t.ToString()));
        }

        internal override bool TryMatchInternal(PartialExecutionData data, ref SequenceReader<byte> reader, out OperationStatus status)
        {
            status = OperationStatus.InvalidData;
            var at = reader.Consumed;

            data.ClearData();

            var i = 0;
            foreach (var token in Tokens)
            {
                if (token.TryMatch(data.GetIndexed(i++, revokeAuthority: true), ref reader, out var match, out var tempStatus))
                {
                    if (reader.Consumed == at)
                        continue;

                    data.AddMatch(-1, match);
                    status = tempStatus;

                    return true;
                }

                if (tempStatus == OperationStatus.NeedMoreData)
                    status = tempStatus;
            }

            return false;
        }
    }
}
