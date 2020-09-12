using System.Buffers;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Finix.CsUtils
{
    public sealed class TokenMatch
    {
        public Token Token { get; }

        public byte[]? Bytes { get; }

        public TokenMatch[]? SubMatches { get; }

        public bool IsCollapsed { get; private set; } = false;

        public TokenMatch(Token token, byte[] bytes)
        {
            Token = token;
            Bytes = bytes.ToArray();
        }

        public TokenMatch(Token token, IEnumerable<byte> bytes)
        {
            Token = token;
            Bytes = bytes.ToArray();
        }

        public TokenMatch(Token token, ReadOnlySpan<byte> bytes)
        {
            Token = token;
            Bytes = bytes.ToArray();
        }

        public TokenMatch(Token token, IEnumerable<TokenMatch> matches)
        {
            Token = token;
            SubMatches = matches.ToArray();
        }

        private void CollapseInternal(List<TokenMatch> matches)
        {
            if (IsCollapsed)
            {
                matches.Add(this);
            }
            else if (Bytes != null)
            {
                matches.Add(new TokenMatch(Token, Bytes));
            }
            else if (SubMatches != null)
            {
                foreach (var sub in SubMatches)
                {
                    if (sub.IsCollapsed)
                        matches.Add(sub);
                    else
                        sub.CollapseInternal(matches);
                }
            }
        }

        public TokenMatch Collapse()
        {
            var matches = new List<TokenMatch>();
            CollapseInternal(matches);

            TokenMatch match;

            if (matches.All(m => !m.IsCollapsed))
                match = new TokenMatch(Token, Combine());
            else
                match = new TokenMatch(Token, matches);

            match.IsCollapsed = true;
            return match;
        }

        public byte[] Combine()
        {
            return Bytes ?? SubMatches.SelectMany(m => m.Combine()).ToArray();
        }

        public override string ToString()
        {
            return $"[{Token}: " + String.Join(' ', Combine().Select(b => $"{b,2:X}")) + "]";
        }
    }
}
