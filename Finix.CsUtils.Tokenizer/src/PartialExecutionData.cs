using System.Text.RegularExpressions;
using System.Text;
using System.Buffers;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    internal sealed class PartialExecutionData
    {
        private readonly Dictionary<int, PartialExecutionData> subData = new Dictionary<int, PartialExecutionData>();

        internal int Index { get; set; }

        internal bool Authoritative { get; set; }

        internal Token? AuthoritativeSource { get; set; }

        internal Dictionary<int, List<TokenMatch>>? MatchMap { get; set; } = new Dictionary<int, List<TokenMatch>>();

        internal IEnumerable<TokenMatch>? Matches => MatchMap?.Values?.SelectMany(m => m);

        internal PartialExecutionData GetIndexed(int i, bool silent = false, bool revokeAuthority = false)
        {
            if (subData.TryGetValue(i, out var val))
                return val;

            var data = subData[i] = new PartialExecutionData();

            if (silent || MatchMap == null)
                data.MatchMap = null;

            if (revokeAuthority)
            {
                data.Authoritative = false;
            }
            else
            {
                data.Authoritative = Authoritative;
                data.AuthoritativeSource = AuthoritativeSource;
            }

            return data;
        }

        internal void ClearData(int? index = null)
        {
            if (MatchMap == null)
                return;

            if (index is int i)
            {
                if (MatchMap.TryGetValue(i, out var matches))
                    matches.Clear();
            }
            else
            {
                MatchMap.Clear();
            }
        }

        internal TokenMatch AddData(Token token, byte[] data)
        {
            return AddMatch(-1, new TokenMatch(token, data));
        }

        internal byte[] ReadData(ref SequenceReader<byte> reader, long amount, bool advance = true)
        {
            var data = new byte[amount];

            if (!reader.TryCopyTo(data))
                throw new InvalidOperationException("Failed to copy specified amount of data");

            if (advance)
                reader.Advance(amount);

            return data;
        }

        internal void AddData(Token token, ref SequenceReader<byte> reader, long amount, bool advance = true)
        {
            if (Matches != null)
            {
                var data = ReadData(ref reader, amount, false);
                AddData(token, data);
            }

            reader.Advance(amount);
        }

        [return: NotNullIfNotNull("match")]
        internal TokenMatch AddMatch(int index, TokenMatch? match)
        {
            if (MatchMap == null || match == null)
                return match!;

            if (!MatchMap.TryGetValue(index, out var matches))
                MatchMap[index] = matches = new List<TokenMatch>();

            matches.Add(match);
            return match;
        }
    }
}
