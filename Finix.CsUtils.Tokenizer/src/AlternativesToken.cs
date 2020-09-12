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

        protected override bool TryMatchInternal(ReadOnlySpan<byte> bytes, out int tokenEnd, ICollection<TokenMatch>? values = null)
        {

            tokenEnd = 0;
            foreach (var token in Tokens)
            {
                if (token.TryMatch(bytes, out tokenEnd, out var match, values == null))
                {
                    if (match != null)
                        values?.Add(match);

                    return true;
                }
            }

            return false;
        }
    }
}
