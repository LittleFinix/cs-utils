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
        public static Func<string, string, string> Joiner(char with)
        {
            return Joiner(with.ToString());
        }

        /// <summary>
        /// Return a higher order function that takes two input strings and joins them using the <code>with</code> string.
        /// </summary>
        /// <param name="with">A string to join the strings with.</param>
        /// <returns>A higher order function that joins strings.</returns>
        public static Func<string, string, string> Joiner(string with)
        {
            return (a, b) => {
                return String.IsNullOrEmpty(a)
                    ? b
                    : String.IsNullOrEmpty(b)
                        ? a
                        : $"{a}{with}{b}";
            };
        }

        /// <summary>
        /// Enumerate a string's words.
        ///
        /// Words a separated by punctuation and whitespace, but not by digits, or separators (eg. dashes).
        /// Additionally, words are broken when an upper-case letter follows an upper case one.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="includePunctuation">Include punctuation in the output.</param>
        /// <param name="includeSymbols">Include symbols in the output.</param>
        /// <param name="indicateWhitespace">Indicate whitespace by including a single standard space (' ', U+0x000A) in the output.</param>
        /// <param name="max">Maximum number of words to return in the output (including symbols and whitespace, if applicable), the last element of the output will be the remaining text.</param>
        /// <returns></returns>
        public static IEnumerable<string> Words(string text, bool includePunctuation = false, bool includeSymbols = false, bool indicateWhitespace = false, int? max = null)
        {
            max ??= Int32.MaxValue;

            var word = new StringBuilder(12);
            var previous = new Rune(0);

            foreach (var ch in text.EnumerateRunes())
            {
                if (!Rune.IsValid(ch.Value) || Rune.IsControl(ch))
                    continue;

                if (max <= 0)
                {
                    word.Append(ch);
                    continue;
                }

                if (Rune.IsWhiteSpace(ch)
                    || (Rune.IsLetter(previous) && Rune.IsUpper(ch) && Rune.IsLower(previous))
                    || (Rune.IsPunctuation(ch) && !includePunctuation && !Rune.IsSeparator(ch))
                    || (Rune.IsSymbol(ch) && !includeSymbols && !Rune.IsSeparator(ch)))
                {
                    if (!String.IsNullOrWhiteSpace(word.ToString()))
                    {
                        max--;
                        yield return word.ToString().Trim();
                    }

                    word.Clear();

                    if (Rune.IsWhiteSpace(ch) && indicateWhitespace)
                    {
                        if (Rune.IsWhiteSpace(previous))
                            continue;

                        max--;
                        previous = ch;
                        yield return " ";
                    }
                    else if (!Rune.IsSeparator(ch)
                        && ((Rune.IsPunctuation(ch) && includePunctuation) || (Rune.IsSymbol(ch) && includeSymbols)))
                    {
                        max--;
                        yield return ch.ToString();
                    }
                }

                if (Rune.IsLetterOrDigit(ch) || Rune.IsSeparator(ch))
                {
                    previous = ch;
                    word.Append(ch);
                }
                else
                {
                    previous = new Rune(0);
                }
            }

            if (!String.IsNullOrWhiteSpace(word.ToString()))
            {
                yield return word.ToString().Trim();
            }
            else if (indicateWhitespace)
            {
                yield return " ";
            }
        }

        private static readonly Regex fileNameCleanup = new Regex("([^a-zA-Z0-9 _-])");

        public static string FileName(string text)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                text = text.Replace(c, '_');

            while (text.Contains("__"))
                text = text.Replace("__", "_");

            return SnakeCase(text); // Words(text).Select(word => TextInfo.ToLower(word)).Aggregate(Joiner('_'));
        }

        public static string CamelCase(string text)
        {
            return String.IsNullOrWhiteSpace(text)
                ? String.Empty
                : Words(text)
                    .Select(
                        (w, n) =>
                            n > 0 ? TextInfo.ToTitleCase(w) : TextInfo.ToLower(w)).Aggregate(Joiner(String.Empty));
        }

        public static string CapitalCase(string text)
        {
            return String.IsNullOrWhiteSpace(text)
                ? String.Empty
                : Words(text).Select(w => TextInfo.ToTitleCase(w)).Aggregate(Joiner(String.Empty));
        }

        public static string SnakeCase(string text)
        {
            return String.IsNullOrWhiteSpace(text)
                ? String.Empty
                : Words(text).Select(w => TextInfo.ToLower(w)).Aggregate(Joiner('_'));
        }

        public static string KebabCase(string text)
        {
            return String.IsNullOrWhiteSpace(text)
                ? String.Empty
                : Words(text).Select(w => TextInfo.ToLower(w)).Aggregate(Joiner('-'));
        }

        public static IEnumerable<string> Split(StringRuneEnumerator text, Rune[] at, Rune[] escapeSingle, (Rune, Rune)[] escapeBetween)
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
                    if (escapeBetween[escapeIdx].Item2 == c)
                    {
                        escapeIdx = -1;
                        continue;
                    }
                }
                else
                {
                    for (var i = 0; i < escapeBetween.Length; i++)
                    {
                        if (escapeBetween[i].Item1 == c)
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

                // switch (c)
                // {
                //     case '\\':
                //         escapeNext = true;
                //         break;

                //     case '\'' when escapeChar == '\0':
                //         escapeChar = '\'';
                //         break;

                //     case '\'' when escapeChar == '\'':
                //         escapeChar = '\0';
                //         break;

                //     case '"' when escapeChar == '\0':
                //         escapeChar = '"';
                //         break;

                //     case '"' when escapeChar == '"':
                //         escapeChar = '\0';
                //         break;

                //     case ' ' when escapeChar == '\0':
                //         yield return part;
                //         part = String.Empty;
                //         break;

                //     default:
                //         part += c;
                //         break;
                // }
            }

            yield return part;
        }

        public static IEnumerable<string> SplitArguments(string text)
        {
            return Split(
                text.EnumerateRunes(),
                new[] { (Rune) ' ', (Rune) '\r', (Rune) '\n' },
                new[] { (Rune) '\\' },
                new[] { ((Rune) '"', (Rune) '"'), ((Rune) '\'', (Rune) '\'') }
            );

            // char escapeChar = '\0';
            // bool escapeNext = false;

            // string part = String.Empty;

            // foreach (var c in text)
            // {
            //     if (escapeNext)
            //     {
            //         escapeNext = false;
            //         part += c;
            //         continue;
            //     }

            //     switch (c)
            //     {
            //         case '\\':
            //             escapeNext = true;
            //             break;

            //         case '\'' when escapeChar == '\0':
            //             escapeChar = '\'';
            //             break;

            //         case '\'' when escapeChar == '\'':
            //             escapeChar = '\0';
            //             break;

            //         case '"' when escapeChar == '\0':
            //             escapeChar = '"';
            //             break;

            //         case '"' when escapeChar == '"':
            //             escapeChar = '\0';
            //             break;

            //         case ' ' when escapeChar == '\0':
            //             yield return part;
            //             part = String.Empty;
            //             break;

            //         default:
            //             part += c;
            //             break;
            //     }
            // }

            // yield return part;
        }

        public static string SignificantSpace(string? val, bool collapseNewlines = true)
        {
            if (String.IsNullOrWhiteSpace(val))
                return String.Empty;

            val.Replace("\n\r", " ");
            val.Replace("\n", " ");

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
