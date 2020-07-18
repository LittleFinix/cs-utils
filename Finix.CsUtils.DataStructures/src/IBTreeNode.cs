using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public interface IBTreeNode<TKey, TValue> : INode<TValue>
    {
        bool IsAvailable { get; }

        IBTreeValue<TKey, TValue> Owner { get; set; }

        new IBTreeNode<TKey, TValue> Parent { get; }

        new IReadOnlyList<IBTreeNode<TKey, TValue>> Children { get; }

        IReadOnlyList<IBTreeValue<TKey, TValue>> Values { get; }

        bool Follow(TKey key, out IBTreeValue<TKey, TValue> node);

        IBTreeValue<TKey, TValue> Add(TKey key, TValue value);

        IBTreeValue<TKey, TValue> Split();

        IBTreeValue<TKey, TValue> MigrateUp();

        void MigrateChildren();

        void Delete();
    }
}
