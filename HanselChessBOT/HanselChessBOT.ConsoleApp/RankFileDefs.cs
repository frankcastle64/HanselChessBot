namespace HanselChessBOT.ConsoleApp
{
    public static class RankFileDefs
    {
        public const string FilesStr = "abcdefgh";
        public const string RanksStr = "12345678";

        public const ulong File_A_BB = 0x0101010101010101UL;
        public const ulong File_B_BB = 0x0202020202020202UL;
        public const ulong File_C_BB = 0x0404040404040404UL;
        public const ulong File_D_BB = 0x0808080808080808UL;
        public const ulong File_E_BB = 0x1010101010101010UL;
        public const ulong File_F_BB = 0x2020202020202020UL;
        public const ulong File_G_BB = 0x4040404040404040UL;
        public const ulong File_H_BB = 0x8080808080808080UL;

        public const ulong Rank_1_BB = 0xFFUL;
        public const ulong Rank_2_BB = 0xFF00UL;
        public const ulong Rank_3_BB = 0xFF0000UL;
        public const ulong Rank_4_BB = 0xFF000000UL;
        public const ulong Rank_5_BB = 0xFF00000000UL;
        public const ulong Rank_6_BB = 0xFF0000000000UL;
        public const ulong Rank_7_BB = 0xFF000000000000UL;
        public const ulong Rank_8_BB = 0xFF00000000000000UL;

        public static ulong[] Files_BB = new ulong[] { File_A_BB, File_B_BB, File_C_BB, File_D_BB, File_E_BB, File_F_BB, File_G_BB, File_H_BB };
        public static ulong[] Ranks_BB = new ulong[] { Rank_1_BB, Rank_2_BB, Rank_3_BB, Rank_4_BB, Rank_5_BB, Rank_6_BB, Rank_7_BB, Rank_8_BB };

        public const int File_A = 0;
        public const int File_H = 7;

        public const int Rank_8 = 7;
        public const int Rank_1 = 0;

    }
}
