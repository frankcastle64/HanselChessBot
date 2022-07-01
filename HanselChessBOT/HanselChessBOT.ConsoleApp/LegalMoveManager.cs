using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HanselChessBOT.ConsoleApp.MagicGeneration;

namespace HanselChessBOT.ConsoleApp
{

    public static class LegalMoveManager
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsKingInCheck(bool turn)
        {
            if (turn)
            {
                // Check for king's square.
                int whiteKingSq = Utilities.BitScanForward(Piece.Pieces_BB[Piece.WK]);

                bool isWhiteKingAttacked = IsSquareAttacked(whiteKingSq, AttackMaps.WHITE_PAWN_ATTACK_MASK[whiteKingSq], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK);
                return isWhiteKingAttacked;


            }
            else
            {
                int blackKingSq = Utilities.BitScanForward(Piece.Pieces_BB[Piece.BK]);
                bool isBlackKingAttacked = IsSquareAttacked(blackKingSq, AttackMaps.BLACK_PAWN_ATTACK_MASK[blackKingSq], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK);
                return isBlackKingAttacked;
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSquareAttacked(int sq, ulong pawnAttacksMask, int pawnColor, int knightColor, int bishopColor, int rookColor, int queenColor, int kingColor)
        {
            ulong occupancyRook = ~Piece.Pieces_BB[Piece.NO_PIECE];
            occupancyRook &= MagicNumbers.RookAttacksInnerSixBits[sq];
            occupancyRook *= MagicNumbers.RookMagicNumbers[sq];
            occupancyRook >>= 64 - MagicNumbers.BitsRequiredByRook[sq];
            ulong movesRook = MagicNumbers.RookAttacks[sq, occupancyRook];

            ulong occupancyBishop = ~Piece.Pieces_BB[Piece.NO_PIECE];
            occupancyBishop &= MagicNumbers.BishopAttacksInnerSixBits[sq];
            occupancyBishop *= MagicNumbers.BishopMagicNumbers[sq];
            occupancyBishop >>= 64 - MagicNumbers.BitsRequiredByBishop[sq];
            ulong movesBishop = MagicNumbers.BishopAttacks[sq, occupancyBishop];

            //ulong attackOnSquare = (AttackMaps.KNIGHT_ATTACKS[sq] & Piece.Pieces_BB[knightColor])
            //                  | (AttackMaps.KING_ATTACKS[sq] & Piece.Pieces_BB[kingColor])
            //                  | (pawnAttacksMask & Piece.Pieces_BB[pawnColor])
            //                  | (movesRook & Piece.Pieces_BB[rookColor])
            //                  | (movesBishop & Piece.Pieces_BB[bishopColor])
            //                  | ((movesRook | movesBishop) & Piece.Pieces_BB[queenColor]);

            return ((AttackMaps.KNIGHT_ATTACKS[sq] & Piece.Pieces_BB[knightColor]) > 0)
                             || ((AttackMaps.KING_ATTACKS[sq] & Piece.Pieces_BB[kingColor]) > 0)
                             || ((pawnAttacksMask & Piece.Pieces_BB[pawnColor]) > 0)
                             || ((movesRook & Piece.Pieces_BB[rookColor]) > 0)
                             || ((movesBishop & Piece.Pieces_BB[bishopColor]) > 0)
                             || (((movesRook | movesBishop) & Piece.Pieces_BB[queenColor]) > 0);


        }


    }
}
