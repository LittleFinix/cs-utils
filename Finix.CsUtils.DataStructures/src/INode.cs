using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public interface INode<T>
    {
        T Value { get; set; }

        INode<T> Parent { get; }

        IReadOnlyList<INode<T>> Children { get; }
    }
}
