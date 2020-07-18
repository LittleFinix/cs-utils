using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public class BinaryHeap<TKey, TValue> : IBinaryHeap<TKey, TValue> where TValue : unmanaged where TKey : unmanaged, IComparable<TKey>
    {
        private ulong lastIndex = 0;

        public const ulong MAX_DEPTH = 64;

        private IPageAccessor<Data> pages;

        public BinaryHeap(IPageAccessor<Data> pageAccessor)
        {
            // Console.WriteLine($"Creating new heap");
            pages = pageAccessor;
            RootNode = new BinaryHeapNode<TKey, TValue>(pages, 0);
        }

        public IBinaryNode<TKey, TValue> RootNode { get; }

        public int Depth => throw new NotImplementedException();

        public bool IsBalanced => true; // Heap is always balanced

        INode<TValue> ITree<TValue>.RootNode => RootNode;

        public void Balance()
        {
            return;
        }

        public void Add(TKey key, TValue value)
        {
            if (FindIndex(key, out var idx))
            {
                // Console.WriteLine($"Adding value to heap at existing index {idx}");

                pages.GetReference(idx).Value.Value = value;
                return;
            }

            var index = FindLastFreeIndex();

            var page = pages.GetPageFor(index);

            page.Set(index);
            page[index] = new Data {
                Key = key,
                Value = value
            };

            // Console.WriteLine($"Adding value to heap at new index {idx}");

            lastIndex = index + 1;
            BubbleUp(index);
        }

        public TValue Remove(TKey key)
        {
            if (!FindIndex(key, out var idx))
                return default;

            var val = pages[idx].Value;
            var swapIdx = FindLastFreeIndex();

            pages.GetPageFor(idx).Clear(idx);

            if (swapIdx == 0)
                return val;

            Swap(idx, swapIdx - 1);
            BubbleDown(swapIdx - 1);

            return val;
        }

        private ulong FindLastFreeIndex()
        {
            return lastIndex;
        }

        private bool FindIndex(TKey key, out ulong idx)
        {
            idx = 0;

            while (true) // idx == 0 || TreeHelper.CalculateDepthForIndex(idx) < MAX_DEPTH
            {
                var page = pages.GetPageFor(idx);

                if (!page.IsIndexUsed(idx))
                    return false;

                var comp = key.CompareTo(page[idx].Key);

                if (comp == 0)
                    return true;
                else if (comp > 0)
                    idx = TreeHelper.CalculateLeftChildIndex(idx);
                else
                    idx = TreeHelper.CalculateRightChildIndex(idx);
            }

            // return false;
        }

        private void BubbleUp(ulong index)
        {
            var parent = TreeHelper.CalculateParentIndex(index);
            // Console.WriteLine($"BUBBLE UP: bubbling {index} (parent: {parent})");

            while (index > 0 && pages[index].Key.CompareTo(pages[parent].Key) < 0)
            {
                // Console.WriteLine($"BUBBLE UP: Swapping {index} and {parent}");
                Swap(index, parent);

                index = parent;
                parent = TreeHelper.CalculateParentIndex(index);
            }
        }

        private void BubbleDown(ulong index)
        {
            while (SmallerChild(index, out var child))
            {
                Console.WriteLine($"BUBBLE DOWN: Swapping {index} and {child}");

                Swap(index, child);
                index = child;
            }
        }

        private bool SmallerChild(ulong index, out ulong child)
        {
            var key = pages.GetPageFor(index)[index].Key;

            var lchild = TreeHelper.CalculateLeftChildIndex(index);
            var rchild = TreeHelper.CalculateRightChildIndex(index);

            var p1 = pages.GetPageFor(lchild);
            var p2 = pages.GetPageFor(rchild);

            var lchild_avail = p1.IsIndexUsed(lchild);
            var rchild_avail = p2.IsIndexUsed(rchild);

            child = 0;
            if (lchild_avail && !rchild_avail)
            {
                child = lchild;
                return key.CompareTo(p1[lchild].Key) > 0;
            }
            else if (rchild_avail && !lchild_avail)
            {
                child = rchild;
                return key.CompareTo(p2[rchild].Key) > 0;
            }
            else if (rchild_avail && lchild_avail)
            {
                var lchild_data = p1[lchild].Key;
                var rchild_data = p2[rchild].Key;

                if (rchild_data.CompareTo(lchild_data) > 0)
                {
                    child = lchild;
                    return key.CompareTo(lchild_data) > 0;
                }
                else
                {
                    child = rchild;
                    return key.CompareTo(rchild_data) > 0;
                }
            }
            else
            {
                return false;
            }
        }

        private void Swap(ulong indexA, ulong indexB)
        {
            var valA = pages[indexA];
            var valB = pages[indexB];

            pages[indexB] = valA;
            pages[indexA] = valB;
        }

        public struct Data
        {
            public TKey Key;

            public TValue Value;
        }
    }
}
