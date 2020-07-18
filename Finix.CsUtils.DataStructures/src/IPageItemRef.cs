using System;
using System.Collections.Generic;
using System.Text;

namespace Finix.CsUtils
{
    public interface IPageItemRef<T>
    {
        public bool IsAvailable { get; }

        public ulong Index { get; }

        public ref T Value { get; }

        void Set();

        void Clear();
    }
}
