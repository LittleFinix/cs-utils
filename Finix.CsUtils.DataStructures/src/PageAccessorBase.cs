using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections;

namespace Finix.CsUtils
{
    public abstract class PageAccessorBase<T> : IPageAccessor<T> where T : unmanaged
    {
        public abstract IEnumerable<KeyValuePair<ulong, IPage<T>>> Pages { get; }

        public abstract IPage<T> GetPage(ulong pageIndex);

        public abstract ulong CalculatePageIndex(ulong index);

        public virtual IPage<T> GetPageFor(ulong index)
        {
            return GetPage(CalculatePageIndex(index));
        }

        public virtual bool ContainsIndex(ulong index)
        {
            var page = GetPageFor(index);
            return page.IsIndexAvailable(index);
        }

        public virtual ulong FindFreeIndex()
        {
            ulong last = 0;
            foreach (var kv in Pages)
            {
                if (kv.Value.FindFirstFreeIndex(out var index))
                {
                    return index;
                }

                last = Math.Max(last, kv.Value.Offset + kv.Value.Size);
            }

            return last;
        }

        public virtual bool TryGetValue(ulong index, out T value)
        {
            var page = GetPageFor(index);

            value = page[index];
            return page.IsIndexAvailable(index);
        }

        public virtual T this[ulong index]
        {
            get
            {
                var page = GetPageFor(index);

                if (!page.IsIndexAvailable(index))
                    throw new KeyNotFoundException($"Index {index} is not available");

                return page[index];
            }

            set
            {
                var page = GetPageFor(index);
                page.Set(index);

                page[index] = value;
            }
        }
    }
}
