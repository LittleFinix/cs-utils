using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public interface IBinaryNode<TKey, TValue> : INode<TValue>
    {
        new IBinaryNode<TKey, TValue> Parent { get; }

        IBinaryNode<TKey, TValue> LeftChild { get; }

        IBinaryNode<TKey, TValue> RightChild { get; }

        new IReadOnlyList<IBinaryNode<TKey, TValue>> Children { get; }

        new ref TValue Value { get; }

        TKey Key { get; }
    }
}
