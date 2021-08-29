
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

        IEnumerable<IEnumeratedProperty> EnumerateObjectType(bool recurse, int depth, string path);

        IEnumerable<IEnumeratedProperty> EnumerateObjectType(bool recurse);
    }

    public interface IEnumeratedProperty<T> : IEnumeratedProperty, IProperty<T>
    {
    }
}
