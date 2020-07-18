using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using Finix.CsUtils.DataStructures;

namespace Finix.CsUtils
{
    public partial class MemoryPageAccessor<T> : PageAccessorBase<T> where T : unmanaged
    {
        public unsafe class Page : IPage<T>, IDisposable
        {
            private byte* buffer;
            private ulong buffer_length;
            private ulong data_offset;

            private MemoryPageAccessor<T> memoryPageAccessor;

            public Page(MemoryPageAccessor<T> mpa, ulong offset, ulong size)
            {
                memoryPageAccessor = mpa;
                Offset = offset;
                Size = size;

                mpa.lowestAvailablePage ??= this;
            }

            ~Page()
            {
                Dispose();
            }

            public bool IsAvailable => buffer != null;

            public ulong Offset { get; }

            public ulong Size { get; }

            public ulong AllocatedBytes => buffer_length;

            public virtual bool Full { get; private set; } = false;

            public virtual bool Empty
            {
                get
                {
                    for (ulong i = 0; i < data_offset; i++)
                    {
                        if (buffer[i] != 0)
                            return false;
                    }

                    return true;
                }
            }

            public virtual bool IsIndexUsed(ulong index)
            {
                unchecked
                {
                    index -= Offset;
                    return (buffer[index / 8] & (1 << (int) (index % 8))) != 0;
                }
            }

            public virtual bool Load()
            {
                if (IsAvailable)
                    return false;

                // Console.WriteLine($"Loading page at offset {Offset}");

                data_offset = Size / 8;
                buffer_length = Size * (ulong) sizeof(T) + data_offset;

                buffer = (byte*) Marshal.AllocHGlobal((int) buffer_length);

                new Span<byte>(buffer, (int) data_offset).Fill(0);
                GC.ReRegisterForFinalize(this);

                return true;
            }

            public virtual bool Unload()
            {
                if (!IsAvailable)
                    return false;

                Marshal.FreeHGlobal((IntPtr) buffer);
                buffer = null;
                GC.SuppressFinalize(this);

                return true;
            }

            public void Dispose()
            {
                Unload();
            }

            public void Set(ulong index)
            {
                unchecked
                {
                    index -= Offset;
                    buffer[index / 8] |= (byte) (1 << (int) (index % 8));
                }
            }

            public void Clear(ulong index)
            {
                unchecked
                {
                    Full = false;
                    index -= Offset;
                    buffer[index / 8] &= (byte) ~(1 << (int) (index % 8));

                    if (Empty)
                        Unload();

                    var lowestPage = memoryPageAccessor.lowestAvailablePage ??= this;

                    if (lowestPage.Offset > Offset)
                        memoryPageAccessor.lowestAvailablePage = this;
                }
            }

            public virtual bool FindFirstFreeIndex(out ulong index)
            {
                unchecked
                {
                    index = 0;
                    if (Full)
                        return false;

                    ulong i, b;

                    for (i = 0; i < data_offset; i++)
                    {
                        if (buffer[i] != 0xFF)
                            break;
                    }

                    if (i == data_offset)
                    {
                        Full = true;
                        return false;
                    }

                    for (b = 0; b < 8; b++)
                    {
                        if (((buffer[i] >> (byte) b) & 1) == 0)
                            break;
                    }

                    index = i * 8UL + b + Offset;
                    return true;
                }
            }

            public virtual IPageItemRef<T> GetReference(ulong index)
            {
                var idx = index - Offset;
                return new MemoryPageItemRef<T>(GetPointer(index), buffer + idx / 8, index);
            }

            protected virtual T* GetPointer(ulong index)
            {
                unchecked
                {
                    index -= Offset;
                    return (T*) (buffer + data_offset + index * (ulong) sizeof(T));
                }
            }

            public T this[ulong index]
            {
                get => GetPointer(index)[0];
                set => GetPointer(index)[0] = value;
            }
        }
    }
}
