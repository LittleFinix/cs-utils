using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Finix.CsUtils.DataStructures
{
    public unsafe class MemoryPageItemRef<T> : IPageItemRef<T> where T : unmanaged
    {
        T* item;

        byte* set;
        byte set_mask;

        public MemoryPageItemRef(T* item, byte* set, ulong index)
        {
            this.item = item;
            this.set = set;
            set_mask = (byte) (1 << (int) (index % 8));
        }

        public bool IsAvailable => (*set & set_mask) != 0;

        public ulong Index { get; }

        public ref T Value => ref MemoryMarshal.GetReference(new Span<T>(item, 1));

        public void Clear()
        {
            *set &= (byte) ~set_mask;
        }

        public void Set()
        {
            *set |= set_mask;
        }
    }
}
