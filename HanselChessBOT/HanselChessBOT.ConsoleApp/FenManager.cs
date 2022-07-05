using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanselChessBOT.ConsoleApp
{
    public class FenManager
    {
       
        public void SetPositionFromFen(string fen,ref BoardDefs boardDefs)
        {
            if (string.IsNullOrEmpty(fen))
            {
                fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            }

            string[] fenElements = fen.Split(' ');

            SetBoardFromFenString(fenElements[0], 0);
            SetTurn(fenElements[1]);
            UpdateCastlingRights(fenElements[2], 0, ref boardDefs);
            UpdateEnpassantSquare(fenElements[3], 0,ref boardDefs);
        }

        private void SetBoardFromFenString(string boardPos, int ply)
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

        private static void UpdateCastlingRights(string castleRightsStr, int ply, ref BoardDefs boardDefs)
        {
            boardDefs.GameStateInformationPerPly[ply].current_white_castle_rights = 0;
            boardDefs.GameStateInformationPerPly[ply].current_black_castle_rights = 0;

            if (castleRightsStr.Contains("K"))
            {
                boardDefs.GameStateInformationPerPly[ply].current_white_castle_rights |= 1;
            }
            if (castleRightsStr.Contains("Q"))
            {
                boardDefs.GameStateInformationPerPly[ply].current_white_castle_rights |= 2;
            }
            if (castleRightsStr.Contains("k"))
            {
                boardDefs.GameStateInformationPerPly[ply].current_black_castle_rights |= 4;
            }
            if (castleRightsStr.Contains("q"))
            {
                boardDefs.GameStateInformationPerPly[ply].current_black_castle_rights |= 8;
            }

            boardDefs.GameStateInformationPerPly[ply].previous_white_castle_rights = boardDefs.GameStateInformationPerPly[ply].current_white_castle_rights;
            boardDefs.GameStateInformationPerPly[ply].previous_black_castle_rights = boardDefs.GameStateInformationPerPly[ply].current_black_castle_rights;
        }

        private static void UpdateEnpassantSquare(string enpassantSq, int ply, ref BoardDefs boardDefs)
        {
            int epSq = 0;
            if (enpassantSq != "-")
            {
                char file = enpassantSq[0];
                char rank = enpassantSq[1];
                epSq = 8 * (rank - '1') + (file - 'a');
            }
            boardDefs.GameStateInformationPerPly[ply].current_enpassant_sq = epSq;
            boardDefs.GameStateInformationPerPly[ply].previous_enpassant_sq = boardDefs.GameStateInformationPerPly[ply].current_enpassant_sq;
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
        }
    }
}
