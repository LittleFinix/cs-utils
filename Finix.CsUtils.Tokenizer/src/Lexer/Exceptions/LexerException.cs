using System;
using System.Runtime.Serialization;

namespace Finix.CsUtils
{
    [Serializable]
    public class LexerException : Exception
    {
        public LexerException(Mark position) : base($"{position:F}: A lexing error occurred.")
        {
            Position = position;
        }

        public LexerException(Mark position, string message) : base($"{position:F}: " + message)
        {
            Position = position;
        }

        public LexerException(Mark position, Exception source) : base($"{position:F}: " + source.Message, source)
        {
            Position = position;
        }

        public LexerException(Mark position, string message, Exception inner) : base($"{position:F}: " + message, inner)
        {
            Position = position;
        }

        protected LexerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Position = new(null, 0, 0);
        }

        public Mark Position { get; }
    }
}
