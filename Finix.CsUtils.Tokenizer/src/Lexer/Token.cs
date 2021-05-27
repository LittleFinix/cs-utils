using System;
namespace Finix.CsUtils
{
    /// <summary>
    /// A token that was read by a <see cref="LexerBase" /> or any of its subtypes.
    /// </summary>
    /// <param name="Position">The position at which the token occurred.</param>
    /// <param name="Value">The value that is associated with the token.</param>
    public abstract record Token
    {
        public Mark Position { get; init; }

        public string Value { get; init; }

        public Token(Mark position, string value)
        {
            Position = position;
            Value = value;
        }

        public Token(ParserOutput parserOutput)
            : this(parserOutput.Start, parserOutput.StringValue)
        {

        }
    }
    public record EOF(Mark Position) : Token(Position, String.Empty);
}
