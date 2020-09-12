using System.Diagnostics;
using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System;

namespace Finix.CsUtils
{
    public sealed class ExclusiveToken : Token
    {
        public ExclusiveToken(Token include, Token exclude)
        {
            Include = include;
            Exclude = exclude;
        }

        public Token Include { get; }

        public Token Exclude { get; }

        public override string GetSyntax()
        {
            return $"( {Include} - {Exclude} )";
        }

        protected override bool TryMatchInternal(ReadOnlySpan<byte> bytes, out int tokenEnd, ICollection<TokenMatch>? values = null)
        {
            tokenEnd = 0;

            var inc = Include.TryMatch(bytes, out tokenEnd, out var match, values == null);
            var exc = Exclude.TryMatch(bytes, out _, out _, true);

            if (!inc || exc)
            {
                tokenEnd = 0;
                return false;
            }

            if (match != null)
                values?.Add(match);

            return true;
        }
    }
}
