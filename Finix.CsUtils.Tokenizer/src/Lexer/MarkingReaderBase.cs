using System;
using System.IO;
namespace Finix.CsUtils
{
    public abstract class MarkingReaderBase
    {
        public MarkingReaderBase(string? source = null)
        {
            Position = new(source, 1, 1);
        }

        public virtual string? Source => Position.Source;

        public Mark Position { get; protected set; }

        public abstract ParserString Peek();

        public abstract ParserString Read(int count = 1);

        public virtual ParserString ReadLine(bool keepNewline = false)
        {
            var str = Read();

            while (!str.StringValue.EndsWith('\n'))
            {
                var ch = Read();

                if (ch.EOF)
                {
                    str = str with { EOF = true };
                    break;
                }

                str += ch;
            }

            return keepNewline || !str.StringValue.EndsWith('\n') ? str : str[..^1];
        }
    }
}
