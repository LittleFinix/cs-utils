using System.Text;
using System;
using System.Collections.Generic;

using Xunit;

using static Finix.CsUtils.Token;

namespace Finix.CsUtils.Tokenizer.Tests
{
    public class TestTokens
    {
        private static readonly Token SentenceTerminator = (T('!') / '?' / '.')
            .Combined()
            .Named("Sentence Ender");

        private static readonly Token Word = (1 * (VCHAR - SentenceTerminator))
            .Combined()
            .Named("Word");

        private static readonly Token SentenceParser = (1 * ((Word / !LWSP) - SentenceTerminator))
            .Combined()
            .Named("Sentence");

        public static Token SentencesParser = (0 * (SentenceParser / (0 * SentenceTerminator)))
            .Combined()
            .Named("Sentences");

        [Fact]
        public void MatchSentence()
        {
            var sentence = "Hello World! How are you doing?\r\n I'm doing fine.";
            var sentenceBytes = Encoding.UTF8.GetBytes(sentence);

            Assert.True(SentencesParser.TryMatch(sentenceBytes, out _, out var match));

            PrintMatch(match);
        }

        private void PrintMatch(TokenMatch match, string prefix = "")
        {
            if (match.Bytes != null)
            {
                var str = Encoding.UTF8.GetString(match.Bytes);

                Console.WriteLine($"{prefix}{match.Token}: {str}");
            }
            else
            {
                Console.WriteLine($"{prefix}{match.Token}:");
                foreach (var sm in match.SubMatches)
                {
                    PrintMatch(sm, $"{prefix} - ");
                }
            }
        }
    }
}
