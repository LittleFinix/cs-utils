using System.Collections.Specialized;
using System;
using System.IO;
using System.Text;

namespace Finix.CsUtils
{
    public class MarkingReader : MarkingReaderBase
    {
        public MarkingReader(TextReader baseReader, string? source = null)
            : base(source ?? Mark.SourceFrom(baseReader))
        {
            BaseReader = baseReader;
            Position = new(Source, 1, 1);
        }

        public TextReader BaseReader { get; }

        public override ParserString Peek()
        {
            var pos = Position;
            var chr = BaseReader.Peek();

            if (chr == -1)
                return new(pos, pos, String.Empty) { EOF = true };

            var str = ((char) chr).ToString();

            return new ParserString(pos, pos.MoveMarkUp(str), str);
        }

        public override ParserString Read(int count = 1)
        {
            var pos = Position;
            var buf = new Rune[count];
            var eof = false;

            while (count > 0)
            {
                var ch = BaseReader.Read();

                if (ch == -1)
                {
                    eof = true;
                    break;
                }

                if (!Rune.TryCreate((char) ch, out var rune))
                {
                    ch = BaseReader.Read();

                    if (ch == -1)
                    {
                        eof = true;
                        break;
                    }

                    Rune.TryCreate((char) ch, out rune);
                }

                buf[^count--] = rune;
            }

            var str = String.Join(String.Empty, buf);

            return new(pos, Position = pos.MoveMarkUp(str), str) { EOF = eof };
        }

        public override ParserString ReadLine(bool keepNewline = false)
        {
            var pos = Position;
            var str = BaseReader.ReadLine();

            if (str is null)
                return new(pos, pos, String.Empty) { EOF = true };

            if (keepNewline && BaseReader.Peek() != -1)
                str += '\n';

            var end = pos.MoveMarkUp(str);
            Position = keepNewline ? end : end.MoveMarkUp("\n");

            return new(pos, end, str);
        }
    }
}
