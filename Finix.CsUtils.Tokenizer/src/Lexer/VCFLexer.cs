using System.ComponentModel;
using System.Security;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Finix.CsUtils.Tokens;
using System.Linq;

namespace Finix.CsUtils
{
    /// <summary>
    /// A lexer base class that reads tokens from a parser.
    /// </summary>
    public sealed class VCFLexer : LexerBase
    {
        public VCFLexer(ITextParser parser) : base(parser)
        {
        }

        #region Helper Methods (Throw)

        private static Exception InvalidEscapeSequence(string escapeSequence)
        {
            return new Exception($"'{escapeSequence}' is not a valid escape sequence");
        }

        #endregion

        #region Helper Methods (Reading)

        private ParserString Expect(params object[] keyword)
        {
            if (!Parser.TryRead(out var kw, StringComparer.OrdinalIgnoreCase, keyword))
                throw UnexpectedString(String.Join(", ", keyword), Parser.ReadWhile(VCF.IsValidNameCharacter));

            return kw;
        }

        private bool TryRead([MaybeNullWhen(false)] out ParserString str, params object[] keyword)
        {
            return Parser.TryRead(out str, StringComparer.OrdinalIgnoreCase, keyword);
        }

        private int ReadInt()
        {
            ConsumeWhitespace();

            var i = Parser.ReadWhile(Rune.IsDigit);

            if (i.Runes.Length == 0)
                throw UnexpectedLiteral("a number", i);

            return Int32.Parse(i.StringValue, provider: CultureInfo.InvariantCulture);
        }

        private Name ReadName()
        {
            ConsumeWhitespace();

            var name = Parser.ReadWhile(VCF.IsValidNameCharacter);

            // if (name.Runes.Length == 0)
            //     throw UnexpectedCharacter(Parser.Peek());

            return new Name(name);
        }

        private Scalar ReadText()
        {
            ConsumeWhitespace();

            if (Parser.TryRead(out _, VCF.QUOTE))
            {
                var str = Parser.ReadWhile(VCF.IsQSafeChar, new Rune('\\'), out var matched);

                if (matched.Value.Value != VCF.QUOTE)
                    throw UnexpectedCharacter(matched);

                Parser.Read();
                ConsumeWhitespace();

                return new Scalar(str.WithString(VCF.Unescape(str.StringValue)));
            }
            else
            {
                var str = Parser.ReadWhile(VCF.IsValidTextChar, new Rune('\\'), out var matched);
                return new Scalar(str.WithString(VCF.Unescape(str.StringValue.Trim())));
            }
        }

        private Scalar ReadString()
        {
            ConsumeWhitespace();

            if (Parser.TryRead(out _, VCF.QUOTE))
            {
                var str = Parser.ReadWhile(VCF.IsQSafeChar, new Rune('\\'), out var matched);

                if (matched.Value.Value != VCF.QUOTE)
                    throw UnexpectedCharacter(matched);

                Parser.Read();
                ConsumeWhitespace();

                return new Scalar(str.WithString(VCF.Unescape(str.StringValue)));
            }
            else
            {
                var str = Parser.ReadWhile(VCF.IsSafeChar, new Rune('\\'), out var matched);
                return new Scalar(str.WithString(VCF.Unescape(str.StringValue.Trim())));
            }
        }

        #endregion

        protected override IEnumerator<Token> Start()
        {
            ConsumeWhitespace();

            while (!Parser.Peek().EOF)
            {
                if (TryRead(out var str, VCF.BEGIN_VCARD))
                {
                    Expect(VCF.NEWLINE);
                    yield return new VCardStart(str);

                    continue;
                }
                else if (TryRead(out str, VCF.END_VCARD))
                {
                    TryRead(out _, VCF.NEWLINE);
                    yield return new VCardEnd(str);

                    ConsumeWhitespace();

                    continue;
                }
                else if (TryRead(out str, VCF.VERSION))
                {
                    var major = ReadInt();
                    Expect('.');
                    var minor = ReadInt();

                    Expect(VCF.NEWLINE);

                    yield return new VCardVersion(str) {
                        Major = major,
                        Minor = minor
                    };

                    continue;
                }

                yield return ReadName();

                while (!TryRead(out str, VCF.VALUE_SEPERATOR))
                {
                    yield return new ParameterSeperator(Expect(VCF.PARAM_SEPERATOR));
                    yield return ReadName();

                    if (TryRead(out str, VCF.PARAM_VALUE))
                    {
                        yield return new ParameterValueStart(str);

                        while (true)
                        {
                            yield return ReadString();

                            if (!TryRead(out str, VCF.LIST_SERPERATOR))
                                break;

                            yield return new ListSeperator(str);
                        }
                    }
                }

                yield return new ValueStart(str);

                while (!TryRead(out str, VCF.NEWLINE))
                {
                    if (TryRead(out str, VCF.LIST_SERPERATOR, VCF.COMPONENT_SERPERATOR))
                    {
                        yield return str.Runes[0].Value switch {
                            VCF.LIST_SERPERATOR => new ListSeperator(str),
                            VCF.COMPONENT_SERPERATOR => new ComponentSeperator(str),
                            int r => throw UnexpectedCharacter(new Rune(r), str),
                        };
                    }
                    else
                    {
                        yield return ReadText();
                    }
                }


                yield return new AttributeEnd(str);
            }

            yield break;
        }
    }
}
