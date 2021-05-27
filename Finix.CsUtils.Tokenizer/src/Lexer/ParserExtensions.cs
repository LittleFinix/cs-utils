using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    /// <summary>
    /// Defines extension methods for any <see cref="ITextParser"/>.
    /// </summary>
    public static class ParserExtensions
    {
        /// <summary>
        /// Read the string up to the <see cref="Rune"/> whose value is equal to <paramref name="c"/> or until EOF is reached.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="c">A character to find in the stream</param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadUntil(this ITextParser parser, char c)
        {
            return parser.ReadUntil(new Rune(c));
        }

        /// <summary>
        /// Read the string up to the <see cref="Rune"/> which is equal to <paramref name="r"/> or until EOF is reached.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="r"></param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadUntil(this ITextParser parser, Rune r)
        {
            return parser.ReadUntil(rune => rune == r);
        }

        /// <summary>
        /// Read the string up to the <see cref="Rune"/> where <paramref name="matcher"/> returns true or until EOF is reached.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="matcher">A predicate to test for when to terminate the read.</param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadUntil(this ITextParser parser, Predicate<Rune> matcher)
        {
            return parser.ReadUntil(matcher, out _);
        }

        /// <summary>
        /// Read the string up to the <see cref="Rune"/> where <paramref name="matcher"/> returns true or until EOF is reached.
        /// The matched character is put in <paramref name="matched"/>.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="matcher">A predicate to test for when to terminate the read.</param>
        /// <param name="matched">The character that was finally matched, or 0 if the end of the stream was reached.</param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadUntil(this ITextParser parser, Predicate<Rune> matcher, out ParserValue matched)
        {
            return parser.ReadUntil(matcher, null, out matched);
        }

        /// <summary>
        /// Read the string up to any <see cref="Rune"/> that is included in <paramref name="chars"/> or until EOF is reached.
        /// The matched character is put in <paramref name="matched"/>.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="chars">The characters to find in the stream</param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadUntilAny(this ITextParser parser, params char[] chars)
        {
            return parser.ReadUntilAny(chars, out _);
        }

        /// <summary>
        /// Read the string up to any <see cref="Rune"/> that is included in <paramref name="runes"/> or until EOF is reached.
        /// The matched character is put in <paramref name="matched"/>.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="runes">The runes to find in the stream</param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadUntilAny(this ITextParser parser, params Rune[] runes)
        {
            return parser.ReadUntilAny(runes, out _);
        }

        /// <summary>
        /// Read the string up to any <see cref="Rune"/> that is included in <paramref name="chars"/> or until EOF is reached.
        /// The matched character is put in <paramref name="matched"/>.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="chars">The characters to find in the stream</param>
        /// <param name="matched">The character that was finally matched, or 0 if the end of the stream was reached.</param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadUntilAny(this ITextParser parser, char[] chars, out ParserValue match)
        {
            return parser.ReadUntilAny(chars.Select(c => new Rune(c)).ToArray(), out match);
        }

        /// <summary>
        /// Read the string up to any <see cref="Rune"/> that is included in <paramref name="runes"/> or until EOF is reached.
        /// The matched character is put in <paramref name="matched"/>.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="runes">The runes to find in the stream</param>
        /// <param name="matched">The character that was finally matched, or 0 if the end of the stream was reached.</param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadUntilAny(this ITextParser parser, Rune[] runes, out ParserValue match)
        {
            return parser.ReadUntil(runes.Contains, out match);
        }

        /// <summary>
        /// Read the string up to the <see cref="Rune"/> where <paramref name="matcher"/> returns false or until EOF is reached.
        /// The matched character is put in <paramref name="matched"/>.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="matcher">A predicate that receives a <see cref="Rune"/> and returns whether to continue reading.</param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadWhile(this ITextParser parser, Predicate<Rune> matcher)
        {
            return parser.ReadUntil(r => !matcher(r), out _);
        }

        /// <summary>
        /// Read the string up to the <see cref="Rune"/> where <paramref name="matcher"/> returns false or until EOF is reached.
        /// The matched character is put in <paramref name="matched"/>.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="matcher">A predicate that receives a <see cref="Rune"/> and returns whether to continue reading.</param>
        /// <param name="matched">The character that was finally matched, or 0 if the end of the stream was reached.</param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadWhile(this ITextParser parser, Predicate<Rune> matcher, out ParserValue matched)
        {
            return parser.ReadUntil(r => !matcher(r), null, out matched);
        }

        /// <summary>
        /// Read the string up to the <see cref="Rune"/> where <paramref name="matcher"/> returns false or until EOF is reached.
        /// The matched character is put in <paramref name="matched"/>.
        /// If <paramref name="escape"/> is set to a non-null value, the character immediately following the escape-character
        /// is always matched, even if <paramref name="matcher"/> returns false.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="matcher">A predicate that receives a <see cref="Rune"/> and returns whether to continue reading.</param>
        /// <param name="escape">A character to use to escape the matched character, or null to not allow escaping of characters.</param>
        /// <param name="matched">The character that was finally matched, or 0 if the end of the stream was reached.</param>
        /// <returns>A parser string containing the read characters.</returns>
        public static ParserString ReadWhile(this ITextParser parser, Predicate<Rune> matcher, Rune? escape, out ParserValue matched)
        {
            return parser.ReadUntil(r => !matcher(r), escape, out matched);
        }

        /// <summary>
        /// Attempts to read one of many strings from the parser and if it succeeds removes the string from the parsers buffer.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="matched">The actual string that was matched.</param>
        /// <param name="comparer">The comparer to work with to match the strings, defaults to <see cref="StringComparer.Ordinal" />.</param>
        /// <param name="expect">A list of strings to check again, in order.</param>
        /// <returns>True, if the buffer would return <paramref name="expect"/> next, false otherwise.</returns>
        public static bool TryRead(this ITextParser parser, [MaybeNullWhen(false)] out ParserString matched, IEqualityComparer<string?>? comparer, params object[] expected)
        {
            matched = null;

            foreach (var o in expected)
            {
                var str = o?.ToString();

                if (str != null && parser.StartsWith(str, out str, comparer))
                {
                    matched = parser.Read(str.Length);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to read one of many strings from the parser and if it succeeds removes the string from the parsers buffer.
        /// </summary>
        /// <param name="parser">A parser on which to operate.</param>
        /// <param name="matched">The actual string that was matched.</param>
        /// <param name="expect">A list of strings to check again, in order.</param>
        /// <returns>True, if the buffer would return <paramref name="expect"/> next, false otherwise.</returns>
        public static bool TryRead(this ITextParser parser, [MaybeNullWhen(false)] out ParserString matched, params object[] expected)
        {
            return parser.TryRead(out matched, null, expected);
        }
    }
}
