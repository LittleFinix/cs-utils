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

        protected override bool TryMatchInternal(ReadOnlySpan<byte> bytes, out int tokenEnd, ICollection<TokenMatch>? values = null)
        {
            tokenEnd = 0;

            foreach (var token in Tokens)
            {
                if (!token.TryMatch(bytes[tokenEnd..], out var tend, out var match, values == null))
                    return false;

                if (match != null)
                    values?.Add(match);

                tokenEnd += tend;
            }

            return true;
        }
    }
}
