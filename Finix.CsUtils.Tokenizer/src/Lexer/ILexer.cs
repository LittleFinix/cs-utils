using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public interface ILexer
    {
        ITextParser Parser { get; }

        Token Consume();

        Token Peek();
    }
}
