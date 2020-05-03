using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace Finix.CsUtils.IO
{
    public class SplitStream : Stream
    {
        public List<Stream> Streams { get; }

        public SplitStream(IEnumerable<Stream> streams)
        {
            Streams = streams.ToList();
        }

        public SplitStream(params Stream[] streams) : this((IEnumerable<Stream>) streams)
        {
        }

        public bool DontThrow { get; set; } = false;

        public override bool CanRead => Streams.Any(s => s.CanRead);

        public override bool CanSeek => false;

        public override bool CanWrite => Streams.Any(s => s.CanWrite);

        public override long Length => throw new InvalidOperationException();

        public override long Position { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

        public override void Flush()
        {
            if (!CanWrite)
                throw new InvalidOperationException();

            foreach (var stream in Streams)
            {
                stream.Flush();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead)
                return DontThrow ? -1 : throw new InvalidOperationException();

            foreach (var stream in Streams.Where(s => s.CanRead))
            {
                var read = UnlessDisposed(stream, s => s.Read(buffer, offset, count));

                if (read > 0)
                    return read;
            }

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
            if (!CanWrite)
            {
                if (DontThrow)
                    return;

                throw new InvalidOperationException();
            }

            var tasks = Streams.Where(s => s.CanWrite).Select(UnlessDisposed(stream =>
                stream.WriteAsync(buffer, offset, count)
            ));

            Task.WhenAll(tasks).Wait();
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var s in Streams)
            {
                s.Dispose();
            }
        }

        private T UnlessDisposed<T>(Stream stream, Func<Stream, T> action)
        {
            try
            {
                return action(stream);
            }
            catch (ObjectDisposedException)
            {
                Streams.Remove(stream);
                return default;
            }
        }

        private Func<Stream, Task> UnlessDisposed(Func<Stream, Task> action)
        {
            return async (stream) => {
                try
                {
                    await action(stream);
                }
                catch (ObjectDisposedException)
                {
                    Streams.Remove(stream);
                }
            };
        }
    }
}
