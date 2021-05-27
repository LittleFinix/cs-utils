using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public interface IPrinter
    {
        TextWriter Writer { get; }

        void Write(Token token);
    }
}
