using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public interface IBTreeValue<TKey, TValue>
    {
        int ArityIndex { get; }

        IBTreeNode<TKey, TValue> TreeNode { get; }

        bool IsAvailable { get; }

        TValue Value { get; }

        TKey Key { get; }

        IBTreeNode<TKey, TValue> LesserNode { get; set; }

        void Set(TKey key, TValue value);

        void Unset(bool delete = true);
    }
}
