using System.Linq;
using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public sealed unsafe class BTreeNode<TKey, TValue> : IBTreeNode<TKey, TValue> where TValue : unmanaged where TKey : unmanaged, IComparable<TKey>
    {

        private ulong index, parentIndex;

        private IBTreeValue<TKey, TValue>? parent;

        private IReadOnlyList<IBTreeValue<TKey, TValue>> values;

        private IReadOnlyList<IBTreeNode<TKey, TValue>> siblings;

        private IPageAccessor<BTreeNode<TKey, TValue>.Data> pages;

        private int arity;

        /// <summary>
        /// Constructs a new BinaryHeapNode
        /// </summary>
        /// <param name="pages">A list of pointers to pages</param>
        /// <param name="pageIndex">The index of the page in pages</param>
        /// <param name="pageSize">The number of values per page</param>
        /// <param name="index">The index inside the page, must be less than pageSize</param>
        public BTreeNode(IPageAccessor<BTreeNode<TKey, TValue>.Data> pages, IBTreeValue<TKey, TValue>? parent, int arity, ulong index)
        {
            if (arity < 2)
                throw new ArgumentOutOfRangeException(nameof(arity), arity, "Arity must be greater or equal to two");

            if (index == 0)
                throw new ArgumentNullException(nameof(index), "The node's index may not be 0");

            this.pages = pages;
            this.parent = parent;
            this.index = index;
            this.arity = arity;
        }

        public bool IsAvailable => pages.GetPageFor(index).IsIndexUsed(index);

        public IReadOnlyList<IBTreeNode<TKey, TValue>> Children => Values.Select(v => v.LesserNode).ToList();

        public IReadOnlyList<IBTreeValue<TKey, TValue>> Values => Reference.GetOrActivate(ref values, () => {
            var list = new List<IBTreeValue<TKey, TValue>>();

            for (var i = 0UL; i <= (ulong) arity; i++)
                list.Add(new BTreeNode<TKey, TValue>.Value(this, index + i));

            return list;
        });

        public ref TValue ValueRef
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

        public IBTreeValue<TKey, TValue> Owner
        {
            get => parent;
            set => parent = value;
        }

        public IBTreeNode<TKey, TValue> Parent => parent?.TreeNode;

        TValue INode<TValue>.Value
        {
            get => ValueRef;
            set => ValueRef = value;
        }

        INode<TValue> INode<TValue>.Parent => Parent;

        IReadOnlyList<INode<TValue>> INode<TValue>.Children => Children;

        public IBTreeNode<TKey, TValue>? PreviousSibling => throw new NotImplementedException();

        public IBTreeNode<TKey, TValue>? NextSibling => throw new NotImplementedException();

        public IBTreeNode<TKey, TValue> GreaterNode => throw new NotImplementedException();

        public bool Follow(TKey key, out IBTreeValue<TKey, TValue> node)
        {
            node = default;

            for (var i = 0; i < arity; i++)
            {
                var n = Values[i];

                node = n;
                if (!n.IsAvailable)
                {
                    return i > 0;
                }

                if (key.CompareTo(n.Key) <= 0)
                    return true;
            }

            return false;
        }

        public IBTreeValue<TKey, TValue> Add(TKey key, TValue value)
        {
            var val = Values[0];

            int i;
            for (i = 0; i < Values.Count; i++)
            {
                var v = Values[i];

                if (v.IsAvailable)
                {
                    if (v.Key.CompareTo(key) == 0)
                    {
                        v.Set(key, value);
                        return val;
                    }

                    continue;
                }

                val = v;
                break;
            }

            val.Set(key, value);

            if (i == arity)
                Split();

            return val;
        }

        public IBTreeValue<TKey, TValue> Split()
        {
            var count = Values.Count - 1;

            count /= 2;

            var first = Values.Take(count);
            var second = Values.Skip(count + 1);

            var median = Values[count];

            var left = BTreeNode<TKey, TValue>.Create(pages, arity);
            var right = BTreeNode<TKey, TValue>.Create(pages, arity);

            foreach (var v in first)
            {
                left.Add(v.Key, v.Value).LesserNode = v.LesserNode;
                v.Unset(false);
            }

            foreach (var v in second)
            {
                right.Add(v.Key, v.Value).LesserNode = v.LesserNode;
                v.Unset(false);
            }

            var last_left = left.Values.SkipWhile(l => l.IsAvailable).First();
            last_left.Unset();
            last_left.LesserNode = median.LesserNode;

            var last_right = right.Values.SkipWhile(r => r.IsAvailable).First();

            // left.MigrateChildren();
            // right.MigrateChildren();

            var mk = median.Key;
            var mv = median.Value;

            Values[0].Unset(false);
            Values[0].Set(mk, mv);
            Values[0].LesserNode = left;

            Values[1].Unset(false);
            Values[1].LesserNode = right;

            foreach (var v in values.Skip(2))
                v.Unset(false);

            // MigrateChildren();

            return Values[0];
        }

        public IBTreeValue<TKey, TValue> MigrateUp()
        {
            if (parent == null)
                throw new InvalidOperationException("Root node can't migrate up");

            var i = parent.ArityIndex;

            if (i >= arity)
                return parent; // nothing to do.

            if (parent.IsAvailable)
                throw new InvalidOperationException("Only the greatest child of a node may be migrated");

            Console.WriteLine($"Migrating {Key}");

            var n = 0;
            for (; i < arity; i++)
            {
                parent.TreeNode.Values[i].Unset(false);
                parent.TreeNode.Values[i].Set(Values[n].Key, Values[n].Value);
                parent.TreeNode.Values[i].LesserNode = Values[n].LesserNode;

                if (!Values[n].IsAvailable)
                {
                    i++;
                    break; // return parent.TreeNode.Values[i];
                }

                n++;
            }

            var j = 0;
            for (; j < arity; j++)
            {
                if (j < arity)
                    Move(j, j + 1);
                else
                    Values[j].Unset(false);
            }

            i--;
            parent.TreeNode.Values[i].Unset(false);
            parent.TreeNode.Values[i].LesserNode = this;
            return parent.TreeNode.Values[i];
        }

        public void MigrateChildren()
        {
            foreach (var child in Values)
            {
                if (!child.IsAvailable)
                    child.LesserNode?.MigrateUp();
            }
        }

        public void Delete()
        {
            for (var i = 0; i <= arity; i++)
            {
                Values[i].Unset();

                var idx = index + (ulong) i;

                var page = pages.GetPageFor(idx);
                page[idx] = new BTreeNode<TKey, TValue>.Data {
                    IsAvailable = false,
                    LesserIndex = 0
                };

                page.Clear(idx);
            }
        }

        private void Move(int idxA, int idxB)
        {
            var a = Values[idxA];
            var b = Values[idxB];

            a.Unset(false);
            if (b.IsAvailable)
                a.Set(b.Key, b.Value);

            a.LesserNode = b.LesserNode;
        }

        public static BTreeNode<TKey, TValue> Create(IPageAccessor<BTreeNode<TKey, TValue>.Data> pages, int arity)
        {
            var index = pages.FindFreeIndex();

            if (index == 0)
            {
                pages.GetPage(0).Set(0);
                pages[0] = new BTreeNode<TKey, TValue>.Data {
                    IsAvailable = false,
                    LesserIndex = 0
                };

                index = pages.FindFreeIndex();
            }

            // if (((index - 1) % (ulong)(arity + 1)) != 0)
            //     throw new CongruityException("Data must be aligned by arity, but that is not the case.");

            for (var i = 0; i <= arity; i++)
            {
                var idx = index + (ulong) i;
                if (pages.GetPageFor(idx).IsIndexUsed(idx))
                    throw new CongruityException("Data must be aligned by arity, but that is not the case.");
            }

            for (var i = 0; i <= arity; i++)
            {
                var idx = index + (ulong) i;
                var data = pages.GetReference(idx);

                data.Set();
                data.Value.IsAvailable = false;
                data.Value.LesserIndex = 0;
            }

            return new BTreeNode<TKey, TValue>(pages, null, arity, index);
        }

        public override int GetHashCode()
        {
            return index.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is BTreeNode<TKey, TValue> node ? node.index == index : false;
        }

        public class Value : IBTreeValue<TKey, TValue>
        {
            private ulong index;

            private BTreeNode<TKey, TValue> node, lesser;

            private IPageItemRef<Data> dataRef;

            public Value(BTreeNode<TKey, TValue> node, ulong index)
            {
                this.node = node;
                this.index = index;
                dataRef = node.pages.GetReference(index);
            }

            public int ArityIndex => (int) (index - node.index);

            public IBTreeNode<TKey, TValue> TreeNode => node;

            public bool IsAvailable => dataRef.IsAvailable && dataRef.Value.IsAvailable;

            public TKey Key => dataRef.Value.Key;

            public IBTreeNode<TKey, TValue> LesserNode
            {
                get => Reference.GetOrActivate(ref lesser, () => {
                    ulong idx;
                    return !IsAvailable || (idx = dataRef.Value.LesserIndex) == 0
                        ? null
                        : new BTreeNode<TKey, TValue>(node.pages, this, node.arity, idx);
                });

                set
                {
                    LesserNode?.Delete();
                    // if (LesserNode != null)
                    //     LesserNode.Owner = null;

                    var node = (BTreeNode<TKey, TValue>) value;

                    ref var data = ref dataRef.Value;

                    data.LesserIndex = node?.index ?? 0;

                    if (node != null)
                        node.Owner = this;

                    lesser = node;
                }
            }

            TValue IBTreeValue<TKey, TValue>.Value => dataRef.Value.Value;

            public void Set(TKey key, TValue value)
            {
                dataRef.Set();
                ref var data = ref dataRef.Value;

                data.IsAvailable = true;
                data.Key = key;
                data.Value = value;
            }

            public void Unset(bool delete = true)
            {
                if (delete && LesserNode is var n)
                    n?.Delete();

                ref var data = ref dataRef.Value;
                data.IsAvailable = false;
                data.LesserIndex = 0;

                lesser = null;
            }
        }

        public struct Data
        {
            public bool IsAvailable;

            public TKey Key;

            public TValue Value;

            public ulong LesserIndex;
        }
    }
}
