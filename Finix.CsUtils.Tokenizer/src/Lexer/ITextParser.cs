using System.Collections.Generic;
using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    /// <summary>
    /// An interface for a simple text parser that allows branching and returning the read characters.
    /// </summary>
    public interface ITextParser
    {
        /// <summary>
        /// Gets whether the current parser is a branch or the main parser.
        /// </summary>
        bool IsBranch { get; }

        /// <summary>
        /// Gets the current position of the parser.
        /// </summary>
        /// <remarks>
        /// The position is effectively <see cref="ParserOutput.End" /> of the last read.
        /// </remarks>
        Mark Position { get; }

        // /// <summary>
        // /// Branches of the current parser.
        // /// </summary>
        // /// <returns>A parsing branch that may be returned to the current parser.</returns>
        // IParsingBranch Branch();

        /// <summary>
        /// Commits the changes made to the current parsing branch, only valid when the <see cref="IsBranch"/> is true.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// When <see cref="IsBranch"/> is false.
        /// </exception>
        void Commit();

        /// <summary>
        /// Reads a number of <see cref="Rune" />s from the stream.
        /// </summary>
        /// <param name="count">The number of <see cref="Rune" />s to read.</param>
        /// <returns>A parser string containing the position and value of the read string.</returns>
        ParserString Read(int count = 1);

        /// <summary>
        /// Returns a previously read string to the stream.
        /// </summary>
        /// <remarks>
        /// Care should be taken when putting back values, as if they where different than those read,
        /// subsequent reading from the parser will return incorrect marks.
        /// </remarks>
        /// <param name="str">A string that was previously read.</param>
        void PutBack(ParserString str);

        /// <summary>
        /// Peek at the next value that the parser would return on a <see cref="Read"/>.
        /// </summary>
        /// <remarks>
        /// This operation is logically equivalent to
        /// <code>
        /// ParserValue val = parser.Read();
        /// parser.PutBack(val.Value);
        ///
        /// return val;
        /// </code>
        /// </remarks>
        /// <returns>A parser value containing the position and value of the next <see cref="Rune" />.</returns>
        ParserString Peek(int count = 1);

        /// <summary>
        /// Peek at the next string and check if it matches according to the equality comparer
        /// </summary>
        /// <param name="expect">The string to check against.</param>
        /// <param name="actual">The actual string that was matched.</param>
        /// <param name="comparer">The comparer to work with to match the strings, defaults to <see cref="StringComparer.Ordinal" />.</param>
        /// <returns>True, if the buffer would return <paramref name="expect"/> next, false otherwise.</returns>
        bool StartsWith(string expect, [MaybeNullWhen(false)] out string actual, IEqualityComparer<string?>? comparer = null);

        /// <summary>
        /// Read the string up to the <see cref="Rune"/> where <paramref name="matcher"/> returns true or until EOF is reached.
        /// The matched character is put in <paramref name="matched"/>.
        /// If <paramref name="escape"/> is set to a non-null value, the character immediately following the escape-character
        /// is never matched, even if <paramref name="matcher"/> returns true.
        /// </summary>
        /// <param name="matcher">A predicate that receives a <see cref="Rune"/> and returns whether to stop further reading.</param>
        /// <param name="escape">A character to use to escape the matched character, or null to not allow escaping of characters.</param>
        /// <param name="matched">The character that was finally matched, or 0 if the end of the stream was reached.</param>
        /// <returns>A parser string containing the read characters.</returns>
        ParserString ReadUntil(Predicate<Rune> matcher, Rune? escape, out ParserValue matched);

        /// <summary>
        /// Reads a single line from the stream. <c>\n</c>, <c>\r</c> and <c>\r\n</c> are all considered line-terminating characters.
        /// </summary>
        /// <returns>A parser string containing the read line.</returns>
        ParserString ReadLine();
    }
}
