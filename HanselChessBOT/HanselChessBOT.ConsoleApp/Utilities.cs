using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace HanselChessBOT.ConsoleApp
{
    public static class Utilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BitScanForward(ulong bb)
        {
            return BitOperations.TrailingZeroCount(bb);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountSetBits(ulong bb)
        {
            return BitOperations.PopCount(bb);
        }
    }
}
