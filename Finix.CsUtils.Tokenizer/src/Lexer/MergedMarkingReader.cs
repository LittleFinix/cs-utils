using System.Collections.Generic;
using System;
using System.IO;
namespace Finix.CsUtils
{
    public class MergedMarkingReader : MarkingReaderBase
    {
        public MergedMarkingReader(IEnumerable<MarkingReaderBase> readers)
        {
            Readers = readers;
            enumerator = Readers.GetEnumerator();
        }

        public IEnumerable<MarkingReaderBase> Readers { get; }

        private IEnumerator<MarkingReaderBase> enumerator;

        private MarkingReaderBase Current
        {
            get
            {
                if ((enumerator.Current is null || enumerator.Current.Peek().EOF) && !enumerator.MoveNext())
                    return new EOFMarkingReader();

                return enumerator.Current ?? new EOFMarkingReader();
            }
        }

        public override ParserString Peek()
        {
            var next = Current.Peek();
            return next with { EOF = Current.Peek().EOF };
        }

        public override ParserString Read(int count = 1)
        {
            var str = Current.Read(count);
            return str with { EOF = Current.Peek().EOF };
        }

        public override ParserString ReadLine(bool keepNewline = false)
        {
            var str = Current.ReadLine(keepNewline);
            return str with { EOF = Current.Peek().EOF };
        }
    }
}
