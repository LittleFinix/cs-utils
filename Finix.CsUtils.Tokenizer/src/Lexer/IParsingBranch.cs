using System;

namespace Finix.CsUtils
{
    /// <summary>
    /// An interface representing a parsing branch that shall return its read contents upon dispose.
    /// </summary>
    public interface IParsingBranch : ITextParser, IDisposable
    {
    }
}
