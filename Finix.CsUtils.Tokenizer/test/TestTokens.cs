using System.Linq;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

using Xunit;

using static Finix.CsUtils.Token;
using System.Buffers;

namespace Finix.CsUtils.Tokenizer.Tests
{
    public class TestTokens
    {
        private static readonly Token Alternatives = (
                1 * T('a')) + ('.' + (1 * T('b'))
            )
            .Combined()
            .Debugging(recurse: true)
            .Named("Alternatives");

        [Fact]
        public void MatchAlternatives()
        {
            var values = new[] {
                "aaa.bbb",
                "aa.bb",
                "a.b"
            };

            foreach (var val in values)
            {
                var bytes = Encoding.UTF8.GetBytes(val);

                Assert.True(Alternatives.Debugging(recurse: true).Execute(bytes, out var match, out var status));
            }
        }

        [Fact]
        public void MatchIPs()
        {
            var uris = new[] {
                "2001:db8::7",
                "192.0.2.16",
            };

            foreach (var uri in uris)
            {
                var bytes = Encoding.UTF8.GetBytes(uri);

                Assert.True(IP.Debugging(recurse: true).Execute(bytes, out var match, out var status));
            }
        }

        [Fact]
        public void MatchURIs()
        {
            var uris = new[] {
                "https://john.doe@www.example.com:123/forum/questions/?tag=networking&order=newest#top",
                "ldap://[2001:db8::7]/c=GB?objectClass?one",
                "news:comp.infosystems.www.servers.unix",
                "tel:+1-816-555-1212",
                "telnet://192.0.2.16:80/",
                "urn:oasis:names:specification:docbook:dtd:xml:4.1.2"
            };

            foreach (var uri in uris)
            {
                var realUri = new Uri(uri);
                var bytes = Encoding.UTF8.GetBytes(uri);

                Assert.True(URI.Execute(bytes, out var match, out var status));
                Assert.NotEqual(OperationStatus.InvalidData, status);
                Assert.NotEqual(OperationStatus.DestinationTooSmall, status);

                Assert.Equal(realUri.Scheme, match.GetString(URIParts.Scheme));
                Assert.Equal(realUri.UserInfo, match.GetString(URIParts.Userinfo, true));
                Assert.Equal(realUri.Host, match.GetString(URIParts.Host, true));

                // Assert.Equal(realUri.Authority, match.GetString(URIParts.Authority)); // ! CLR Authority is not as per RFC 3986 3.2., it should contain userinfo
            }
        }

        private static readonly Token SentenceTerminator = (T('!') / '?' / '.')
            .Combined()
            .Named("Sentence Terminator");

        private static readonly Token Word = (1 * (VCHAR - SentenceTerminator))
            .Combined()
            .Named("Word");

        private static readonly Token SentenceParser = (1 * (Word / !AWSP))
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

            SentencesParser.Execute(sentenceBytes, out var match, out _);

            PrintMatch(match);
        }

        public static readonly Token EscapeChar = !T('\\');

        public static readonly Token EscapedChar = (
            (EscapeChar + '\\') /
            (EscapeChar + ';') /
            (EscapeChar + ',') /
            (EscapeChar + ':') /
            R(EscapeChar + 'n', '\n')
        )
            .Named("EscapedChar");

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
            // .Debugging()
            .Named("NonAscii");

        public static readonly Token QSafeChar = (
            WSP / (T(0x23, 0x7E) - EscapedChar) / '!' / NonAscii / EscapedChar
        )
            // .Debugging()
            .Named("QSafeChar");

        public static readonly Token SafeChar = (
            WSP / (T(0x23, 0x39) - ',' / EscapedChar) / T(0x3C, 0x7E) / '!' / NonAscii / EscapedChar
        )
            // .Debugging()
            .Named("SafeChar");

        public static readonly Token ValueChar = (
            (WSP / VCHAR / NonAscii - ';') / EscapedChar
        )
            // .Debugging()
            .Named("ValueChar");

        public static readonly Token ParamValue = (
            0 * SafeChar / (0 * (!DQUOTE + 0 * QSafeChar + !DQUOTE))
        )
            .Authoritative()
            .Combined()
            .Named("ParamValue");

        public static readonly Token Value = (
            0 * (ValueChar / !LWSP)
        )
            .Authoritative()
            .Combined()
            .Named("Value");

        public static readonly Token Param = (
            Name + !T('=') + ParamValue + 0 * (!T(',') + ParamValue)
        )
            .Combined()
            .Named("Param");

        public static readonly Token ContentLine = (
            Group + (Name - I("END") / I("VERSION")) +
            0 * (!T(';') + Param.Authoritative()) + !T(':') +
            (Value + 0 * (!T(';') + Value) + !CRLF).Authoritative()
        )
            .Combined()
            .Named("ContentLine");

        public static readonly Token Version = (
            DIGIT + '.' + DIGIT
        )
            .Combined()
            .Named("Version");

        public static readonly Token VCard = (
            (
                !(I("BEGIN:VCARD") + CRLF) +
                !I("VERSION:") + Version + !CRLF
            ).Authoritative() +

            1 * ContentLine +

            !(I("END:VCARD") + CRLF).Authoritative()
        )
            .Combined()
            .Named("VCard");

        [Fact]
        public void MatchVCard()
        {
            var vcard = File.ReadAllBytes("example.vcf");
            var test = @"LABEL=""42 Plantation St.\nBaytown\, LA 30314\nUnited States of America""";

            // Assert.Throws<FormatException>(() => SafeChar.Execute(new[] { (byte) '"' }, out _, out _));

            TokenMatch match;

            Assert.True(Param.Execute(Encoding.UTF8.GetBytes(test), out match, out var status));
            Assert.Equal(OperationStatus.NeedMoreData, status);

            PrintMatch(match);

            Assert.True(VCard.Execute(vcard, out match, out _));
            PrintMatch(match);

            // var s = 0;
            // var e = 5;
            // var obj = new object();

            // while (true)
            // {
            //     var seq = new ReadOnlySequence<byte>(vcard[s..(s + Math.Min(vcard.Length - s, e))]);
            //     var reader = new SequenceReader<byte>(seq);

            //     if (!VCard.ExecutePartial(ref reader, out match, out status, ref obj))
            //         break;

            //     if (reader.Consumed == 0)
            //         e++;

            //     s += (int) reader.Consumed;
            // }

            // PrintMatch(match);
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
