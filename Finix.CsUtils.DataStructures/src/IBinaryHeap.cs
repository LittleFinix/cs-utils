using System;

namespace Finix.CsUtils
{
    public interface IBinaryHeap<TKey, TValue> : ITree<TValue>
    {
        new IBinaryNode<TKey, TValue> RootNode { get; }

        IBinaryNode<TKey, TValue>? FindValue(TKey key);

        void Add(TKey key, TValue value);

        TValue Remove(TKey key);

        TValue this[TKey key] { get; set; }
    }
}
