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

        internal override bool TryMatchInternal(PartialExecutionData data, ref SequenceReader<byte> reader, out OperationStatus status)
        {
            return BaseToken.TryMatch(data.GetIndexed(0, true), ref reader, out _, out status);
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
