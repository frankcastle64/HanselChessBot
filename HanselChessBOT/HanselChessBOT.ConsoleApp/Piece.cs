using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanselChessBOT.ConsoleApp
{
    public static class Piece
    {
        public const int PieceColor_WHITE = 0;
        public const int PieceColor_BLACK = 1;
        public const int PieceColor_BOTH = 2;

        public const int NO_PIECE = 0;
        public const int WP = 1;
        public const int WN = 2;
        public const int WB = 3;
        public const int WR = 4;
        public const int WQ = 5;
        public const int WK = 6;

        public const int BP = 7;
        public const int BN = 8;
        public const int BB = 9;
        public const int BR = 10;
        public const int BQ = 11;
        public const int BK = 12;

        public const int PieceType_NO_Piece = 0;
        public const int PieceType_Pawn = 1;
        public const int PieceType_Knight = 2;
        public const int PieceType_Bishop = 3;
        public const int PieceType_Rook = 4;
        public const int PieceType_Queen = 5;
        public const int PieceType_King = 6;

        public const string PieceStr = ".PNBRQKpnbrqk";

        public static int[] Pieces = new int[] { NO_PIECE, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK };

        public static ulong[] Pieces_BB = new ulong[Pieces.Length];

    }
}
