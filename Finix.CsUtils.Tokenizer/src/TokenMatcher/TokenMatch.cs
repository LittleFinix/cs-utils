﻿using System.Text;
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

        public string AsString(Encoding? encoder = null)
        {
            encoder ??= Encoding.UTF8;

            return encoder.GetString(Combine());
        }

        public IEnumerable<TokenMatch> Find(string tokenName, bool recursive = false)
        {
            var matches = SubMatches == null
                ? Enumerable.Empty<TokenMatch>()
                : SubMatches;

            if (recursive)
                matches = matches.SelectMany(m => m.Find(tokenName, true).Append(m));

            return matches.Where(m => m.Token.Name == tokenName);
        }

        public IEnumerable<TokenMatch> Find(Token token, bool recursive = false)
        {
            if (token.Name == null)
                return Enumerable.Empty<TokenMatch>();

            return Find(token.Name, recursive);
        }

        public TokenMatch? First(Token key, bool recursive = false)
        {
            return Find(key, recursive).FirstOrDefault();
        }

        public TokenMatch[] All(Token key, bool recursive = false)
        {
            return Find(key, recursive).ToArray();
        }

        public string? GetString(Token key, bool recursive = false)
        {
            return First(key, recursive)?.AsString();
        }

        public string[] GetStrings(Token key, bool recursive = false)
        {
            return Find(key, recursive).Select(m => m.AsString()).ToArray();
        }

        public override string ToString()
        {
            var bytes = Combine();
            var text = String.Join(' ', bytes.Select(b => b.ToString("X2")));

            if (bytes.All(c => !Char.IsControl((char) c)))
                text = '"' + AsString() + '"'; // Encoding.UTF8.GetString(bytes).Replace("\n", "\\n").Replace("\r", "\\r")


            return $"[{Token}: " + text + "]";
        }
    }
}
