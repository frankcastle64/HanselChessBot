using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanselChessBOT.ConsoleApp
{
    public static class FenManager
    {
        public static void SetPositionFromFen(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            string[] fenElements = fen.Split(' ');

            SetBoardFromFenString(fenElements[0], 0);
            SetTurn(fenElements[1]);
            UpdateCastlingRights(fenElements[2], 0);
            UpdateEnpassantSquare(fenElements[3], 0);
        }

        private static void SetBoardFromFenString(string boardPos, int ply)
        {
            int file = RankFileDefs.File_A;
            int rank = RankFileDefs.Rank_8;
            int boardPosLength = boardPos.Length;
            for (int i = 0; i < boardPosLength; i++)
            {
                int sq = 8 * rank + file;
                switch (boardPos[i])
                {
                    case 'P': BoardDefs.Board[sq] = Piece.WP; file++; break;
                    case 'N': BoardDefs.Board[sq] = Piece.WN; file++; break;
                    case 'B': BoardDefs.Board[sq] = Piece.WB; file++; break;
                    case 'R': BoardDefs.Board[sq] = Piece.WR; file++; break;
                    case 'Q': BoardDefs.Board[sq] = Piece.WQ; file++; break;
                    case 'K': BoardDefs.Board[sq] = Piece.WK; file++; break;

                    case 'p': BoardDefs.Board[sq] = Piece.BP; file++; break;
                    case 'n': BoardDefs.Board[sq] = Piece.BN; file++; break;
                    case 'b': BoardDefs.Board[sq] = Piece.BB; file++; break;
                    case 'r': BoardDefs.Board[sq] = Piece.BR; file++; break;
                    case 'q': BoardDefs.Board[sq] = Piece.BQ; file++; break;
                    case 'k': BoardDefs.Board[sq] = Piece.BK; file++; break;

                    case '/': rank = rank - 1; file = RankFileDefs.File_A; break;

                    default: file = file + (boardPos[i] - '0'); break;

                }
            }

            UpdatePiecesBitBoards();

        }

        private static void UpdateCastlingRights(string castleRightsStr, int ply)
        {
            BoardDefs.GameStateInformationPerPly[ply].current_white_castle_rights = 0;
            BoardDefs.GameStateInformationPerPly[ply].current_black_castle_rights = 0;

            if (castleRightsStr.Contains("K"))
            {
                BoardDefs.GameStateInformationPerPly[ply].current_white_castle_rights |= 1;
            }
            if (castleRightsStr.Contains("Q"))
            {
                BoardDefs.GameStateInformationPerPly[ply].current_white_castle_rights |= 2;
            }
            if (castleRightsStr.Contains("k"))
            {
                BoardDefs.GameStateInformationPerPly[ply].current_black_castle_rights |= 4;
            }
            if (castleRightsStr.Contains("q"))
            {
                BoardDefs.GameStateInformationPerPly[ply].current_black_castle_rights |= 8;
            }

            BoardDefs.GameStateInformationPerPly[ply].previous_white_castle_rights = BoardDefs.GameStateInformationPerPly[ply].current_white_castle_rights;
            BoardDefs.GameStateInformationPerPly[ply].previous_black_castle_rights = BoardDefs.GameStateInformationPerPly[ply].current_black_castle_rights;
        }

        private static void UpdateEnpassantSquare(string enpassantSq, int ply)
        {
            int epSq = 0;
            if (enpassantSq != "-")
            {
                char file = enpassantSq[0];
                char rank = enpassantSq[1];
                epSq = 8 * (rank - '1') + (file - 'a');
            }
            BoardDefs.GameStateInformationPerPly[ply].current_enpassant_sq = epSq;
            BoardDefs.GameStateInformationPerPly[ply].previous_enpassant_sq = BoardDefs.GameStateInformationPerPly[ply].current_enpassant_sq;
        }

        private static void SetTurn(string turnCh)
        {
            if (turnCh == "w")
            {
                CurrentStateInformation.turn = true;
            }
            else
            {
                CurrentStateInformation.turn = false;
            }
        }
        private static void UpdatePiecesBitBoards()
        {
            for (int sq = Square.a1; sq <= Square.h8; sq++)
            {
                Piece.Pieces_BB[BoardDefs.Board[sq]] ^= (1UL << sq);
            }

            // Update pieces by their piece types.
            //Piece.PiecesByType_BB[Piece.PieceType_NO_Piece] = Piece.Pieces_BB[Piece.NO_PIECE];
            //Piece.PiecesByType_BB[Piece.PieceType_Pawn] = Piece.Pieces_BB[Piece.WP] | Piece.Pieces_BB[Piece.BP];
            //Piece.PiecesByType_BB[Piece.PieceType_Knight] = Piece.Pieces_BB[Piece.WN] | Piece.Pieces_BB[Piece.BN];
            //Piece.PiecesByType_BB[Piece.PieceType_Bishop] = Piece.Pieces_BB[Piece.WB] | Piece.Pieces_BB[Piece.BB];
            //Piece.PiecesByType_BB[Piece.PieceType_Rook] = Piece.Pieces_BB[Piece.WR] | Piece.Pieces_BB[Piece.BR];
            //Piece.PiecesByType_BB[Piece.PieceType_Queen] = Piece.Pieces_BB[Piece.WQ] | Piece.Pieces_BB[Piece.BQ];
            //Piece.PiecesByType_BB[Piece.PieceType_King] = Piece.Pieces_BB[Piece.WK] | Piece.Pieces_BB[Piece.BK];

            //// Update pieces by their colors.
            //Piece.PiecesByColor_BB[CommonDefs.WHITE_TURN] = Piece.Pieces_BB[Piece.WP] | Piece.Pieces_BB[Piece.WN] | Piece.Pieces_BB[Piece.WB] | Piece.Pieces_BB[Piece.WR] | Piece.Pieces_BB[Piece.WQ] | Piece.Pieces_BB[Piece.WK];
            //Piece.PiecesByColor_BB[CommonDefs.BLACK_TURN] = Piece.Pieces_BB[Piece.BP] | Piece.Pieces_BB[Piece.BN] | Piece.Pieces_BB[Piece.BB] | Piece.Pieces_BB[Piece.BR] | Piece.Pieces_BB[Piece.BQ] | Piece.Pieces_BB[Piece.BK];
        }
    }
}
