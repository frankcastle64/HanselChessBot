using HanselChessBOT.ConsoleApp.MagicGeneration;
using System.Runtime.CompilerServices;

namespace HanselChessBOT.ConsoleApp
{

    public ref struct LegalMoveManager
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
            if ((AttackMaps.KNIGHT_ATTACKS[sq] & Piece.Pieces_BB[knightColor]) > 0) return true;

            ulong occupancy = ~Piece.Pieces_BB[Piece.NO_PIECE];
            ulong occupancyRook = occupancy;
            occupancyRook &= MagicNumbers.RookAttacksInnerSixBits[sq];
            occupancyRook *= MagicNumbers.RookMagicNumbers[sq];
            occupancyRook >>= MagicNumbers.BitsRequiredByRook[sq];
            ulong movesRook = MagicNumbers.RookAttacks[sq][occupancyRook];
            if ((movesRook & Piece.Pieces_BB[rookColor]) > 0) return true;

            ulong occupancyBishop = occupancy;
            occupancyBishop &= MagicNumbers.BishopAttacksInnerSixBits[sq];
            occupancyBishop *= MagicNumbers.BishopMagicNumbers[sq];
            occupancyBishop >>= MagicNumbers.BitsRequiredByBishop[sq];
            ulong movesBishop = MagicNumbers.BishopAttacks[sq][occupancyBishop];
            if ((movesBishop & Piece.Pieces_BB[bishopColor]) > 0) return true;

            if (((movesRook | movesBishop) & Piece.Pieces_BB[queenColor]) > 0) return true;

            if ((AttackMaps.KING_ATTACKS[sq] & Piece.Pieces_BB[kingColor]) > 0) return true;
            if ((pawnAttacksMask & Piece.Pieces_BB[pawnColor]) > 0) return true;

            return false;
            ////ulong attackOnSquare = (AttackMaps.KNIGHT_ATTACKS[sq] & Piece.Pieces_BB[knightColor])
            ////                  | (AttackMaps.KING_ATTACKS[sq] & Piece.Pieces_BB[kingColor])
            ////                  | (pawnAttacksMask & Piece.Pieces_BB[pawnColor])
            ////                  | (movesRook & Piece.Pieces_BB[rookColor])
            ////                  | (movesBishop & Piece.Pieces_BB[bishopColor])
            ////                  | ((movesRook | movesBishop) & Piece.Pieces_BB[queenColor]);

            //return ((AttackMaps.KNIGHT_ATTACKS[sq] & Piece.Pieces_BB[knightColor]) > 0)
            //                 || ((AttackMaps.KING_ATTACKS[sq] & Piece.Pieces_BB[kingColor]) > 0)
            //                 || ((pawnAttacksMask & Piece.Pieces_BB[pawnColor]) > 0)
            //                 || ((movesRook & Piece.Pieces_BB[rookColor]) > 0)
            //                 || ((movesBishop & Piece.Pieces_BB[bishopColor]) > 0)
            //                 || (((movesRook | movesBishop) & Piece.Pieces_BB[queenColor]) > 0);


        }


    }
}
