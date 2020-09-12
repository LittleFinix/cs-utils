using System.Diagnostics;
using System.Collections.Generic;
using System.Buffers;
using System.Linq;
using System;
using System.Collections;

namespace Finix.CsUtils
{
    public abstract class MultiToken : Token, IEnumerable<Token>
    {
        private readonly List<Token> tokens;

        public IReadOnlyCollection<Token> Tokens => tokens.AsReadOnly();

        public MultiToken(IEnumerable<Token> tokens)
        {
            this.tokens = new List<Token>(tokens);
        }

        public void Add(Token token)
        {
            tokens.Add(token);
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
