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
            data.MayContinue = true;

            foreach (var token in Tokens.Skip(data.Index))
            {
                try
                {
                    var d = data.GetIndexed(data.Index, revokeAuthority: true);

                    // var tempReader = reader;
                    if (token.TryMatch(d, ref reader, out var match, out var tempStatus))
                    {
                        if (reader.Consumed == at)
                            continue;

                        data.AddMatch(data.Index, match);
                        status = tempStatus;

                        if (d.MayContinue)
                            data.Index--;

                        return true;
                    }

                    if (tempStatus == OperationStatus.NeedMoreData)
                        status = tempStatus;
                }
                finally
                {
                    data.Index++;
                }
            }

            data.MayContinue = false;

            return false;
        }
    }
}
