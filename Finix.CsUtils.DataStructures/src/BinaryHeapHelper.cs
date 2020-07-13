using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public static class BinaryHeapHelper
    {
        public static ulong CalculateParentIndex(ulong index)
        {
            return (index - 1) / 2;
        }

        public static ulong CalculateLeftChildIndex(ulong index)
        {
            return 2 * index + 1;
        }

        public static ulong CalculateRightChildIndex(ulong index)
        {
            return 2 * index + 2;
        }

        public static ulong CalculateDepthForIndex(ulong index)
        {
            if (index == 0)
                throw new NotFiniteNumberException();

            return (ulong) Math.Log2(index);
        }
    }
}
