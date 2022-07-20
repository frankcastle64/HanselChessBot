using System.Numerics;
using System.Runtime.CompilerServices;

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
