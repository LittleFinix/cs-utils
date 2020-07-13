using System;
using System.Collections.Generic;

namespace Finix.CsUtils
{
    public interface ITree<T>
    {
        int Depth { get; }

        bool IsBalanced { get; }

        INode<T> RootNode { get; }

        void Balance();
    }
}
