using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    /// <summary>
    /// A lexer base class that reads tokens from a parser.
    /// </summary>
    public sealed class BufferedLexer : ILexer
    {
        private LinkedList<Token> Tokens { get; init; } = new();

        public BufferedLexer(ILexer baseLexer)
        {
            BaseLexer = baseLexer;
        }

        public ITextParser Parser => BaseLexer.Parser;

        public ILexer BaseLexer { get; }

        public Token? Last { get; set; }

        public BufferedLexer Copy()
        {
            return new BufferedLexer(BaseLexer) {
                Tokens = new(Tokens)
            };
        }

        public void Buffer(int count)
        {
            while (count-- > 0)
            {
                var next = BaseLexer.Consume();
                Tokens.AddLast(next);
            }
        }

        public void BufferUntil(Predicate<Token> matcher)
        {
            while (!matcher(BaseLexer.Peek()))
                Buffer(1);
        }

        public void BufferWhile(Predicate<Token> matcher)
        {
            BufferUntil(t => !matcher(t));
        }

        public void BufferUntil<T>()
        {
            BufferUntil(t => t is T);
        }

        public void BufferWhile<T>()
        {
            BufferWhile(t => t is T);
        }

        public Token Consume()
        {
            if (Tokens.Count == 0)
            {
                return Last = BaseLexer.Peek();
            }
            else
            {
                var t = Tokens.First!.Value;
                Last = t;
                Tokens.RemoveFirst();

                return t;
            }
        }

        public Token Peek()
        {
            if (Tokens.Count == 0)
                return BaseLexer.Peek();
            else
                return Tokens.First!.Value;
        }
    }
}
