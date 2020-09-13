using System.IO;
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
            .Named("Sentence Terminator");

        private static readonly Token Word = (1 * (VCHAR - SentenceTerminator))
            .Combined()
            .Named("Word");

        private static readonly Token SentenceParser = (1 * ((Word / !AWSP) - SentenceTerminator))
            .Combined()
            .Named("Sentence");

        public static readonly Token SentencesParser = (0 * (SentenceParser / (0 * SentenceTerminator)))
            .Combined()
            .Named("Sentences");

        [Fact]
        public void MatchSentence()
        {
            var sentence = "Hello World! How are you doing?\nI'm doing fine.";
            var sentenceBytes = Encoding.UTF8.GetBytes(sentence);

            VCard.Execute(sentenceBytes, out var match);

            PrintMatch(match);
        }

        public static readonly Token Name = (
            1 * (ALPHA / DIGIT / '-')
        )
            .Combined()
            .Named("Name");

        public static readonly Token Group = (
           (0, 1) * (Name + !T('.'))
        )
            .Combined()
            .Named("Group");

        public static readonly Token NonAscii = (
           UTF8_2 / UTF8_3 / UTF8_4
        )
            .Named("NonAscii");

        public static readonly Token QSafeChar = (
            WSP / T(0x23, 0x7E) / '!' / NonAscii
        )
            .Named("QSafeChar");

        public static readonly Token SafeChar = (
            WSP / (T(0x23, 0x39) - ',') / T(0x3C, 0x7E) / '!' / NonAscii
        )
            .Named("SafeChar");

        public static readonly Token ValueChar = (
            ((WSP / VCHAR / NonAscii) - ';') / ('\\' + ';')
        )
            .Named("ValueChar");

        public static readonly Token ParamValue = (
            (0 * SafeChar) / (0 * (!DQUOTE + 0 * QSafeChar + !DQUOTE))
        )
            .Combined()
            .Named("ParamValue");

        public static readonly Token Value = (
            0 * (ValueChar / !LWSP)
        )
            .Combined()
            .Named("Value");

        public static readonly Token Param = (
            Name + !T('=') + ParamValue + 0 * (!T(',') + ParamValue)
        )
            .Combined()
            .Named("Param");

        public static readonly Token ContentLine = (
            Group + (Name - I("END") / I("VERSION")) + 0 * (!T(';') + Param) + !T(':') + Value + 0 * (!T(';') + Value) + !CRLF
        )
            .Combined()
            .Named("ContentLine");

        public static readonly Token Version = (
            DIGIT + '.' + DIGIT
        )
            .Combined()
            .Named("Version");

        public static readonly Token VCard = (
            !(I("BEGIN:VCARD") + CRLF) +
            !I("VERSION:") + Version + !CRLF +
            1 * ContentLine +
            !(I("END:VCARD") + CRLF)
        )
            .Combined()
            .Named("VCard");

        [Fact]
        public void MatchVCard()
        {
            var vcard = File.ReadAllBytes("example.vcf");
            var test = @"LABEL=""42 Plantation St.\nBaytown\, LA 30314\nUnited States of America""";

            Assert.False(SafeChar.TryMatch(new[] { (byte) '"' }, out _, out _, true));

            Assert.True(Param.TryMatch(Encoding.UTF8.GetBytes(test), out _, out _, true));

            VCard.Execute(vcard, out var match);

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
