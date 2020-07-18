using System;
using System.Collections.Generic;

namespace Finix.CsUtils
{
    public interface IBTree<TKey, TValue> : ITree<TValue>
    {
        int Arity { get; }

        bool Unique { get; }

        new IBTreeNode<TKey, TValue> RootNode { get; }

        IBTreeValue<TKey, TValue>? FindValue(TKey key);

        void Add(TKey key, TValue value);

        TValue Remove(TKey key);

        TValue this[TKey key] { get; set; }
    }
}
