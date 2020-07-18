using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public sealed unsafe class BinaryHeapNode<TKey, TValue> : IBinaryNode<TKey, TValue> where TValue : unmanaged where TKey : unmanaged, IComparable<TKey>
    {
        private ulong index, parentIndex, leftChildIndex, rightChildIndex;

        private IBinaryNode<TKey, TValue> parent, leftChild, rightChild;

        private IReadOnlyList<IBinaryNode<TKey, TValue>> children;

        private IPageAccessor<BinaryHeap<TKey, TValue>.Data> pages;

        /// <summary>
        /// Constructs a new BinaryHeapNode
        /// </summary>
        /// <param name="pages">A list of pointers to pages</param>
        /// <param name="pageIndex">The index of the page in pages</param>
        /// <param name="pageSize">The number of values per page</param>
        /// <param name="index">The index inside the page, must be less than pageSize</param>
        public BinaryHeapNode(IPageAccessor<BinaryHeap<TKey, TValue>.Data> pages, ulong index)
        {
            this.index = index;
            this.pages = pages;

            parentIndex = TreeHelper.CalculateParentIndex(index);
            leftChildIndex = TreeHelper.CalculateLeftChildIndex(index);
            rightChildIndex = TreeHelper.CalculateRightChildIndex(index);
        }

        public IBinaryNode<TKey, TValue> Parent => Reference.GetOrCreate(ref parent, pages, parentIndex);

        public IBinaryNode<TKey, TValue> LeftChild => Reference.GetOrCreate(ref leftChild, pages, leftChildIndex);

        public IBinaryNode<TKey, TValue> RightChild => Reference.GetOrCreate(ref leftChild, pages, leftChildIndex);

        public IReadOnlyList<IBinaryNode<TKey, TValue>> Children => Reference.GetOrActivate(ref children, () => new IBinaryNode<TKey, TValue>[] { LeftChild, RightChild });

        public ref TValue Value
        {
            get
            {
                var page = pages.GetPageFor(index);

                if (!page.IsIndexUsed(index))
                    throw new IndexOutOfRangeException();

                ref var data = ref page.GetReference(index).Value;
                return ref data.Value;
            }
        }

        public TKey Key => pages[index].Key;

        TValue INode<TValue>.Value
        {
            get => Value;
            set => Value = value;
        }

        INode<TValue> INode<TValue>.Parent => Parent;

        IReadOnlyList<INode<TValue>> INode<TValue>.Children => Children;
    }
}
