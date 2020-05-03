using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace Finix.CsUtils.IO
{
    public class TransformStream : Stream
    {
        public int Count { get; }

        public bool Input { get; }

        public Func<byte[], byte[]> Transformer { get; }

        public Stream BaseStream { get; }

        public byte[] Buffer { get; }

        public TransformStream(Stream baseStream, int count, bool input, Func<byte[], byte[]> transformer, int? buffer = null)
        {
            BaseStream = baseStream;
            Count = count;
            Buffer = new byte[buffer ?? Count];
            Input = input;
            Transformer = transformer;
        }

        public override bool CanRead => Input;

        public override bool CanSeek => false;

        public override bool CanWrite => !Input;

        public override long Length => throw new InvalidOperationException();

        public override long Position { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

        public override void Flush()
        {
            if (!CanWrite)
                throw new InvalidOperationException();

            BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {

            return 0;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
        }

        protected override void Dispose(bool disposing)
        {
            BaseStream.Dispose();
        }

    }
}
