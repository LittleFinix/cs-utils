using System.Collections.Immutable;
using System;
using System.IO;
using System.Text;

namespace Finix.CsUtils
{
    public sealed class EOFMarkingReader : MarkingReaderBase
    {
        public EOFMarkingReader()
        {
        }

        public override ParserString Peek()
        {
            return new ParserString(Position, Position, ImmutableArray<Rune>.Empty) { EOF = true };
        }

        public override ParserString Read(int count = 1)
        {
            return new ParserString(Position, Position, ImmutableArray<Rune>.Empty) { EOF = true };
        }

        public override ParserString ReadLine(bool keepNewline = false)
        {
            return new ParserString(Position, Position, ImmutableArray<Rune>.Empty) { EOF = true };
        }
    }
}
