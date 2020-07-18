using System;

namespace Finix.CsUtils
{
    public interface IBinaryHeap<TKey, TValue> : ITree<TValue>
    {
        new IBinaryNode<TKey, TValue> RootNode { get; }

        void Add(TKey key, TValue value);

        TValue Remove(TKey key);
    }
}
