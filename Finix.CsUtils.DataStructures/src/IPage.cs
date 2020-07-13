using System;

namespace Finix.CsUtils
{
    public interface IPage<T>
    {
        bool IsAvailable { get; }

        ulong Offset { get; }

        ulong Size { get; }

        ulong AllocatedBytes { get; }

        bool IsIndexAvailable(ulong index);

        bool FindFirstFreeIndex(out ulong index);

        bool Load();

        bool Unload();

        ref T GetReference(ulong index);

        void Set(ulong index);

        void Clear(ulong index);

        T this[ulong index] { get; set; }
    }
}
