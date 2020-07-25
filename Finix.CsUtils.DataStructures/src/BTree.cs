using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public class BTree<TKey, TValue> : IBTree<TKey, TValue> where TValue : unmanaged where TKey : unmanaged, IComparable<TKey>
    {
        private ulong lastIndex = 0;

        public const ulong MAX_DEPTH = 64;

        private IPageAccessor<BTreeNode<TKey, TValue>.Data> pages;

        public BTree(IPageAccessor<BTreeNode<TKey, TValue>.Data> pageAccessor, int arity = 2)
        {
            // Console.WriteLine($"Creating new heap");
            pages = pageAccessor;
            Arity = arity;

            if (!pages.GetPageFor(1).IsIndexUsed(1))
            {
                for (var i = 0; i <= arity; i++)
                {
                    var idx = (ulong) i;
                    idx++;

                    var page = pages.GetPageFor(idx);
                    page.Set(idx);
                    page[idx] = new BTreeNode<TKey, TValue>.Data {
                        IsAvailable = false,
                        LesserIndex = 0
                    };
                }
            }

            RootNode = new BTreeNode<TKey, TValue>(pages, null, arity, 1);
        }

        public IBTreeNode<TKey, TValue> RootNode { get; }

        public int Depth { get; private set; } = 1;

        public bool IsBalanced => true; // BTree is always balanced

        INode<TValue> ITree<TValue>.RootNode => RootNode;

        public int Arity { get; }

        public bool Unique => true;

        public void Balance()
        {
            return;
        }

        private void Swap(ulong indexA, ulong indexB)
        {
            var valA = pages[indexA];
            var valB = pages[indexB];

            pages[indexB] = valA;
            pages[indexA] = valB;
        }

        private IBTreeValue<TKey, TValue>? FindValue(TKey key, out IBTreeValue<TKey, TValue> parent)
        {
            var node = RootNode;
            parent = RootNode.Values[0];

            int d = 0;

            while (parent.Key.CompareTo(key) != 0 && node != null && node.Follow(key, out parent))
            {
                if (node.Equals(parent.LesserNode))
                {
                    throw new Exception("Infinite Loop");
                }

                node = parent.LesserNode;
                d++;
            }

            // Console.WriteLine($"Depth {d}");

            return parent.IsAvailable && parent.Key.CompareTo(key) == 0 ? parent : null;
        }

        public void Add(TKey key, TValue value)
        {
            var node = FindValue(key, out var parent);

            if (node != null)
            {
                node.Set(key, value);
                return;
            }

            // while (parent.TreeNode.Owner != null && !parent.IsAvailable) {
            //     parent = parent.TreeNode.Owner;
            // }

            if (parent.IsAvailable && parent.Key.CompareTo(key) > 0 && parent.LesserNode != null)
            {
                parent = parent.LesserNode.Add(key, value);
            }
            else
            {
                parent.TreeNode.Add(key, value);
            }

            var d = 0;
            var p = parent.TreeNode;
            var prev = p;
            while ((p = p.Parent) != null)
            {
                if (p == prev)
                    break;

                prev = p;
                d++;
            }

            Depth = Math.Max(Depth, d);
        }

        public TValue Remove(TKey key)
        {
            var node = FindValue(key, out var parent);

            if (node == null)
                return default;

            return default;
        }

        public IBTreeValue<TKey, TValue>? FindValue(TKey key)
        {
            return FindValue(key, out _);
        }

        public TValue this[TKey key]
        {
            get => FindValue(key, out _)?.Value ?? throw new KeyNotFoundException($"{key}");
            set => Add(key, value);
        }
    }
}
