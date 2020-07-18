using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public partial class MemoryPageAccessor<T> : PageAccessorBase<T>, IDisposable where T : unmanaged
    {
        private IPage<T>[][] pages = new IPage<T>[1000][];

        private IPage<T> lowestAvailablePage = null;

        private ulong lowestUnusedPageIndex = 0;

        private ulong highestUsedPageIndex = 0;

        public override ulong FindFreeIndex()
        {
            return base.FindFreeIndex();

            if (lowestAvailablePage == null)
                return 0;

            var freeidx = lowestUnusedPageIndex * PageSize;
            if (lowestAvailablePage.Offset < lowestUnusedPageIndex && lowestAvailablePage.FindFirstFreeIndex(out var index))
                return index;

            return freeidx;
        }

        public MemoryPageAccessor(ulong pageSize)
        {
            PageSize = pageSize;

            for (int i = 0; i < pages.Length; i++)
            {
                pages[i] = new IPage<T>[10];
            }
        }

        public override IEnumerable<KeyValuePair<ulong, IPage<T>>> Pages => pages.SelectMany(
            (list, index1) => list
                .Select((page, index2) => new KeyValuePair<ulong, IPage<T>>((ulong) (index1 + index2 * pages.Length), page))
                .Where(kv => kv.Value != null)
        );

        public ulong PageSize { get; }

        public override ulong CalculatePageIndex(ulong index)
        {
            return index / PageSize;
        }

        public void Dispose()
        {
            foreach (var page in Pages)
                page.Value.Unload();
        }

        public override IPage<T> GetPage(ulong pageIndex)
        {
            int bucket = (int)pageIndex % pages.Length;
            int index = (int)pageIndex / pages.Length;

            if (pages[bucket].Length <= index)
                Array.Resize(ref pages[bucket], index + 1);

            return pages[bucket][index] ?? (pages[bucket][index] = CreatePage(pageIndex));
        }

        protected virtual IPage<T> CreatePage(ulong pageIndex)
        {
            if (pageIndex > highestUsedPageIndex)
            {
                highestUsedPageIndex = pageIndex;
            }

            if (lowestUnusedPageIndex == pageIndex)
            {
                lowestUnusedPageIndex = highestUsedPageIndex + 1;
            }

            var page = new Page(this, pageIndex * PageSize, PageSize);
            page.Load();

            return page;
        }
    }
}
