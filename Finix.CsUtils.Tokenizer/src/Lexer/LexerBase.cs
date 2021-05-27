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
    public abstract class LexerBase : ILexer
    {
        /// <summary>
        /// Creates a lexer from a parser.
        /// </summary>
        /// <param name="parser">A text parser</param>
        protected LexerBase(ITextParser parser)
        {
            Parser = parser;
        }

        /// <summary>
        /// Gets the underlying parser.
        /// </summary>
        public ITextParser Parser { get; }

        #region Helper Methods (Throw)

        protected Exception UnexpectedEOF(Mark? position = null)
        {
            return new LexerException(position ?? Parser.Position, $"Unexpected end of file.");
        }

        protected Exception UnexpectedEOF(Rune expected, Mark? position = null)
        {
            return new LexerException(position ?? Parser.Position, $"Expected character '{expected.ToString().Replace("'", "\\'")}', got end of file.");
        }

        protected Exception UnexpectedEOF(string expectedLiteral, Mark? position = null)
        {
            return new LexerException(position ?? Parser.Position, $"Expected {expectedLiteral}, got end of file.");
        }

        protected Exception UnexpectedString(string expected, ParserOutput actual)
        {
            if (actual.EOF)
                return UnexpectedEOF($"'{expected}'", actual.Start);

            return new LexerException(actual.Start, $"Expected '{expected}', got '{actual.StringValue.Replace("'", "\\'")}'.");
        }

        protected Exception UnexpectedValue(ParserOutput actual)
        {
            if (actual.EOF)
                return UnexpectedEOF(actual.Start);

            return new LexerException(actual.Start, $"Unexpected '{actual.StringValue.Replace("'", "\\'")}'.");
        }

        protected Exception UnexpectedLiteral(string expected, ParserOutput actual)
        {
            if (actual.EOF)
                return UnexpectedEOF(expected, actual.Start);

            return new LexerException(actual.Start, $"Expected {expected}, got '{actual.StringValue.Replace("'", "\\'")}'.");
        }

        protected Exception UnexpectedCharacter(ParserOutput actual)
        {
            if (actual.EOF)
                return UnexpectedEOF(actual.Start);

            return new LexerException(actual.Start, $"Unexpected character '{actual.StringValue.Replace("'", "\\'")}'.");
        }

        protected Exception UnexpectedCharacter(Rune expected, ParserOutput actual)
        {
            if (actual.EOF)
                return UnexpectedEOF(expected, actual.Start);

            return new LexerException(actual.Start, $"Expected character '{expected}', got '{actual.StringValue.Replace("'", "\\'")}'.");
        }

        #endregion

        #region Helper Methods (Reading)

        /// Attempt to read an entire line and optionally throw
        protected ParserString ReadLine(bool throwOnEOF = true)
        {
            var line = Parser.ReadLine();

            if (line.EOF && throwOnEOF)
                throw UnexpectedEOF();

            return line;
        }

        protected void ConsumeWhitespace()
        {
            Parser.ReadWhile(Rune.IsWhiteSpace);
        }

        #endregion

        #region Helper Methods (Parsing)

        protected static ParserValue NextRune(Mark end, Rune rune)
        {
            return new ParserValue(end, end.MoveMarkUp(rune), rune);
        }

        #endregion

        protected abstract IEnumerator<Token> Start();

        private Token? peeked;

        private IEnumerator<Token>? enumerator;

        /// <summary>
        /// Consume the next token found in the stream.
        /// </summary>
        /// <returns>The next token in the stream.</returns>
        public Token Consume()
        {
            // If we've previously peeked a token, return that one first.
            if (peeked != null)
            {
                var p = peeked;
                peeked = null;

                return p;
            }

            enumerator ??= Start();

            Token token;
            var pos = Parser.Position;
            var i = 0;

            try
            {
                // Call the current state
                if (enumerator.MoveNext())
                    token = enumerator.Current;
                else
                    token = new EOF(Parser.Position);

                if (Parser.Position == pos && i++ >= 10)
                    throw new LexerException(pos, "Infinite loop detected");
                else
                    i = 0;
            }
            catch (Exception ex) when (ex is not LexerException)
            {
                throw new LexerException(pos, ex);
            }

            return token;
        }


        /// <summary>
        /// Read but don't consume the next token found in the stream.
        /// </summary>
        /// <returns>The next token in the stream.</returns>
        public Token Peek()
        {
            return peeked = Consume();
        }
    }
}
