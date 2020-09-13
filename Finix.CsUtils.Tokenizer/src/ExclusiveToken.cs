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

        protected override bool TryMatchInternal(ref SequenceReader<byte> reader, ICollection<TokenMatch>? values, out OperationStatus status)
        {
            var at = reader.Consumed;
            var tempReader = reader;

            var inc = Include.TryMatch(ref reader, out var match, values == null, out status);

            if (inc)
            {
                if (Exclude.TryMatch(ref tempReader, out _, true, out var tempStatus))
                {
                    status = tempStatus;
                    reader = tempReader;
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (match != null)
                values?.Add(match);

            return true;
        }
    }
}
