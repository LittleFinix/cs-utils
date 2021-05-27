using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.Threading;
using System.IO;

namespace Finix.CsUtils
{
    [Flags]
    public enum WordRuneType
    {
        None = 0,
        AlphaNumeric = 1 << 1,
        Punctuation = 1 << 2,
        Symbols = 1 << 3,
        Whitespace = 1 << 4,
        Underscore = 1 << 5,
        Dash = 1 << 6,
    }

    public enum StrEnumeratorResult
    {
        Continue = 0,
        Split = 1,
        Skip = 2,
        Discard = 3,
        Stop = 4
    }

    /// <summary>
    /// A class containing helper functions for string operations.
    /// </summary>
    public static class Str
    {
        private static CultureInfo? culture = null;

        /// <summary>
        /// Get or set the selected <see cref=CultureInfo /> used in the helper functions.
        /// If set to <code>null</code>, returns <see cref=Thread.CurrentCulture /> from the calling thread instead.
        /// </summary>
        /// <value>A <see cref=CultureInfo /> object, or null to use the thread's culture info.</value>
        public static CultureInfo Culture
        {
            get => culture ?? Thread.CurrentThread.CurrentCulture;
            set => culture = value;
        }

        /// <summary>
        /// Get the <see cref=System.Globalization.TextInfo /> from <see cref=Culture />.
        /// </summary>
        public static TextInfo TextInfo => Culture.TextInfo;

        /// <summary>
        /// Return a higher order function that takes two input strings and joins them using the <code>with</code> character.
        /// </summary>
        /// <param name="with">A character to join the strings with.</param>
        /// <returns>A higher order function that joins strings.</returns>
        public static Func<string?, string?, string> Joiner(char with)
        {
            return Joiner(with.ToString());
        }

        /// <summary>
        /// Return a higher order function that takes two input strings and joins them using the <code>with</code> string.
        /// </summary>
        /// <param name="with">A string to join the strings with.</param>
        /// <returns>A higher order function that joins strings.</returns>
        public static Func<string?, string?, string> Joiner(string with)
        {
            return (a, b) => {
                var result = String.IsNullOrEmpty(a)
                    ? b
                    : String.IsNullOrEmpty(b)
                        ? a
                        : $"{a}{with}{b}";

                return result ?? String.Empty;
            };
        }

        public static WordRuneType GetWordRuneType(int r)
        {
            return GetWordRuneType(new Rune(r));
        }

        public static WordRuneType GetWordRuneType(this Rune r)
        {
            if (Rune.IsLetterOrDigit(r))
                return WordRuneType.AlphaNumeric;
            else if (Rune.IsWhiteSpace(r))
                return WordRuneType.Whitespace;
            else if (r.Value is '-' or (>= 0x2012 and <= 0x2014))
                return WordRuneType.Dash;
            else if (r.Value == '_')
                return WordRuneType.Underscore;
            else if (Rune.IsPunctuation(r))
                return WordRuneType.Punctuation;
            else if (Rune.IsSymbol(r))
                return WordRuneType.Symbols;
            else
                return WordRuneType.None;
        }

        public static IEnumerable<Rune> EnumerateRunes(this StringBuilder builder)
        {
#if NETCOREAPP3_0_OR_GREATER
            foreach (var chunk in builder.GetChunks())
            {
                foreach (var rune in new string(chunk.Span).EnumerateRunes())
                    yield return rune;
            }
#else
            foreach (var rune in builder.ToString().EnumerateRunes())
                yield return rune;
#endif
        }

        public struct RuneGroup
        {
            public Rune Previous, Rune;
            public int Index;
        }

        public static IEnumerable<RuneGroup> Grouped(this IEnumerable<Rune> runes)
        {
            var i = 0;
            var prev = new Rune(0);

            foreach (var rune in runes)
            {
                yield return new RuneGroup { Previous = rune, Rune = prev, Index = i++ };
                prev = rune;
            }
        }

#if NETCOREAPP3_0_OR_GREATER

        public static IEnumerable<T> EnumerateSubstrings<T>(this string text, Func<string, int, Rune, Rune, StrEnumeratorResult> split, Func<Range, string, T> output, bool reverse = false)
        {
            IEnumerable<Rune> it = text.EnumerateRunes();

            if (reverse)
                it = it.Reverse();

            var i = reverse ? text.Length : 0;

            var prev = i;
            var previous = new Rune(0);

            foreach (var rune in it)
            {
                var result = split(text, i, rune, previous);
                switch (result)
                {
                    case StrEnumeratorResult.Continue:
                        break;

                    case StrEnumeratorResult.Split:
                        yield return output(Math.Min(prev, i)..Math.Max(prev, i), text);
                        prev = i;
                        break;

                    case StrEnumeratorResult.Skip:
                        if (i != prev)
                            yield return output(Math.Min(prev, i - 1)..Math.Max(prev, i - 1), text);

                        prev = i + 1;
                        break;


                    case StrEnumeratorResult.Discard:
                        prev = i;
                        break;

                    case StrEnumeratorResult.Stop:
                        yield break;

                    default:
                        throw new NotSupportedException($"Unsupported StrEnumeratorResult: {result}");
                }

                i = reverse ? i-- : i++;
                previous = rune;
            }

            yield return output(Math.Min(prev, i)..Math.Max(prev, i), text);
        }
#endif

        // public static IEnumerable<string> Words(this string text, WordRuneType includeInWord = WordRuneType.AlphaNumeric, WordRuneType includeInOutput = WordRuneType.None, int? max = null)
        // {
        //     return text.EnumerateSubstrings(
        //         (text, nr, ch, prev) => {
        //             if (!Rune.IsValid(ch.Value) || Rune.IsControl(ch))
        //                 return StrEnumeratorResult.Skip;

        //             if (max <= 0)
        //                 return StrEnumeratorResult.Continue;

        //             var category = ch.GetWordRuneType();

        //             if (category == WordRuneType.None)
        //                 return StrEnumeratorResult.Skip;

        //             if (!includeInWord.HasFlag(category))
        //                 return StrEnumeratorResult.Split;
        //         },
        //         (r, text) => text[r]
        //     );
        // }

        /// <summary>
        /// Enumerate a string's words.
        ///
        /// Words a separated by punctuation, whitespace, and symbols (eg. dashes).
        /// Additionally, words are broken when a lower-case letter follows an upper case one.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="includePunctuation">Include punctuation in the output.</param>
        /// <param name="includeSymbols">Include symbols in the output.</param>
        /// <param name="indicateWhitespace">Indicate whitespace by including a single standard space (' ', U+0x000A) in the output.</param>
        /// <param name="max">Maximum number of words to return in the output (including symbols and whitespace, if applicable), the last element of the output will be the remaining text.</param>
        /// <returns></returns>
        public static IEnumerable<string> Words(this string text, WordRuneType includeInWord = WordRuneType.AlphaNumeric, WordRuneType includeInOutput = WordRuneType.None, int? max = null)
        {
            max ??= Int32.MaxValue;

            var word = new StringBuilder(12);
            var previous = new Rune(0);

            bool TryReturn(out string result)
            {
                result = word!.ToString();

                if (!String.IsNullOrWhiteSpace(result))
                {
                    max--;
                    word.Clear();

                    return true;
                }

                return false;
            }

            foreach (var ch in text.EnumerateRunes())
            {
                if (!Rune.IsValid(ch.Value) || Rune.IsControl(ch))
                    continue;

                if (max <= 0)
                {
                    word.Append(ch);
                    continue;
                }

                var category = ch.GetWordRuneType();

                if (category == WordRuneType.None)
                    continue;

                /*
                Rune.IsWhiteSpace(ch)
                    || (Rune.IsLetter(previous) && Rune.IsUpper(ch) && Rune.IsLower(previous))
                    || (Rune.IsPunctuation(ch) && !includeInOutput.HasFlag(WordRuneType.Punctuation) && !Rune.IsSeparator(ch))
                    || (Rune.IsSymbol(ch) && !includeInOutput.HasFlag(WordRuneType.Symbols) && !Rune.IsSeparator(ch))
                    */

                if (!includeInWord.HasFlag(category))
                {
                    if (TryReturn(out var result))
                        yield return result;

                    if (category == WordRuneType.Whitespace && includeInOutput.HasFlag(WordRuneType.Whitespace))
                    {
                        if (Rune.IsWhiteSpace(previous))
                            continue;

                        max--;
                        yield return " ";
                    }
                    else if (includeInOutput.HasFlag(category))
                    {
                        max--;
                        yield return ch.ToString();
                    }
                }
                else if (Rune.IsLetter(previous) && Rune.IsUpper(ch) && Rune.IsLower(previous))
                {
                    if (TryReturn(out var result))
                        yield return result;
                }

                if (includeInWord.HasFlag(category))
                {
                    word.Append(ch);
                }

                previous = ch;
            }

            if (TryReturn(out var finalResult))
                yield return finalResult;
            else if (includeInOutput.HasFlag(WordRuneType.Whitespace))
                yield return " ";
        }

        public static bool IsWordBoundary(Rune a, Rune b, WordRuneType wordTypes)
        {
            return IsWordBoundary(a, b, r => wordTypes.HasFlag(r.GetWordRuneType()));
        }

        public static bool IsWordBoundary(Rune a, Rune b, ICollection<Rune> wordCharacters)
        {
            return IsWordBoundary(a, b, r => wordCharacters.Contains(r));
        }

        public static bool IsWordBoundary(Rune a, Rune b, params Predicate<Rune>[] isWordCharacterMatchers)
        {
            if (isWordCharacterMatchers.Length == 0)
                isWordCharacterMatchers = new Predicate<Rune>[] { Rune.IsLetterOrDigit };

            var match_a = isWordCharacterMatchers.Any(m => m(a));
            var match_b = isWordCharacterMatchers.Any(m => m(b));

            return match_a != match_b;
        }

#if NETCOREAPP3_0_OR_GREATER
        public static Index WordBoundary(this IEnumerable<Rune> text, Index from, Func<Rune, Rune, bool>? wordBoundaryFn = null)
        {
            wordBoundaryFn ??= (a, b) => IsWordBoundary(a, b);

            if (from.IsFromEnd)
                text = text.Reverse().Skip(from.Value);
            else
                text = text.Skip(from.Value);

            var start = true;
            var last = 0;
            foreach (var g in text.Grouped())
            {
                var rune = g.Rune;
                var prev = g.Previous;
                var i = g.Index;

                if (start && (Rune.IsWhiteSpace(rune) || Rune.IsWhiteSpace(prev)))
                    continue;
                else
                    start = false;

                if (prev.Value > 0 && wordBoundaryFn(rune, prev))
                    return new Index(from.Value + i, from.IsFromEnd);

                last = i + 1;
            }

            return new Index(from.Value + last, from.IsFromEnd);
        }

        public static Index WordBoundary(this string text, Index from, Func<Rune, Rune, bool>? wordBoundaryFn = null)
        {
            return text.EnumerateRunes().WordBoundary(from, wordBoundaryFn);
        }

#else

        public static int WordBoundary(this IEnumerable<Rune> text, int from, Func<Rune, Rune, bool>? wordBoundaryFn = null)
        {
            wordBoundaryFn ??= (a, b) => IsWordBoundary(a, b);

            bool fromEnd = from < 0;
            from = Math.Abs(from);

            if (fromEnd)
                text = text.Reverse().Skip(from);
            else
                text = text.Skip(from);

            var start = true;
            var last = 0;
            foreach (var g in text.Grouped())
            {
                var rune = g.Rune;
                var prev = g.Previous;
                var i = g.Index;

                if (start && (Rune.IsWhiteSpace(rune) || Rune.IsWhiteSpace(prev)))
                    continue;
                else
                    start = false;

                if (prev.Value > 0 && wordBoundaryFn(rune, prev))
                    return from + i * (fromEnd ? -1 : 1);

                last = i + 1;
            }

            return from + last * (fromEnd ? -1 : 1);
        }

        public static int WordBoundary(this string text, int from, Func<Rune, Rune, bool>? wordBoundaryFn = null)
        {
            return text.EnumerateRunes().WordBoundary(from, wordBoundaryFn);
        }
#endif

        public static string FileName(this string text)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                text = text.Replace(c, '_');

            while (text.Contains("__"))
                text = text.Replace("__", "_");

            return SnakeCase(text); // Words(text).Select(word => TextInfo.ToLower(word)).Aggregate(Joiner('_'));
        }

        public static string CamelCase(this string text)
        {
            return String.IsNullOrWhiteSpace(text.ToString())
                ? String.Empty
                : Words(text)
                    .Select(
                        (w, n) =>
                            n > 0 ? TextInfo.ToTitleCase(w) : TextInfo.ToLower(w)).Join(String.Empty);
        }

        public static string CapitalCase(this string text)
        {
            return String.IsNullOrWhiteSpace(text)
                ? String.Empty
                : Words(text).Select(w => TextInfo.ToTitleCase(w)).Join(String.Empty);
        }

        public static string SnakeCase(this string text)
        {
            return String.IsNullOrWhiteSpace(text)
                ? String.Empty
                : Words(text).Select(w => TextInfo.ToLower(w)).Join('_');
        }

        public static string KebabCase(this string text)
        {
            return String.IsNullOrWhiteSpace(text)
                ? String.Empty
                : Words(text).Select(w => TextInfo.ToLower(w)).Join('-');
        }

        public static string Join(this IEnumerable<string?> words, char with = ' ')
        {
            return words.Where(w => !String.IsNullOrEmpty(w)).DefaultIfEmpty().Aggregate(Joiner(with))!; // Joiner never returns null.
        }

        public static string Join(this IEnumerable<string?> words, string with)
        {
            return words.Where(w => !String.IsNullOrEmpty(w)).DefaultIfEmpty().Aggregate(Joiner(with))!; // Joiner never returns null.
        }

        public struct EscapePair
        {
            public Rune Start, End;

            public EscapePair(Rune start, Rune end)
            {
                Start = start;
                End = end;
            }
        }

#if NETCOREAPP3_0_OR_GREATER
        [Obsolete("Use Split with the EscapePair parameter instead")]
        public static IEnumerable<string> Split(this IEnumerable<Rune> text, Rune[] at, Rune[] escapeSingle, (Rune, Rune)[] escapeBetween)
        {
            return text.Split(at, escapeSingle, escapeBetween.Select(pair => new EscapePair { Start = pair.Item1, End = pair.Item2 }).ToArray());
        }
#endif

        public static IEnumerable<string> Split(this IEnumerable<Rune> text, Rune[] at, Rune[] escapeSingle, EscapePair[] escapeBetween)
        {
            var escapeIdx = -1;
            var escapeNext = false;
            var part = String.Empty;

            foreach (var c in text)
            {
                if (escapeNext)
                {
                    escapeNext = false;
                    part += c;
                    continue;
                }

                if (escapeSingle.Contains(c))
                {
                    escapeNext = true;
                    continue;
                }

                if (escapeIdx >= 0)
                {
                    if (escapeBetween[escapeIdx].End == c)
                    {
                        escapeIdx = -1;
                        continue;
                    }
                }
                else
                {
                    for (var i = 0; i < escapeBetween.Length; i++)
                    {
                        if (escapeBetween[i].Start == c)
                        {
                            escapeIdx = i;
                            break;
                        }
                    }

                    if (escapeIdx >= 0)
                    {
                        continue;
                    }
                    else if (at.Contains(c))
                    {
                        yield return part;
                        part = String.Empty;
                        continue;
                    }
                }

                part += c;
            }

            yield return part;
        }

        public static IEnumerable<string> SplitArguments(string text)
        {
            return Split(
                text.EnumerateRunes(),
                Runes.BreakableWhiteSpace.ToArray(),
                new[] { (Rune) '\\' },
                new[] { new EscapePair((Rune) '"', (Rune) '"'), new EscapePair((Rune) '\'', (Rune) '\'') }
            );
        }

        public static string SignificantSpace(string? val, bool collapseNewlines = true)
        {
            if (String.IsNullOrWhiteSpace(val))
                return String.Empty;

            if (collapseNewlines)
            {
                val = val.Replace("\n\r", " ");
                val = val.Replace("\n", " ");
            }

            var len = val.Length;

            val = val.TrimStart();

            if (val.Length != len)
                val = ' ' + val;

            len = val.Length;

            val = val.TrimEnd();

            if (val.Length != len)
                val += ' ';

            return val;
        }
    }
}
