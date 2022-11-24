namespace HanselChessBOT.ConsoleApp.MagicGeneration
{
    public static class MagicNumbers
    {
        public static ulong[,] BishopOccupancyAttacks = new ulong[64, 512];
        public static ulong[,] RookOccupancyAttacks = new ulong[64, 4096];

        public static ulong[][] RookAttacks = new ulong[64][];
        public static ulong[][] BishopAttacks = new ulong[64][];

        public static ulong[] RookAttacksInnerSixBits = new ulong[64];
        public static ulong[] BishopAttacksInnerSixBits = new ulong[64];

        public static readonly int[] BitsRequiredByRook = new int[]
       {
            52, 53, 53, 53, 53, 53, 53, 52,
            53, 54, 54, 54, 54, 54, 54, 53,
            53, 54, 54, 54, 54, 54, 54, 53,
            53, 54, 54, 54, 54, 54, 54, 53,
            53, 54, 54, 54, 54, 54, 54, 53,
            53, 54, 54, 54, 54, 54, 54, 53,
            53, 54, 54, 54, 54, 54, 54, 53,
            52, 53, 53, 53, 53, 53, 53, 52
      };

        public static readonly int[] BitsRequiredByBishop = new int[]
        {
            58, 59, 59, 59, 59, 59, 59, 58,
            59, 59, 59, 59, 59, 59, 59, 59,
            59, 59, 57, 57, 57, 57, 59, 59,
            59, 59, 57, 55, 55, 57, 59, 59,
            59, 59, 57, 55, 55, 57, 59, 59,
            59, 59, 57, 57, 57, 57, 59, 59,
            59, 59, 59, 59, 59, 59, 59, 59,
            58, 59, 59, 59, 59, 59, 59, 58,
        };

        public static readonly ulong[] RookMagicNumbers = new ulong[]
        {
            0x6200102200410081UL,0x1c00010012001c2UL,0x900200042890010UL,0x1280080204100080UL,0x4100021100080004UL,0x200104481080200UL,0x1003a8600310004UL,0x200004089040022UL,
            0x118800440022280UL,0x1f004000210c80UL,0x80801000802001UL,0x4002800800100080UL,0x1000801000410UL,0x802001002000804UL,0x500040e000500UL,0x200040a208841UL,
            0x828002400220UL,0x1008888020004000UL,0x880828020001008UL,0x5410008008001080UL,0x8004004020040UL,0x100808004000200UL,0x82040008100102UL,0x20000a40041UL,
            0x840400480088020UL,0x50200040c0003000UL,0x104100200102UL,0x40120200200840UL,0x3108000980040080UL,0x40080800200UL,0x4001900402020008UL,0x30010200249044UL,
            0x20804000800020UL,0x3180201004400044UL,0x100080802000UL,0x200081c802801000UL,0x4000080080800400UL,0x2001812007084UL,0x2114ca0104001018UL,0x90103204200108cUL,
            0x1000804000208000UL,0x8024442010004001UL,0x45a001082220040UL,0x100104090020UL,0x4859008800050010UL,0x20004008080UL,0x82000104820008UL,0x2400040090420001UL,
            0x5800104334100UL,0x104200280400b80UL,0x82004010208200UL,0x800421108220200UL,0x1104000408008080UL,0x2004020080040080UL,0x200214208100400UL,0x9000004100840200UL,
            0x800100182041UL,0x102001825008042UL,0x4401220800a02UL,0x8cb0000410082101UL,0x1502010420100802UL,0x11000802040001UL,0x220a000840810402UL,0x9a0004144080210aUL

        };

        public static readonly ulong[] BishopMagicNumbers = new ulong[]
        {
            0x20142142002202UL,0x8002102200810000UL,0x1022088b02040041UL,0xc2080a0828000040UL,0x2061104000082000UL,0x4022080249000000UL,0x909009010080048UL,0x8004108094202020UL,
            0x840840040400c20eUL,0x80274005200UL,0xc4004a902020005UL,0x1015310400880200UL,0x1001021210006010UL,0x2110023010080020UL,0x202141201104900UL,0x10010880842000UL,
            0x41200424041400UL,0x1098a082038702UL,0x40205404c024082UL,0x912802024002UL,0x30102202100020UL,0x4922002303088200UL,0x2008048240450UL,0x84010a0049180100UL,
            0x802210040480240UL,0xc90500004410a3bUL,0x1c40008042404UL,0x2082004040080UL,0x8420050042008200UL,0x200808001006001UL,0x28084000820810UL,0x810012092082UL,
            0x4038480842412200UL,0x21022107d0828UL,0x229008080221UL,0x20a004040840100UL,0x60021010040040UL,0x20a0b8200190050UL,0x1510040082006204UL,0x8008620010920UL,
            0x10108280400c100UL,0x402401084a041901UL,0x180220030024201UL,0x80014202818802UL,0x1000280104000840UL,0x6120181000880043UL,0x2020010400961110UL,0x410108900540500UL,
            0xa00890c820100000UL,0x14484040202840aUL,0x220a091100ecUL,0x6a00000084041000UL,0x20820821010010UL,0x2020080a083010UL,0x5812105020808018UL,0x40404004a0024UL,
            0x8800820101014000UL,0x8044a08404620200UL,0x100022042109000UL,0x82004020208800UL,0x8c35820010421a00UL,0x9004010410429200UL,0x8000440802081220UL,0x4840088010102UL
        };

        private static void GenerateInnerSixBits()
        {
            for (int sq = Square.a1; sq <= Square.h8; sq++)
            {
                RookAttacksInnerSixBits[sq] = MagicSetup.ComputeRookAttackInnerMask(sq);
                BishopAttacksInnerSixBits[sq] = MagicSetup.ComputeBishopAttackInnerMask(sq);
            }
        }

        private static void CreateBlockersMatrix()
        {
            for (int sq = Square.a1; sq <= Square.h8; sq++)
            {
                int setBitsInMaskForRook = 64 - BitsRequiredByRook[sq];
                ulong maskRook = RookAttacksInnerSixBits[sq];
                for (int i = 0; i < (1 << setBitsInMaskForRook); i++)
                {
                    RookOccupancyAttacks[sq, i] = MagicSetup.CreateBlockerOccupancy(i, setBitsInMaskForRook, maskRook);
                }


                int setBitsInMaskForBishop = 64 - BitsRequiredByBishop[sq];
                ulong maskBishop = BishopAttacksInnerSixBits[sq];
                for (int i = 0; i < (1 << setBitsInMaskForBishop); i++)
                {
                    BishopOccupancyAttacks[sq, i] = MagicSetup.CreateBlockerOccupancy(i, setBitsInMaskForRook, maskBishop);
                }
            }
        }

        private static void ResetAttacks()
        {
            for (int sq = Square.a1; sq <= Square.h8; sq++)
            {
                int setBitsInMaskForRook = BitsRequiredByRook[sq];
                RookAttacks[sq] = new ulong[4096];
                
                for (int i = 0; i < 1 << (64 - setBitsInMaskForRook); i++)
                {
                    ulong occupancy = RookOccupancyAttacks[sq, i];
                    int magic_index = (int)((occupancy * RookMagicNumbers[sq]) >> (setBitsInMaskForRook));
                    RookAttacks[sq][magic_index] = MagicSetup.ComputeRookAttackOnOccupancy(sq, occupancy);
                }


                int setBitsInMaskForBishop = BitsRequiredByBishop[sq];
                BishopAttacks[sq] = new ulong[512];
                for (int i = 0; i < 1 << (64 - setBitsInMaskForBishop); i++)
                {
                    ulong occupancy = BishopOccupancyAttacks[sq, i];
                    int magic_index = (int)((occupancy * BishopMagicNumbers[sq]) >> (setBitsInMaskForBishop));
                    BishopAttacks[sq][magic_index] = MagicSetup.ComputeBishopAttackOnOccupancy(sq, occupancy);
                }
            }
        }

        public static void InitMagics()
        {
            GenerateInnerSixBits();
            CreateBlockersMatrix();
            ResetAttacks();
        }
    }
}
