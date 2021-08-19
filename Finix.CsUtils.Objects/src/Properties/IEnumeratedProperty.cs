
using System.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public interface IEnumeratedProperty : IProperty
    {
        IProperties Root { get; }

        string Path { get; }

        int Depth { get; }
    }

    public interface IEnumeratedProperty<T> : IEnumeratedProperty, IProperty<T>
    {
    }
}
