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

        protected override bool TryMatchInternal(ref SequenceReader<byte> reader, ICollection<TokenMatch>? values, out OperationStatus status)
        {
            status = OperationStatus.InvalidData;
            var at = reader.Consumed;

            foreach (var token in Tokens)
            {
                if (token.TryMatch(ref reader, out var match, values == null, out var tempStatus))
                {
                    status = tempStatus;

                    if (reader.Consumed == at)
                        continue;

                    if (match != null)
                        values?.Add(match);

                    return true;
                }

                if (tempStatus == OperationStatus.NeedMoreData)
                    status = tempStatus;
            }

            return false;
        }
    }
}
