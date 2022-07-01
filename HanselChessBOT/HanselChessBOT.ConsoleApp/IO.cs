using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanselChessBOT.ConsoleApp
{
    public static class IO
    {
        public static void SetPosition(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            FenManager.SetPositionFromFen(fen);
        }

        public static void DisplayBoard()
        {
            for (int rank = RankFileDefs.Rank_8; rank >= RankFileDefs.Rank_1; rank--)
            {
                for (int file = RankFileDefs.File_A; file <= RankFileDefs.File_H; file++)
                {
                    int sq = 8 * rank + file;

                    Console.Write(" " + Piece.PieceStr[BoardDefs.Board[sq]] + " ");
                }

                Console.WriteLine();
            }
        }

        public static void DisplayInformation()
        {
            for (int piece = Piece.WP; piece <= Piece.BK; piece++)
            {
                Console.WriteLine(" Bitboards " + Piece.PieceStr[piece] + " : " + Piece.Pieces_BB[piece]);
            }
        }


        public static void PrintMove(ulong move)
        {
            int sqFrom = (int)(move & MoveEncodingMasks.SQ_FROM_MASK);
            int sqTo = (int)((move >> MoveEncodingMasks.SQ_TO_SHIFT) & MoveEncodingMasks.SQ_TO_MASK);
            int pieceFrom = (int)((move >> MoveEncodingMasks.PIECE_FROM_SHIFT) & MoveEncodingMasks.PIECE_FROM_MASK);
            int pieceTo = (int)((move >> MoveEncodingMasks.PIECE_TO_SHIFT) & MoveEncodingMasks.PIECE_TO_MASK);
            int moveType = (int)((move >> MoveEncodingMasks.TYPE_OF_MOVE_SHIFT) & MoveEncodingMasks.MOVE_TYPE_MASK);
            int promotedPiece = (int)((move >> MoveEncodingMasks.PROMOTED_PIECE_SHIFT) & MoveEncodingMasks.PROMOTED_PIECE_MASK);

            PrintMove(sqFrom, sqTo);
        }

        public static void PrintMoveWithNodes(ulong move, ulong nodes)
        {
            int sqFrom = (int)(move & MoveEncodingMasks.SQ_FROM_MASK);
            int sqTo = (int)((move >> MoveEncodingMasks.SQ_TO_SHIFT) & MoveEncodingMasks.SQ_TO_MASK);
            int pieceFrom = (int)((move >> MoveEncodingMasks.PIECE_FROM_SHIFT) & MoveEncodingMasks.PIECE_FROM_MASK);
            int pieceTo = (int)((move >> MoveEncodingMasks.PIECE_TO_SHIFT) & MoveEncodingMasks.PIECE_TO_MASK);
            int moveType = (int)((move >> MoveEncodingMasks.TYPE_OF_MOVE_SHIFT) & MoveEncodingMasks.MOVE_TYPE_MASK);
            int promotedPiece = (int)((move >> MoveEncodingMasks.PROMOTED_PIECE_SHIFT) & MoveEncodingMasks.PROMOTED_PIECE_MASK);

            if (promotedPiece == 0)
            {
                Console.WriteLine(GetNotationFromNumber(sqFrom) + "" + GetNotationFromNumber(sqTo) + " " + nodes);
            }
            else
            {
                string pieceStr = "" + Piece.PieceStr[promotedPiece];

                Console.WriteLine(GetNotationFromNumber(sqFrom) + "" + GetNotationFromNumber(sqTo) + pieceStr + " " + nodes);
            }
        }
        public static void PrintMove(int sqFrom, int sqTo)
        {
            Console.WriteLine(GetNotationFromNumber(sqFrom) + "" + GetNotationFromNumber(sqTo));
        }

        public static string GetNotationFromNumber(int sq)
        {
            int rank = sq / 8;
            int file = sq % 8;

            return RankFileDefs.FilesStr[file] + "" + RankFileDefs.RanksStr[rank];
        }
    }
}
