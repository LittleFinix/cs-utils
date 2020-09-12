using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System;

namespace Finix.CsUtils
{
    public sealed class IgnoreToken : Token
    {
        public override string? Name
        {
            get => BaseToken.Name;
            set => BaseToken.Name = value;
        }

        public IgnoreToken(Token baseToken)
        {
            BaseToken = baseToken;
        }

        public Token BaseToken { get; }

        protected override bool TryMatchInternal(ReadOnlySpan<byte> bytes, out int tokenEnd, ICollection<TokenMatch>? values = null)
        {
            return BaseToken.TryMatch(bytes, out tokenEnd, out _, true);
        }

        public override string GetName()
        {
            return BaseToken.GetName();
        }

        public override string GetSyntax()
        {
            return BaseToken.GetSyntax();
        }
    }
}
