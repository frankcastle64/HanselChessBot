namespace HanselChessBOT.ConsoleApp.MagicGeneration
{
    public static class MagicSetup
    {

        public static ulong[] Blockers = new ulong[4096];
        public static ulong[] AttackMask = new ulong[4096];
        public static ulong[] Used = new ulong[4096];

        public static ulong ComputeRookAttackInnerMask(int sq)
        {
            ulong attackMask = 0UL;
            int currentRank = sq / 8;
            int currentFile = sq % 8;

            // North direction
            for (int currentRankIndex = currentRank + 1; currentRankIndex < 7; currentRankIndex++)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFile);
            }

            // South direction
            for (int currentRankIndex = currentRank - 1; currentRankIndex > 0; currentRankIndex--)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFile);
            }


            // East direction
            for (int currentFileIndex = currentFile + 1; currentFileIndex < 7; currentFileIndex++)
            {
                attackMask |= 1UL << (8 * currentRank + currentFileIndex);
            }

            // South direction
            for (int currentFileIndex = currentFile - 1; currentFileIndex > 0; currentFileIndex--)
            {
                attackMask |= 1UL << (8 * currentRank + currentFileIndex);
            }

            return attackMask;
        }
        public static ulong ComputeBishopAttackInnerMask(int sq)
        {
            ulong attackMask = 0UL;
            int currentRank = sq / 8;
            int currentFile = sq % 8;

            // North-East direction
            for (int currentRankIndex = currentRank + 1, currentFileIndex = currentFile + 1; currentRankIndex < 7 && currentFileIndex < 7; currentFileIndex++, currentRankIndex++)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFileIndex);
            }

            // South-West direction
            for (int currentRankIndex = currentRank - 1, currentFileIndex = currentFile - 1; currentRankIndex > 0 && currentFileIndex > 0; currentFileIndex--, currentRankIndex--)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFileIndex);
            }

            // North-West direction
            for (int currentRankIndex = currentRank + 1, currentFileIndex = currentFile - 1; currentRankIndex < 7 && currentFileIndex > 0; currentFileIndex--, currentRankIndex++)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFileIndex);
            }

            // South-East direction
            for (int currentRankIndex = currentRank - 1, currentFileIndex = currentFile + 1; currentRankIndex > 0 && currentFileIndex < 7; currentFileIndex++, currentRankIndex--)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFileIndex);
            }

            return attackMask;
        }

        public static ulong ComputeRookAttackOnOccupancy(int sq, ulong blockers)
        {
            ulong attackMask = 0UL;
            int currentRank = sq / 8;
            int currentFile = sq % 8;


            // North direction
            for (int currentRankIndex = currentRank + 1; currentRankIndex < 8; currentRankIndex++)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFile);
                if ((blockers & (1UL << (8 * currentRankIndex + currentFile))) != 0)
                {
                    break;
                }
            }

            // South direction
            for (int currentRankIndex = currentRank - 1; currentRankIndex >= 0; currentRankIndex--)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFile);
                if ((blockers & (1UL << (8 * currentRankIndex + currentFile))) != 0)
                {
                    break;
                }
            }


            // East direction
            for (int currentFileIndex = currentFile + 1; currentFileIndex < 8; currentFileIndex++)
            {
                attackMask |= 1UL << (8 * currentRank + currentFileIndex);
                if ((blockers & (1UL << (8 * currentRank + currentFileIndex))) != 0)
                {
                    break;
                }
            }

            // South direction
            for (int currentFileIndex = currentFile - 1; currentFileIndex >= 0; currentFileIndex--)
            {
                attackMask |= 1UL << (8 * currentRank + currentFileIndex);
                if ((blockers & (1UL << (8 * currentRank + currentFileIndex))) != 0)
                {
                    break;
                }
            }

            return attackMask;
        }

        public static ulong ComputeBishopAttackOnOccupancy(int sq, ulong blockers)
        {
            ulong attackMask = 0UL;
            int currentRank = sq / 8;
            int currentFile = sq % 8;

            // North-East direction
            for (int currentRankIndex = currentRank + 1, currentFileIndex = currentFile + 1; currentRankIndex < 8 && currentFileIndex < 8; currentFileIndex++, currentRankIndex++)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFileIndex);
                if ((blockers & (1UL << (8 * currentRankIndex + currentFileIndex))) != 0)
                {
                    break;
                }
            }

            // South-West direction
            for (int currentRankIndex = currentRank - 1, currentFileIndex = currentFile - 1; currentRankIndex >= 0 && currentFileIndex >= 0; currentFileIndex--, currentRankIndex--)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFileIndex);
                if ((blockers & (1UL << (8 * currentRankIndex + currentFileIndex))) != 0)
                {
                    break;
                }
            }

            // North-West direction
            for (int currentRankIndex = currentRank + 1, currentFileIndex = currentFile - 1; currentRankIndex < 8 && currentFileIndex >= 0; currentFileIndex--, currentRankIndex++)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFileIndex);
                if ((blockers & (1UL << (8 * currentRankIndex + currentFileIndex))) != 0)
                {
                    break;
                }
            }

            // South-East direction
            for (int currentRankIndex = currentRank - 1, currentFileIndex = currentFile + 1; currentRankIndex >= 0 && currentFileIndex < 8; currentFileIndex++, currentRankIndex--)
            {
                attackMask |= 1UL << (8 * currentRankIndex + currentFileIndex);
                if ((blockers & (1UL << (8 * currentRankIndex + currentFileIndex))) != 0)
                {
                    break;
                }
            }

            return attackMask;
        }


        public static ulong ComputeMagics(int sq, int numberOfBitsRequiredByPiece, bool isRook)
        {
            // This code is borrowed from chess programming wiki (Tord's implementation)
            ulong mask = isRook ? ComputeRookAttackInnerMask(sq) : ComputeBishopAttackInnerMask(sq);
            int setBitsInMask = (int)Utilities.CountSetBits(mask);
            int j, fail;

            for (int i = 0; i < (1 << setBitsInMask); i++)
            {
                Blockers[i] = CreateBlockerOccupancy(i, setBitsInMask, mask);
                AttackMask[i] = isRook ? ComputeRookAttackOnOccupancy(sq, Blockers[i]) : ComputeBishopAttackOnOccupancy(sq, Blockers[i]);
            }

            for (int k = 0; k < 100000000; k++)
            {
                fail = 0;

                ulong magic = GenerateRandomNumbersFewBits();

                if (Utilities.CountSetBits((mask * magic) & 0xFF00000000000000UL) < 6) continue;

                for (int i = 0; i < 4096; i++)
                {
                    Used[i] = 0UL;
                }

                for (int i = 0; i < (1 << setBitsInMask); i++)
                {
                    j = transform(Blockers[i], magic, numberOfBitsRequiredByPiece);
                    if (Used[j] == 0UL) Used[j] = AttackMask[i];
                    else if (Used[j] != AttackMask[i])
                    {
                        fail = 1;
                        break;
                    }
                }

                if (fail != 1)
                {
                    return magic;
                }
            }
            Console.WriteLine("***Failed***\n");
            return 0UL;

        }

        private static int transform(ulong blockers, ulong magic, int numberOfBitsRequiredByPiece)
        {
            return (int)((blockers * magic) >> (64 - numberOfBitsRequiredByPiece));
        }
        public static ulong CreateBlockerOccupancy(int index, int bits, ulong m)
        {
            int i, j;
            ulong result = 0UL;
            for (i = 0; i < bits; i++)
            {
                j = Utilities.BitScanForward(m);// pop_1st_bit(&m);
                m &= m - 1;
                if ((index & (1 << i)) != 0) result |= (1UL << j);
            }
            return result;
        }

        private static ulong GenerateRandomNumbers()
        {
            Random random = new Random();
            ulong u1, u2, u3, u4;
            u1 = (ulong)(random.Next()) & 0xFFFF; u2 = (ulong)(random.Next()) & 0xFFFF;
            u3 = (ulong)(random.Next()) & 0xFFFF; u4 = (ulong)(random.Next()) & 0xFFFF;
            return u1 | (u2 << 16) | (u3 << 32) | (u4 << 48);

        }

        private static ulong GenerateRandomNumbersFewBits()
        {
            return GenerateRandomNumbers() & GenerateRandomNumbers() & GenerateRandomNumbers();
        }

    }
}
