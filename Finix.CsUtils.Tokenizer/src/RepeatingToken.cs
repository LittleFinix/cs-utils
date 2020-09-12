using System.Diagnostics;
using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System;

namespace Finix.CsUtils
{
    public sealed class RepeatingToken : Token
    {
        public RepeatingToken(Token baseToken, int min = 0, int max = Int32.MaxValue, bool combine = false)
        {
            BaseToken = baseToken;
            Min = min > 0 ? min : 1;
            Max = max;
            Combine = combine;
        }

        public Token BaseToken { get; }

        public int Min { get; }

        public int Max { get; }

        public override string GetSyntax()
        {
            var min = Min == 0 ? "" : Min.ToString();
            var max = Max == Int32.MaxValue ? "" : Max.ToString();

            return $"{min}*{max}({BaseToken})";
        }

        protected override bool TryMatchInternal(ReadOnlySpan<byte> bytes, out int tokenEnd, ICollection<TokenMatch>? values = null)
        {
            tokenEnd = 0;

            var i = 0;
            for (; i < Max; i++)
            {
                if (!BaseToken.TryMatch(bytes[tokenEnd..], out var token, out var match, values == null) || token == 0)
                    goto end;

                if (match != null)
                    values?.Add(match);

                tokenEnd += token;
            }

        end:
            return i >= Min;
        }
    }
}
