using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System;

namespace Finix.CsUtils
{
    public sealed class SilentToken : Token
    {
        public override string? Name
        {
            get => BaseToken.Name;
            set => BaseToken.Name = value;
        }

        public SilentToken(Token baseToken)
        {
            BaseToken = baseToken;
        }

        public Token BaseToken { get; }

        protected override bool TryMatchInternal(ref SequenceReader<byte> reader, ICollection<TokenMatch>? values, out OperationStatus status)
        {
            return BaseToken.TryMatch(ref reader, out _, true, out status);
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
