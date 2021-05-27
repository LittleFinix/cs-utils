using System;
using System.Text;
using System.IO;

namespace Finix.CsUtils
{
    /// <summary>
    /// A <see cref="TextReader" /> that reads VCF escaped linebreaks
    /// </summary>
    public class EscapedLinebreakReader : TextReader
    {
        private readonly StringBuilder buffer = new();

        /// <summary>
        /// Creates a new EscapedLinebreakReader
        /// </summary>
        /// <param name="reader">Another <see cref="TextReader"/> for this reader to use.</param>
        public EscapedLinebreakReader(TextReader reader)
        {
            BaseReader = reader;
        }

        /// <summary>
        /// Gets the <see cref="TextReader"/> used by this reader.
        /// </summary>
        /// <value></value>
        public TextReader BaseReader { get; }

        private bool Expand()
        {
            // Check for EOF from the base reader.
            var next = BaseReader.Peek();

            // The base reader returns -1 on EOF, in which case to return false.
            if (next == -1)
                return false;

            var l = String.Empty;

            // Since we just read past a line break (and continue doing so incease the condition succeeds),
            // while the next character is a space or tab, read that line as well.
            do
            {
                l = BaseReader.ReadLine();

                Console.WriteLine(l);

                if (l == null || l.Length == 0)
                    break;

                if (l[^1] == '=')
                {
                    l = l[..^1];
                }

                // Append the read line to the buffer.
                buffer.Append(l);
            }
            while (l[^1] == '=' || BaseReader.Peek() is ' ' or '\t');

            buffer.AppendLine();
            return true;
        }

        /// <inheritdoc />
        public override void Close()
        {
            BaseReader.Close();
        }

        /// <inheritdoc />
        public override int Peek()
        {
            // If the buffer is empty and we're unable to expand, return -1.
            if (buffer.Length == 0 && !Expand())
                return -1;

            return buffer[0];
        }

        /// <inheritdoc />
        public override int Read()
        {
            // Peek the next character, and if the character is
            // not -1, remove it from the buffer.
            var i = Peek();

            if (i != -1)
                buffer.Remove(0, 1);

            return i;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BaseReader.Dispose();
                buffer.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
