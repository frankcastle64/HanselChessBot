using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanselChessBOT.ConsoleApp
{
    public struct GameStateInformation
    {
        public int current_enpassant_sq;
        public int previous_enpassant_sq;
        public int current_white_castle_rights;
        public int previous_white_castle_rights;
        public int current_black_castle_rights;
        public int previous_black_castle_rights;
    }

    public struct CurrentStateInformation
    {
        public static bool turn;
    }

    public ref struct BoardDefs
    {

        public static int[] Board = new int[64];
        public Span<GameStateInformation> GameStateInformationPerPly; 

        public BoardDefs()
        {
            this.GameStateInformationPerPly = new GameStateInformation[MoveGeneration.MAX_PLY];
        }

        public void ResetStates()
        {
            // Reset the Board to empty.
            for (int sq = Square.a1; sq <= Square.h8; sq++)
            {
                Board[sq] = Piece.NO_PIECE;
            }

            // Reset the game state information.
            for (int index = 0; index < MoveGeneration.MAX_PLY; index++)
            {
                GameStateInformationPerPly[index].current_white_castle_rights = 0;
                GameStateInformationPerPly[index].previous_white_castle_rights = 0;
                GameStateInformationPerPly[index].current_black_castle_rights = 0;
                GameStateInformationPerPly[index].previous_black_castle_rights = 0;
                GameStateInformationPerPly[index].previous_enpassant_sq = 0;
                GameStateInformationPerPly[index].current_enpassant_sq = 0;
            }

            // Reset the bitboards
            for (int piece = Piece.NO_PIECE; piece <= Piece.BK; piece++)
            {
                Piece.Pieces_BB[piece] = 0UL;
            }

            // Reset the turn to white.
            CurrentStateInformation.turn = true;

            // Allocate jagged-array for move list.
            for (int ply = 0; ply < MoveGeneration.MAX_PLY; ply++)
            {
                MoveGeneration.moveList[ply] = new Move[MoveGeneration.MAX_MOVES_PER_PLY];
            }
        }
        public void InitOnce()
        {
            ResetStates();
            MagicGeneration.MagicNumbers.InitMagics();
        }
    }
}
