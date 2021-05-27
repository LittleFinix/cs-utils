using System.Collections.Generic;
using System.Data;
using System;
using Xunit;
using FluentAssertions;
using System.Text;

namespace Finix.CsUtils.Strings
{
    public class TestStr
    {
        public static IEnumerable<object[]> GetWordPairs()
        {
            // Basic Tests
            yield return new object[] { "Hello, World!", new[] { "Hello", "World" }, WordRuneType.None };
            yield return new object[] { "HelloWorld!", new[] { "Hello", "World" }, WordRuneType.AlphaNumeric };
            yield return new object[] { "foo_bar-baz", new[] { "foo", "bar", "baz" }, WordRuneType.AlphaNumeric };

            yield return new object[] { "Hello, World!", new[] { "Hello", "World" }, WordRuneType.Dash };
            yield return new object[] { "HelloWorld!", new[] { "Hello", "World" }, WordRuneType.Dash };
            yield return new object[] { "foo_bar-baz", new[] { "foo", "bar", "-", "baz" }, WordRuneType.Dash };

            yield return new object[] { "Hello, World!", new[] { "Hello", "World" }, WordRuneType.Underscore };
            yield return new object[] { "HelloWorld!", new[] { "Hello", "World" }, WordRuneType.Underscore };
            yield return new object[] { "foo_bar-baz", new[] { "foo", "_", "bar", "baz" }, WordRuneType.Underscore };

            yield return new object[] { "Hello, World!", new[] { "Hello", "World" }, WordRuneType.Dash | WordRuneType.Underscore };
            yield return new object[] { "HelloWorld!", new[] { "Hello", "World" }, WordRuneType.Dash | WordRuneType.Underscore };
            yield return new object[] { "foo_bar-baz", new[] { "foo", "_", "bar", "-", "baz" }, WordRuneType.Dash | WordRuneType.Underscore };

            // Complex Tests
            var strings = new[] {
                new string[] { "Hello", ",", " ", "World", "!" },
                new string[] { "Hello", "World", "!" },
                new string[] { "foo", "_", "bar", "-", "baz" },
            };

            var flags = new[] {
                WordRuneType.AlphaNumeric,
                WordRuneType.Dash,
                WordRuneType.Underscore,
                WordRuneType.Dash | WordRuneType.Underscore
            };

            foreach (var flag in flags)
            {
                foreach (var str in strings)
                {
                    var text = String.Join(String.Empty, str);
                    var expected = new List<string>();

                    foreach (var s in str)
                    {
                        if (s == "-" && flag.HasFlag(WordRuneType.Dash))
                            expected.Add(s);
                        else if (s == "_" && flag.HasFlag(WordRuneType.Underscore))
                            expected.Add(s);
                        else if (Char.IsWhiteSpace(s[0]) && flag.HasFlag(WordRuneType.Whitespace))
                            expected.Add(s);
                        else if (Char.IsPunctuation(s[0]) && flag.HasFlag(WordRuneType.Punctuation))
                            expected.Add(s);
                        else if (Char.IsSymbol(s[0]) && flag.HasFlag(WordRuneType.Symbols))
                            expected.Add(s);
                        else if (Char.IsLetterOrDigit(s[0]))
                            expected.Add(s);
                    }

                    yield return new object[] { text, expected, flag };
                }
            }
        }

        public static IEnumerable<object[]> GetWordBoundaries()
        {
            var str = "AAA bbb ccc";

            yield return new object[] { str, 0, 3, 0, "|AAA bbb ccc -> AAA| bbb ccc", "|AAA bbb ccc -> |AAA bbb ccc" };
            yield return new object[] { str, 5, 7, 4, "AAA b|bb ccc -> AAA bbb| ccc", "AAA b|bb ccc -> AAA |bbb ccc" };
            yield return new object[] { str, 7, 11, 4, "AAA bbb| ccc -> AAA bbb ccc|", "AAA bbb| ccc -> AAA |bbb ccc" };
            yield return new object[] { str, 2, 3, 0, "AA|A bbb ccc -> AAA| bbb ccc", "AA|A bbb ccc -> |AAA bbb ccc" };
        }

        [Theory]
        [MemberData(nameof(GetWordPairs))]
        public void TestWords(string text, string[] words, WordRuneType flags)
        {
            //Given

            //When
            var result = text.Words(includeInOutput: flags);

            //Then
            result.Should().BeEquivalentTo(words);
        }

        [Theory]
        [MemberData(nameof(GetWordBoundaries))]
        public void TestWordBoundary(string text, int pos, int expectedRight, int expectedLeft, string infoR, string infoL)
        {
            //Given

            //When
            var leftBoundary = text.WordBoundary(^(text.Length - pos)).GetOffset(text.Length);
            var rightBoundary = text.WordBoundary(pos).GetOffset(text.Length);

            //Then
            $"{text.Insert(pos, "|")} -> {text.Insert(leftBoundary, "|")}".Should().Be(infoL);
            $"{text.Insert(pos, "|")} -> {text.Insert(rightBoundary, "|")}".Should().Be(infoR);

            leftBoundary.Should().Be(expectedLeft);
            rightBoundary.Should().Be(expectedRight);
        }
    }
}
