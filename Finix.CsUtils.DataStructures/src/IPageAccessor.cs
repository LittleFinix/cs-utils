using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public interface IPageAccessor<T>
    {
        IEnumerable<KeyValuePair<ulong, IPage<T>>> Pages { get; }

        IPage<T> GetPage(ulong pageIndex);

        IPage<T> GetPageFor(ulong index);

        ulong CalculatePageIndex(ulong index);

        bool TryGetValue(ulong index, out T value);

        ulong FindFreeIndex();

        T this[ulong index] { get; set; }
    }
}
