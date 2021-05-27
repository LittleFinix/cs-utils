using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System;

namespace Finix.CsUtils
{
    public sealed class ReplaceToken : Token
    {
        public override string? Name
        {
            get => BaseToken.Name;
            set => BaseToken.Name = value;
        }

        public ReplaceToken(Token baseToken, Func<TokenMatch, byte[]> replacer)
        {
            BaseToken = baseToken;
            Replacer = replacer;
        }

        public ReplaceToken(Token baseToken, byte[] bytes)
            : this(baseToken, _ => bytes)
        {
        }

        public Token BaseToken { get; }

        public Func<TokenMatch, byte[]> Replacer { get; set; }

        internal override bool TryMatchInternal(PartialExecutionData data, ref SequenceReader<byte> reader, out OperationStatus status)
        {
            data.MatchMap = new Dictionary<int, List<TokenMatch>>();
            if (!BaseToken.TryMatch(data.GetIndexed(0), ref reader, out var match, out status))
                return false;

            if (IsLiteral)
                data.AddMatch(0, match);
            else
                data.AddData(this, Replacer(match));

            return true;
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
