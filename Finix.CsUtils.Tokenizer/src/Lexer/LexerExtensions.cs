using System.Linq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    /// <summary>
    /// Defines extension methods for the lexer.
    /// </summary>
    public static class LexerExtensions
    {
        /// <summary>
        /// Requires the next token returned by the lexer
        /// </summary>
        /// <param name="lexer"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Token Expect(this ILexer lexer, params Type[] types)
        {
            var token = lexer.Consume();

            if (!types.Contains(token.GetType()))
                throw new LexerException(token.Position, $"Expected {String.Join(" or ", types.Select(t => t.Name))}, got {token.GetType().Name}.");

            return token;
        }

        /// <summary>
        /// Requires the next token returned by the lexer
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public static T Expect<T>(this ILexer lexer) where T : Token
        {
            var token = lexer.Consume();

            if (token is not T t)
                throw new LexerException(token.Position, $"Expected {typeof(T).Name}, got {token.GetType().Name}.");

            return t;
        }

        public static bool TryConsume(this ILexer lexer, [MaybeNullWhen(false)] out Token token, params Type[] types)
        {
            token = lexer.Peek();

            if (!types.Contains(token.GetType()))
                return false;

            token = lexer.Consume();
            return true;
        }

        public static bool TryConsume<T>(this ILexer lexer, [MaybeNullWhen(false)] out T token) where T : Token
        {
            var t = lexer.Peek();
            token = null;

            if (t is not T ttoken)
                return false;
            else
                token = ttoken;

            lexer.Consume();
            return true;
        }

        /// <summary>
        /// Requires the next token returned by the lexer
        /// </summary>
        /// <param name="lexer"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static ILexer Skip(this ILexer lexer, params Type[] types)
        {
            foreach (var type in types)
                lexer.Expect(type);

            return lexer;
        }

        /// <summary>
        /// Requires the next token returned by the lexer
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public static ILexer Skip<T>(this ILexer lexer) where T : Token
        {
            lexer.Expect<T>();
            return lexer;
        }
    }
}
