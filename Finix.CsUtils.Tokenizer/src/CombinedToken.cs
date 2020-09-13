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

        protected override bool TryMatchInternal(ref SequenceReader<byte> reader, ICollection<TokenMatch>? values, out OperationStatus status)
        {
            status = OperationStatus.Done;

            foreach (var token in Tokens)
            {
                if (!token.TryMatch(ref reader, out var match, values == null, out status))
                {
                    return false;
                }

                if (match != null)
                    values?.Add(match);
            }

            return true;
        }
    }
}
