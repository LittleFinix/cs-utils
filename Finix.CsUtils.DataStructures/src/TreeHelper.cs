using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public static class TreeHelper
    {
        public static ulong CalculateParentIndex(ulong index, int arity = 2)
        {
            return (index - 1) / (ulong) arity;
        }

        public static ulong CalculateChildIndex(ulong index, uint child, int arity = 2)
        {
            return (ulong) arity * index + child + 1;
        }

        public static ulong CalculateLeftChildIndex(ulong index, int arity = 2)
        {
            return CalculateChildIndex(index, 0, arity);
        }

        public static ulong CalculateRightChildIndex(ulong index, int arity = 2)
        {
            return CalculateChildIndex(index, 1, arity);
        }

        public static ulong CalculateDepthForIndex(ulong index, int arity = 2)
        {
            if (index == 0)
                return 0;

            return (ulong) Math.Log(index, arity);
        }
    }
}
