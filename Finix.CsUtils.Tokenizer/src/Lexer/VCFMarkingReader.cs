using System;
using System.IO;
namespace Finix.CsUtils
{
    public class VCFMarkingReader : MarkingReaderBase
    {
        public VCFMarkingReader(MarkingReaderBase baseReader)
            : base(baseReader.Source)
        {
            BaseReader = baseReader;
            Position = new(Source, 1, 1);
        }

        public MarkingReaderBase BaseReader { get; }

        public override ParserString Peek()
        {
            return BaseReader.Peek();
        }

        private ParserString TrimLineBreak(ParserString str)
        {
            if (str.StringValue.EndsWith("\r\n"))
                return str[..^2];
            else if (str.StringValue.EndsWith("=\n"))
                return str[..^2];
            else if (str.StringValue.EndsWith('\n'))
                return str[..^1];
            else
                return str;
        }

        private bool IsNextCharSpace()
        {
            return BaseReader.Peek() is var r && !r.EOF && r.Runes[0].Value == ' ';
        }

        private bool IsEscapedLineBreak(ParserString str)
        {
            return str.StringValue.EndsWith("=\n") || str.StringValue.EndsWith('\n') && IsNextCharSpace();
        }

        public override ParserString Read(int count = 1)
        {
            var str = BaseReader.Read(count);
            str = str.WithString(str.StringValue.Replace("=\n", String.Empty));
            str = str.WithString(str.StringValue.Replace("\n ", " "));

            if (IsEscapedLineBreak(str))
                str = TrimLineBreak(str) + BaseReader.Read();

            Position = BaseReader.Position;

            return str with { EOF = BaseReader.Peek().EOF };
        }

        public override ParserString ReadLine(bool keepNewline = false)
        {
            var str = BaseReader.ReadLine(true);

            while (IsEscapedLineBreak(str))
                str = TrimLineBreak(str) + BaseReader.ReadLine(true);

            Position = BaseReader.Position;

            str = str with { EOF = BaseReader.Peek().EOF };

            return keepNewline || !str.StringValue.EndsWith('\n') ? str : TrimLineBreak(str);
        }
    }
}
