using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Finix.CsUtils.Tokens;
using Serilog;

namespace Finix.CsUtils
{
    public class VCFPrinter : IPrinter
    {
        private static ILogger Log => Serilog.Log.ForContext<VCFPrinter>();

        public VCFPrinter(TextWriter writer)
        {
            Writer = writer;
        }

        public TextWriter Writer { get; }

        public bool UseOldEscaping { get; set; }

        public bool UseNonUTF8Escaping { get; set; }

        public void Write(Token token)
        {
            Log.Verbose("Writing {@token}", token);

            switch (token)
            {
                case VCardStart:
                    Writer.WriteLine(VCF.BEGIN_VCARD);
                    break;

                case VCardEnd:
                    Writer.WriteLine(VCF.END_VCARD);
                    break;

                case VCardVersion version:
                    Writer.Write(VCF.VERSION);
                    Writer.Write(version.Major);
                    Writer.Write('.');
                    Writer.Write(version.Minor);
                    Writer.WriteLine();
                    break;

                case Name name:
                    Writer.Write(name.Value);
                    break;

                case ParameterSeperator:
                case ComponentSeperator:
                    Writer.Write(';');
                    break;

                case ListSeperator:
                    Writer.Write(',');
                    break;

                case ParameterValueStart:
                    Writer.Write('=');
                    break;

                case ValueStart:
                    Writer.Write(':');
                    break;

                case AttributeEnd:
                    Writer.WriteLine();
                    break;

                case Scalar scalar:
                    WriteScalar(scalar);
                    break;

                default:
                    throw new NotSupportedException($"Unknown token {token.GetType()}");
            }
        }

        private string ConvertRune(Rune r)
        {
            if (VCF.IsSafeChar(r) || (!r.IsAscii && !UseNonUTF8Escaping))
                return r.ToString();

            if (r.Value == '\n')
                return UseOldEscaping ? "=\n" : "\n ";

            if (UseOldEscaping || !r.IsAscii)
            {
                var b = new byte[r.Utf8SequenceLength];
                var count = r.EncodeToUtf8(b);

                return String.Join(String.Empty, b.Take(count).Select(b => $"={b:X2}"));
            }
            else
            {
                return $"\\{r}";
            }
        }

        private void WriteScalar(Scalar scalar)
        {
            if (scalar.Value.EnumerateRunes().All(VCF.IsSafeChar))
            {
                Writer.Write(scalar.Value);
            }
            else
            {
                var value = scalar.Value.EnumerateRunes().Select(ConvertRune);

                foreach (var val in value)
                    Writer.Write(val);
            }
        }
    }
}
