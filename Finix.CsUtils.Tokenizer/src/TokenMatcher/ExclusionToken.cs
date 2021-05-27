using System.Diagnostics;
using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System;

namespace Finix.CsUtils
{
    public sealed class ExclusionToken : Token
    {
        public ExclusionToken(Token include, Token exclude)
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

        internal override bool TryMatchInternal(PartialExecutionData data, ref SequenceReader<byte> reader, out OperationStatus status)
        {
            var at = reader.Consumed;
            var tempReader = reader;

            data.ClearData();
            var inc = Include.TryMatch(data.GetIndexed(0), ref reader, out var match, out status);

            if (inc)
            {
                if (Exclude.TryMatch(data.GetIndexed(1, true, true), ref tempReader, out _, out var tempStatus))
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

            data.AddMatch(0, match);

            return true;
        }
    }
}
