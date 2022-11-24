using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace HanselChessBOT.ConsoleApp
{
    public struct Move
    {
        public long move;
        public int score;
    }


    public ref struct MoveGeneration
    {
        public BoardDefs boardDefs;

        public MoveGeneration(BoardDefs boardDefs)
        {
            this.boardDefs = boardDefs;
        }
        public static int MAX_PLY = 10;
        public const int MAX_MOVES_PER_PLY = 255;
        public static int[] MOVES_AT_PLY = new int[MAX_MOVES_PER_PLY];

        public static readonly Move[][] moveList = new Move[MAX_PLY][];

        public void GenerateMoves(int ply, bool turn, int n)
        {
            n = GenerateNonCaptures(ply, turn, n);
            n = GenerateCaptures(ply, turn, n);
        }
        public int GenerateNonCaptures(int ply, bool turn, int n)
        {
            switch (turn)
            {
                case true:
                    n = GenerateWhitePawnNonCapture(ply, n);
                    n = GenerateKnightMoves(ply, n, Piece.WN, CommonDefs.MOVETYPE_ORDINARY, Piece.Pieces_BB[Piece.NO_PIECE]);
                    n = GenerateKingMoves(ply, n, Piece.WK, CommonDefs.MOVETYPE_ORDINARY, Piece.Pieces_BB[Piece.NO_PIECE]);
                    n = GenerateCastleMoves(n, ply, Piece.WK);
                    n = GenerateBishopMoves(ply, n, Piece.WB, CommonDefs.MOVETYPE_ORDINARY, Piece.Pieces_BB[Piece.NO_PIECE]);
                    n = GenerateRookMoves(ply, n, Piece.WR, CommonDefs.MOVETYPE_ORDINARY, Piece.Pieces_BB[Piece.NO_PIECE]);
                    n = GenerateQueenMoves(ply, n, Piece.WQ, CommonDefs.MOVETYPE_ORDINARY, Piece.Pieces_BB[Piece.NO_PIECE]);
                    break;
                case false:
                    n = GenerateBlackPawnNonCapture(ply, n);
                    n = GenerateKnightMoves(ply, n, Piece.BN, CommonDefs.MOVETYPE_ORDINARY, Piece.Pieces_BB[Piece.NO_PIECE]);
                    n = GenerateKingMoves(ply, n, Piece.BK, CommonDefs.MOVETYPE_ORDINARY, Piece.Pieces_BB[Piece.NO_PIECE]);
                    n = GenerateCastleMoves(n, ply, Piece.BK);
                    n = GenerateBishopMoves(ply, n, Piece.BB, CommonDefs.MOVETYPE_ORDINARY, Piece.Pieces_BB[Piece.NO_PIECE]);
                    n = GenerateRookMoves(ply, n, Piece.BR, CommonDefs.MOVETYPE_ORDINARY, Piece.Pieces_BB[Piece.NO_PIECE]);
                    n = GenerateQueenMoves(ply, n, Piece.BQ, CommonDefs.MOVETYPE_ORDINARY, Piece.Pieces_BB[Piece.NO_PIECE]);
                    break;
            }

            MOVES_AT_PLY[ply] = n;
            return n;
        }
        public int GenerateCaptures(int ply, bool turn, int n)
        {

            switch (turn)
            {
                case true:
                    {
                        ulong targetBitBoard = Piece.Pieces_BB[Piece.BP] | Piece.Pieces_BB[Piece.BN] | Piece.Pieces_BB[Piece.BB] | Piece.Pieces_BB[Piece.BR] | Piece.Pieces_BB[Piece.BQ];
                        n = GenerateWhitePawnCapture(ply, n, targetBitBoard);
                        n = GenerateKnightMoves(ply, n, Piece.WN, CommonDefs.MOVETYPE_CAPTURE, targetBitBoard);
                        n = GenerateKingMoves(ply, n, Piece.WK, CommonDefs.MOVETYPE_CAPTURE, targetBitBoard);
                        n = GenerateBishopMoves(ply, n, Piece.WB, CommonDefs.MOVETYPE_CAPTURE, targetBitBoard);
                        n = GenerateRookMoves(ply, n, Piece.WR, CommonDefs.MOVETYPE_CAPTURE, targetBitBoard);
                        n = GenerateQueenMoves(ply, n, Piece.WQ, CommonDefs.MOVETYPE_CAPTURE, targetBitBoard);
                    }
                    break;
                case false:
                    {
                        ulong targetBitBoard = Piece.Pieces_BB[Piece.WP] | Piece.Pieces_BB[Piece.WN] | Piece.Pieces_BB[Piece.WB] | Piece.Pieces_BB[Piece.WR] | Piece.Pieces_BB[Piece.WQ];

                        n = GenerateBlackPawnCapture(ply, n, targetBitBoard);
                        n = GenerateKnightMoves(ply, n, Piece.BN, CommonDefs.MOVETYPE_CAPTURE, targetBitBoard);
                        n = GenerateKingMoves(ply, n, Piece.BK, CommonDefs.MOVETYPE_CAPTURE, targetBitBoard);
                        n = GenerateBishopMoves(ply, n, Piece.BB, CommonDefs.MOVETYPE_CAPTURE, targetBitBoard);
                        n = GenerateRookMoves(ply, n, Piece.BR, CommonDefs.MOVETYPE_CAPTURE, targetBitBoard);
                        n = GenerateQueenMoves(ply, n, Piece.BQ, CommonDefs.MOVETYPE_CAPTURE, targetBitBoard);
                    }
                    break;
            }




            MOVES_AT_PLY[ply] = n;
            return n;
        }
        public int GenerateWhitePawnNonCapture(int ply, int n)
        {
            ulong whitePawnsBB = Piece.Pieces_BB[Piece.WP];
            ulong emptyBitBoard = Piece.Pieces_BB[Piece.NO_PIECE];

            // Promotion pushes.
            // Double pawn pushes.
            // Single pawn pushes.
            ulong singlePawnPushBB = (whitePawnsBB << DirectionMasks.North) & emptyBitBoard;
            ulong singlePawnPushToPromotion = singlePawnPushBB & RankFileDefs.Rank_8_BB;
            ulong singlePawnPushNonPromotion = singlePawnPushBB & ~RankFileDefs.Rank_8_BB;
            ulong doublePawnPush = ((singlePawnPushBB & RankFileDefs.Rank_3_BB) << DirectionMasks.North) & emptyBitBoard;

            while (singlePawnPushToPromotion > 0)
            {
                int sq = Utilities.BitScanForward(singlePawnPushToPromotion);
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.South, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, Piece.NO_PIECE, Piece.WN);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.South, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, Piece.NO_PIECE, Piece.WB);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.South, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, Piece.NO_PIECE, Piece.WR);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.South, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, Piece.NO_PIECE, Piece.WQ);
                n++;

                singlePawnPushToPromotion &= singlePawnPushToPromotion - 1;
            }

            while (doublePawnPush > 0)
            {
                int sq = Utilities.BitScanForward(doublePawnPush);
                moveList[ply][n].move = EncodeMove(sq + (2 * DirectionMasks.South), sq, CommonDefs.MOVETYPE_ORDINARY, Piece.WP, Piece.NO_PIECE, Piece.NO_PIECE);
                n++;
                doublePawnPush &= doublePawnPush - 1;
            }

            while (singlePawnPushNonPromotion > 0)
            {
                int sq = Utilities.BitScanForward(singlePawnPushNonPromotion);
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.South, sq, CommonDefs.MOVETYPE_ORDINARY, Piece.WP, Piece.NO_PIECE, Piece.NO_PIECE);
                n++;
                singlePawnPushNonPromotion &= singlePawnPushNonPromotion - 1;
            }

            return n;
        }
        public int GenerateWhitePawnCapture(int ply, int n, ulong targetBitBoard)
        {
            ulong whitePawnsBB = Piece.Pieces_BB[Piece.WP];
            // Captures in the a1-h8 direction:
            ulong pawnTargetsNorthEast = (whitePawnsBB << DirectionMasks.NorthEast) & ~RankFileDefs.File_A_BB & targetBitBoard;

            // Promotions:
            ulong pawnTargetsPromotionsNorthEast = pawnTargetsNorthEast & RankFileDefs.Rank_8_BB;

            while (pawnTargetsPromotionsNorthEast > 0)
            {
                int sq = Utilities.BitScanForward(pawnTargetsPromotionsNorthEast);
                moveList[ply][n].move = EncodeMove(sq - DirectionMasks.NorthEast, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, BoardDefs.Board[sq], Piece.WN);
                n++;
                moveList[ply][n].move = EncodeMove(sq - DirectionMasks.NorthEast, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, BoardDefs.Board[sq], Piece.WB);
                n++;
                moveList[ply][n].move = EncodeMove(sq - DirectionMasks.NorthEast, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, BoardDefs.Board[sq], Piece.WR);
                n++;
                moveList[ply][n].move = EncodeMove(sq - DirectionMasks.NorthEast, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, BoardDefs.Board[sq], Piece.WQ);
                n++;

                pawnTargetsPromotionsNorthEast &= pawnTargetsPromotionsNorthEast - 1;
            }

            // Non-promotions:
            ulong pawnTargetsNonPromotionsNorthEast = pawnTargetsNorthEast & ~RankFileDefs.Rank_8_BB;
            while (pawnTargetsNonPromotionsNorthEast > 0)
            {
                int sq = Utilities.BitScanForward(pawnTargetsNonPromotionsNorthEast);
                moveList[ply][n].move = EncodeMove(sq - DirectionMasks.NorthEast, sq, CommonDefs.MOVETYPE_CAPTURE, Piece.WP, BoardDefs.Board[sq], Piece.NO_PIECE);
                n++;
                pawnTargetsNonPromotionsNorthEast &= pawnTargetsNonPromotionsNorthEast - 1;
            }

            // Captures in the h1-a8 direction:
            ulong pawnTargetsNorthWest = (whitePawnsBB << DirectionMasks.NorthWest) & ~RankFileDefs.File_H_BB & targetBitBoard;

            // Promotions:
            ulong pawnTargetsPromotionsNorthWest = pawnTargetsNorthWest & RankFileDefs.Rank_8_BB;

            while (pawnTargetsPromotionsNorthWest > 0)
            {
                int sq = Utilities.BitScanForward(pawnTargetsPromotionsNorthWest);
                moveList[ply][n].move = EncodeMove(sq - DirectionMasks.NorthWest, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, BoardDefs.Board[sq], Piece.WN);
                n++;
                moveList[ply][n].move = EncodeMove(sq - DirectionMasks.NorthWest, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, BoardDefs.Board[sq], Piece.WB);
                n++;
                moveList[ply][n].move = EncodeMove(sq - DirectionMasks.NorthWest, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, BoardDefs.Board[sq], Piece.WR);
                n++;
                moveList[ply][n].move = EncodeMove(sq - DirectionMasks.NorthWest, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.WP, BoardDefs.Board[sq], Piece.WQ);
                n++;

                pawnTargetsPromotionsNorthWest &= pawnTargetsPromotionsNorthWest - 1;
            }

            // Non-promotions:
            ulong pawnTargetsNonPromotionsNorthWest = pawnTargetsNorthWest & ~RankFileDefs.Rank_8_BB;
            while (pawnTargetsNonPromotionsNorthWest > 0)
            {
                int sq = Utilities.BitScanForward(pawnTargetsNonPromotionsNorthWest);
                moveList[ply][n].move = EncodeMove(sq - DirectionMasks.NorthWest, sq, CommonDefs.MOVETYPE_CAPTURE, Piece.WP, BoardDefs.Board[sq], Piece.NO_PIECE);
                n++;
                pawnTargetsNonPromotionsNorthWest &= pawnTargetsNonPromotionsNorthWest - 1;
            }

            if (boardDefs.GameStateInformationPerPly[ply].current_enpassant_sq > 0)
            {
                ulong enpassant_captures = whitePawnsBB & AttackMaps.BLACK_PAWN_ATTACK_MASK[boardDefs.GameStateInformationPerPly[ply].current_enpassant_sq];

                while (enpassant_captures > 0)
                {
                    int sq = Utilities.BitScanForward(enpassant_captures);
                    moveList[ply][n].move = EncodeMove(sq, boardDefs.GameStateInformationPerPly[ply].current_enpassant_sq, CommonDefs.MOVETYPE_ENPASSANT, Piece.WP, Piece.NO_PIECE, Piece.NO_PIECE);
                    n++;
                    enpassant_captures &= enpassant_captures - 1;
                }
            }

            return n;
        }

        public int GenerateBlackPawnNonCapture(int ply, int n)
        {
            ulong blackPawnsBB = Piece.Pieces_BB[Piece.BP];
            ulong emptyBitBoard = Piece.Pieces_BB[Piece.NO_PIECE];

            // Promotion pushes.
            // Double pawn pushes.
            // Single pawn pushes.
            ulong singlePawnPushBB = (blackPawnsBB >> DirectionMasks.North) & emptyBitBoard;
            ulong singlePawnPushToPromotion = singlePawnPushBB & RankFileDefs.Rank_1_BB;
            ulong singlePawnPushNonPromotion = singlePawnPushBB & ~RankFileDefs.Rank_1_BB;
            ulong doublePawnPush = ((singlePawnPushBB & RankFileDefs.Rank_6_BB) >> DirectionMasks.North) & emptyBitBoard;

            while (singlePawnPushToPromotion > 0)
            {
                int sq = Utilities.BitScanForward(singlePawnPushToPromotion);
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.North, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, Piece.NO_PIECE, Piece.BN);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.North, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, Piece.NO_PIECE, Piece.BB);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.North, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, Piece.NO_PIECE, Piece.BR);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.North, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, Piece.NO_PIECE, Piece.BQ);
                n++;

                singlePawnPushToPromotion &= singlePawnPushToPromotion - 1;
            }

            while (doublePawnPush > 0)
            {
                int sq = Utilities.BitScanForward(doublePawnPush);
                moveList[ply][n].move = EncodeMove(sq + (2 * DirectionMasks.North), sq, CommonDefs.MOVETYPE_ORDINARY, Piece.BP, Piece.NO_PIECE, Piece.NO_PIECE);
                n++;
                doublePawnPush &= doublePawnPush - 1;
            }

            while (singlePawnPushNonPromotion > 0)
            {
                int sq = Utilities.BitScanForward(singlePawnPushNonPromotion);
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.North, sq, CommonDefs.MOVETYPE_ORDINARY, Piece.BP, Piece.NO_PIECE, Piece.NO_PIECE);
                n++;
                singlePawnPushNonPromotion &= singlePawnPushNonPromotion - 1;
            }


            return n;
        }
        public int GenerateBlackPawnCapture(int ply, int n, ulong targetBitBoard)
        {
            ulong blackPawnsBB = Piece.Pieces_BB[Piece.BP];

            // Captures in the a1-h8 direction:
            ulong pawnTargetsNorthWest = (blackPawnsBB >> DirectionMasks.NorthWest) & ~RankFileDefs.File_A_BB & targetBitBoard;

            // Promotions:
            ulong pawnTargetsPromotionsNorthWest = pawnTargetsNorthWest & RankFileDefs.Rank_1_BB;

            while (pawnTargetsPromotionsNorthWest > 0)
            {
                int sq = Utilities.BitScanForward(pawnTargetsPromotionsNorthWest);
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.NorthWest, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, BoardDefs.Board[sq], Piece.BN);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.NorthWest, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, BoardDefs.Board[sq], Piece.BB);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.NorthWest, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, BoardDefs.Board[sq], Piece.BR);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.NorthWest, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, BoardDefs.Board[sq], Piece.BQ);
                n++;

                pawnTargetsPromotionsNorthWest &= pawnTargetsPromotionsNorthWest - 1;
            }

            // Non-promotions:
            ulong pawnTargetsNonPromotionsNorthWest = pawnTargetsNorthWest & ~RankFileDefs.Rank_1_BB;
            while (pawnTargetsNonPromotionsNorthWest > 0)
            {
                int sq = Utilities.BitScanForward(pawnTargetsNonPromotionsNorthWest);
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.NorthWest, sq, CommonDefs.MOVETYPE_CAPTURE, Piece.BP, BoardDefs.Board[sq], Piece.NO_PIECE);
                n++;
                pawnTargetsNonPromotionsNorthWest &= pawnTargetsNonPromotionsNorthWest - 1;
            }

            // Captures in the h1-a8 direction:
            ulong pawnTargetsNorthEast = (blackPawnsBB >> DirectionMasks.NorthEast) & ~RankFileDefs.File_H_BB & targetBitBoard;

            // Promotions:
            ulong pawnTargetsPromotionsNorthEast = pawnTargetsNorthEast & RankFileDefs.Rank_1_BB;

            while (pawnTargetsPromotionsNorthEast > 0)
            {
                int sq = Utilities.BitScanForward(pawnTargetsPromotionsNorthEast);
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.NorthEast, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, BoardDefs.Board[sq], Piece.BN);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.NorthEast, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, BoardDefs.Board[sq], Piece.BB);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.NorthEast, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, BoardDefs.Board[sq], Piece.BR);
                n++;
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.NorthEast, sq, CommonDefs.MOVETYPE_PROMOTION, Piece.BP, BoardDefs.Board[sq], Piece.BQ);
                n++;

                pawnTargetsPromotionsNorthEast &= pawnTargetsPromotionsNorthEast - 1;
            }

            // Non-promotions:
            ulong pawnTargetsNonPromotionsNorthEast = pawnTargetsNorthEast & ~RankFileDefs.Rank_1_BB;
            while (pawnTargetsNonPromotionsNorthEast > 0)
            {
                int sq = Utilities.BitScanForward(pawnTargetsNonPromotionsNorthEast);
                moveList[ply][n].move = EncodeMove(sq + DirectionMasks.NorthEast, sq, CommonDefs.MOVETYPE_CAPTURE, Piece.BP, BoardDefs.Board[sq], Piece.NO_PIECE);
                n++;
                pawnTargetsNonPromotionsNorthEast &= pawnTargetsNonPromotionsNorthEast - 1;
            }

            if (boardDefs.GameStateInformationPerPly[ply].current_enpassant_sq > 0)
            {
                ulong enpassant_captures = blackPawnsBB & AttackMaps.WHITE_PAWN_ATTACK_MASK[boardDefs.GameStateInformationPerPly[ply].current_enpassant_sq];

                while (enpassant_captures > 0)
                {
                    int sq = Utilities.BitScanForward(enpassant_captures);
                    moveList[ply][n].move = EncodeMove(sq, boardDefs.GameStateInformationPerPly[ply].current_enpassant_sq, CommonDefs.MOVETYPE_ENPASSANT, Piece.BP, Piece.NO_PIECE, Piece.NO_PIECE);
                    n++;
                    enpassant_captures &= enpassant_captures - 1;
                }
            }

            return n;
        }

        public static int GenerateKnightMoves(int ply, int n, int piece, int moveType, ulong targetBitboard)
        {
            int from, to;
            ulong bb = Piece.Pieces_BB[piece];

            while (bb > 0)
            {
                from = Utilities.BitScanForward(bb);
                ulong attacks = AttackMaps.KNIGHT_ATTACKS[from] & targetBitboard;
                while (attacks > 0)
                {
                    to = Utilities.BitScanForward(attacks);
                    moveList[ply][n].move = EncodeMove(from, to, moveType, piece, BoardDefs.Board[to], Piece.NO_PIECE);
                    n++;
                    attacks &= attacks - 1;
                }
                bb &= bb - 1;
            }

            return n;
        }

        public static int GenerateKingMoves(int ply, int n, int piece, int moveType, ulong targetBitboard)
        {
            int from, to;
            ulong bb = Piece.Pieces_BB[piece];

            from = Utilities.BitScanForward(bb);
            ulong attacks = AttackMaps.KING_ATTACKS[from] & targetBitboard;
            while (attacks > 0)
            {
                to = Utilities.BitScanForward(attacks);
                moveList[ply][n].move = EncodeMove(from, to, moveType, piece, BoardDefs.Board[to], Piece.NO_PIECE);
                n++;
                attacks &= attacks - 1;
            }

            return n;
        }

        private int GenerateCastleMoves(int n, int ply, int piece)
        {
            if (piece == Piece.WK)
            {
                if ((boardDefs.GameStateInformationPerPly[ply].current_white_castle_rights & 1) != 0)
                {
                    // Check if f1 and g1 are empty.
                    bool isF1Empty = (Piece.Pieces_BB[Piece.NO_PIECE] & (1UL << Square.f1)) > 0;
                    bool isG1Empty = (Piece.Pieces_BB[Piece.NO_PIECE] & (1UL << Square.g1)) > 0;


                    if (isF1Empty && isG1Empty)
                    {
                        // Check if e1, f1 and g1 are attacked.
                        //bool e1IsAttacked = LegalMoveManager.IsSquareAttacked(Square.e1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.e1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK);
                        //bool f1IsAttacked = LegalMoveManager.IsSquareAttacked(Square.f1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.f1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK);
                        //bool g1IsAttacked = LegalMoveManager.IsSquareAttacked(Square.g1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.g1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK);
                        if (!LegalMoveManager.IsSquareAttacked(Square.e1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.e1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK) &&
                            !LegalMoveManager.IsSquareAttacked(Square.f1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.f1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK) &&
                            !LegalMoveManager.IsSquareAttacked(Square.g1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.g1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK)
                            )
                        {
                            moveList[ply][n].move = EncodeMove(Square.e1, Square.g1, CommonDefs.MOVETYPE_KING_CASTLE, Piece.WK, Piece.NO_PIECE, Piece.NO_PIECE);
                            n++;
                        }
                    }


                }
                if ((boardDefs.GameStateInformationPerPly[ply].current_white_castle_rights & 2) != 0)
                {
                    // Check if b1 and c1 and d1 are empty.
                    bool isB1Empty = (Piece.Pieces_BB[Piece.NO_PIECE] & (1UL << Square.b1)) > 0;
                    bool isC1Empty = (Piece.Pieces_BB[Piece.NO_PIECE] & (1UL << Square.c1)) > 0;
                    bool isD1Empty = (Piece.Pieces_BB[Piece.NO_PIECE] & (1UL << Square.d1)) > 0;

                    //// Check if e1, d1 and c1 are attacked.
                    //bool e1IsAttacked = LegalMoveManager.IsSquareAttacked(Square.e1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.e1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK);
                    //bool d1IsAttacked = LegalMoveManager.IsSquareAttacked(Square.d1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.d1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK);
                    //bool c1IsAttacked = LegalMoveManager.IsSquareAttacked(Square.c1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.c1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK);

                    if (isB1Empty && isC1Empty && isD1Empty)
                    {
                        if (!LegalMoveManager.IsSquareAttacked(Square.e1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.e1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK) &&
                           !LegalMoveManager.IsSquareAttacked(Square.d1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.d1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK) &&
                           !LegalMoveManager.IsSquareAttacked(Square.c1, AttackMaps.WHITE_PAWN_ATTACK_MASK[Square.c1], Piece.BP, Piece.BN, Piece.BB, Piece.BR, Piece.BQ, Piece.BK)
                           )
                        {
                            moveList[ply][n].move = EncodeMove(Square.e1, Square.c1, CommonDefs.MOVETYPE_QUEEN_CASTLE, Piece.WK, Piece.NO_PIECE, Piece.NO_PIECE);
                            n++;
                        }
                    }
                }

            }
            else
            {
                if ((boardDefs.GameStateInformationPerPly[ply].current_black_castle_rights & 4) != 0)
                {
                    // Check if f8 and g8 are empty.
                    bool isF8Empty = (Piece.Pieces_BB[Piece.NO_PIECE] & (1UL << Square.f8)) > 0;
                    bool isG8Empty = (Piece.Pieces_BB[Piece.NO_PIECE] & (1UL << Square.g8)) > 0;

                    //// Check if e8, f8 and g8 are attacked.
                    //bool e8IsAttacked = LegalMoveManager.IsSquareAttacked(Square.e8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.e8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK);
                    //bool f8IsAttacked = LegalMoveManager.IsSquareAttacked(Square.f8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.f8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK);
                    //bool g8IsAttacked = LegalMoveManager.IsSquareAttacked(Square.g8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.g8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK);

                    if (isF8Empty && isG8Empty)
                    {
                        if (!LegalMoveManager.IsSquareAttacked(Square.e8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.e8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK) &&
                           !LegalMoveManager.IsSquareAttacked(Square.f8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.f8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK) &&
                           !LegalMoveManager.IsSquareAttacked(Square.g8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.g8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK)
                           )
                        {
                            moveList[ply][n].move = EncodeMove(Square.e8, Square.f8, CommonDefs.MOVETYPE_KING_CASTLE, Piece.BK, Piece.NO_PIECE, Piece.NO_PIECE);
                            n++;
                        }
                    }


                }

                if ((boardDefs.GameStateInformationPerPly[ply].current_black_castle_rights & 8) != 0)
                {
                    // Check if b8 and c8 and d8 are empty.
                    bool isB8Empty = (Piece.Pieces_BB[Piece.NO_PIECE] & (1UL << Square.b8)) > 0;
                    bool isC8Empty = (Piece.Pieces_BB[Piece.NO_PIECE] & (1UL << Square.c8)) > 0;
                    bool isD8Empty = (Piece.Pieces_BB[Piece.NO_PIECE] & (1UL << Square.d8)) > 0;

                    // Check if e8, d8 and c8 are attacked.
                    //bool e8IsAttacked = LegalMoveManager.IsSquareAttacked(Square.e8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.e8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK);
                    //bool d8IsAttacked = LegalMoveManager.IsSquareAttacked(Square.d8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.d8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK);
                    //bool c8IsAttacked = LegalMoveManager.IsSquareAttacked(Square.c8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.c8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK);

                    //if (isB8Empty && isC8Empty && isD8Empty && !e8IsAttacked && !d8IsAttacked && !c8IsAttacked)
                    //{
                    //    moveList[ply][n].move = EncodeMove(Square.e8, Square.c8, CommonDefs.MOVETYPE_QUEEN_CASTLE, Piece.BK, Piece.NO_PIECE, Piece.NO_PIECE);
                    //    n++;
                    //}

                    if (isB8Empty && isC8Empty && isD8Empty)
                    {
                        if (!LegalMoveManager.IsSquareAttacked(Square.e8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.e8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK) &&
                           !LegalMoveManager.IsSquareAttacked(Square.d8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.d8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK) &&
                           !LegalMoveManager.IsSquareAttacked(Square.c8, AttackMaps.BLACK_PAWN_ATTACK_MASK[Square.c8], Piece.WP, Piece.WN, Piece.WB, Piece.WR, Piece.WQ, Piece.WK)
                           )
                        {
                            moveList[ply][n].move = EncodeMove(Square.e8, Square.c8, CommonDefs.MOVETYPE_QUEEN_CASTLE, Piece.BK, Piece.NO_PIECE, Piece.NO_PIECE);
                            n++;
                        }
                    }
                }

            }

            return n;
        }

        public static int GenerateBishopMoves(int ply, int n, int piece, int moveType, ulong targetBitBoard)
        {
            ulong bb = Piece.Pieces_BB[piece];

            while (bb > 0)
            {
                int from = Utilities.BitScanForward(bb);

                ulong occupancy = ~Piece.Pieces_BB[Piece.NO_PIECE];
                occupancy &= MagicGeneration.MagicNumbers.BishopAttacksInnerSixBits[from];
                occupancy *= MagicGeneration.MagicNumbers.BishopMagicNumbers[from];
                occupancy >>= MagicGeneration.MagicNumbers.BitsRequiredByBishop[from];
                ulong moves = MagicGeneration.MagicNumbers.BishopAttacks[from][occupancy] & targetBitBoard;

                while (moves > 0)
                {
                    int to = Utilities.BitScanForward(moves);

                    moveList[ply][n].move = EncodeMove(from, to, moveType, piece, BoardDefs.Board[to], Piece.NO_PIECE);
                    n++;
                    moves = moves & moves - 1;

                }

                bb &= bb - 1;

            }

            return n;
        }

        public static int GenerateRookMoves(int ply, int n, int piece, int moveType, ulong targetBitBoard)
        {
            ulong bb = Piece.Pieces_BB[piece];

            while (bb > 0)
            {
                int from = Utilities.BitScanForward(bb);

                ulong occupancy = ~Piece.Pieces_BB[Piece.NO_PIECE];
                occupancy &= MagicGeneration.MagicNumbers.RookAttacksInnerSixBits[from];
                occupancy *= MagicGeneration.MagicNumbers.RookMagicNumbers[from];
                occupancy >>= MagicGeneration.MagicNumbers.BitsRequiredByRook[from];
                ulong moves = MagicGeneration.MagicNumbers.RookAttacks[from][occupancy] & targetBitBoard;

                while (moves > 0)
                {
                    int to = Utilities.BitScanForward(moves);

                    moveList[ply][n].move = EncodeMove(from, to, moveType, piece, BoardDefs.Board[to], Piece.NO_PIECE);
                    n++;
                    moves = moves & moves - 1;

                }
                bb &= bb - 1;

            }

            return n;
        }

        public static int GenerateQueenMoves(int ply, int n, int piece, int moveType, ulong targetBitBoard)
        {
            ulong bb = Piece.Pieces_BB[piece];

            while (bb > 0)
            {
                int from = Utilities.BitScanForward(bb);

                ulong occupancyRook = ~Piece.Pieces_BB[Piece.NO_PIECE];
                occupancyRook &= MagicGeneration.MagicNumbers.RookAttacksInnerSixBits[from];
                occupancyRook *= MagicGeneration.MagicNumbers.RookMagicNumbers[from];
                occupancyRook >>= MagicGeneration.MagicNumbers.BitsRequiredByRook[from];
                ulong movesRook = MagicGeneration.MagicNumbers.RookAttacks[from][occupancyRook] & targetBitBoard;

                ulong occupancyBishop = ~Piece.Pieces_BB[Piece.NO_PIECE];
                occupancyBishop &= MagicGeneration.MagicNumbers.BishopAttacksInnerSixBits[from];
                occupancyBishop *= MagicGeneration.MagicNumbers.BishopMagicNumbers[from];
                occupancyBishop >>= MagicGeneration.MagicNumbers.BitsRequiredByBishop[from];
                ulong movesBishop = MagicGeneration.MagicNumbers.BishopAttacks[from][occupancyBishop] & targetBitBoard;

                ulong movesQueen = movesRook | movesBishop;

                while (movesQueen > 0)
                {
                    int to = Utilities.BitScanForward(movesQueen);

                    moveList[ply][n].move = EncodeMove(from, to, moveType, piece, BoardDefs.Board[to], Piece.NO_PIECE);
                    n++;
                    movesQueen = movesQueen & movesQueen - 1;

                }
                bb &= bb - 1;

            }

            return n;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long EncodeMove(int sqFrom, int sqTo, int moveType, int pieceFrom, int pieceTo, int piecePromotedTo)
        {
            // Move encoding: piecePromotedTo<<20|typeofMove<<17|pieceTo<<14|pieceFrom<<10|sqTo<<6|sqFrom.
            // sqFrom will require 6 bits. (0-63 squares)
            // sqTo will require 6 bits. (0-63 squares)
            // pieceFrom will require 4 bits. (0-12 piece types)
            // pieceTo will require 4 bits. (0-12 piece types)
            // typeOfMove will require 3 bits.(ordinary, captures, enpassant, short castle, long castle, promotion)
            // piece promoted will require 3 bits(N,B,R,Q).


            return (
                           (piecePromotedTo << MoveEncodingMasks.PROMOTED_PIECE_SHIFT) |
                           (moveType << MoveEncodingMasks.TYPE_OF_MOVE_SHIFT) |
                           (pieceTo << MoveEncodingMasks.PIECE_TO_SHIFT) |
                           (pieceFrom << MoveEncodingMasks.PIECE_FROM_SHIFT) |
                           (sqTo << MoveEncodingMasks.SQ_TO_SHIFT) |
                            sqFrom);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MakeMove(long move, int ply)
        {
            // Move encoding: piecePromotedTo<<20|typeofMove<<17|pieceTo<<14|pieceFrom<<10|sqTo<<6|sqFrom.
            // sqFrom will require 6 bits. (0-63 squares)
            // sqTo will require 6 bits. (0-63 squares)
            // pieceFrom will require 4 bits. (0-12 piece types)
            // pieceTo will require 4 bits. (0-12 piece types)
            // typeOfMove will require 3 bits.(ordinary, captures, enpassant, short castle, long castle, promotion)
            // piece promoted will require 3 bits(N,B,R,Q).

            int sqFrom = (int)(move & MoveEncodingMasks.SQ_FROM_MASK);
            int sqTo = (int)((move >> MoveEncodingMasks.SQ_TO_SHIFT) & MoveEncodingMasks.SQ_TO_MASK);
            int pieceFrom = (int)((move >> MoveEncodingMasks.PIECE_FROM_SHIFT) & MoveEncodingMasks.PIECE_FROM_MASK);
            int pieceTo = (int)((move >> MoveEncodingMasks.PIECE_TO_SHIFT) & MoveEncodingMasks.PIECE_TO_MASK);
            int moveType = (int)((move >> MoveEncodingMasks.TYPE_OF_MOVE_SHIFT) & MoveEncodingMasks.MOVE_TYPE_MASK);
            int promotedPiece = (int)((move >> MoveEncodingMasks.PROMOTED_PIECE_SHIFT) & MoveEncodingMasks.PROMOTED_PIECE_MASK);

            boardDefs.GameStateInformationPerPly[ply + 1].previous_enpassant_sq = boardDefs.GameStateInformationPerPly[ply + 1].current_enpassant_sq;
            boardDefs.GameStateInformationPerPly[ply + 1].current_white_castle_rights = boardDefs.GameStateInformationPerPly[ply].current_white_castle_rights;
            boardDefs.GameStateInformationPerPly[ply + 1].current_black_castle_rights = boardDefs.GameStateInformationPerPly[ply].current_black_castle_rights;

            boardDefs.GameStateInformationPerPly[ply + 1].current_enpassant_sq = 0;

            if (moveType == CommonDefs.MOVETYPE_PROMOTION)
            {
                // Update the board.
                BoardDefs.Board[sqFrom] = Piece.NO_PIECE;
                BoardDefs.Board[sqTo] = promotedPiece;

                // Update the bit boards.
                // Remove the set bit for the pieceFrom (sqFrom is now empty)
                Piece.Pieces_BB[pieceFrom] ^= (1UL << sqFrom);
                Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << sqFrom);

                Piece.Pieces_BB[promotedPiece] ^= (1UL << sqTo);
                Piece.Pieces_BB[pieceTo] ^= (1UL << sqTo);

            }

            else if (moveType == CommonDefs.MOVETYPE_KING_CASTLE)
            {
                if (pieceFrom == Piece.WK)
                {
                    // Update the board.
                    BoardDefs.Board[Square.e1] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.g1] = Piece.WK;
                    BoardDefs.Board[Square.h1] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.f1] = Piece.WR;

                    // Update the bit boards.
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << Square.e1) ^ (1UL << Square.h1) ^ (1UL << Square.g1) ^ (1UL << Square.f1);
                    Piece.Pieces_BB[Piece.WK] ^= (1UL << Square.e1) ^ (1UL << Square.g1);
                    Piece.Pieces_BB[Piece.WR] ^= (1UL << Square.h1) ^ (1UL << Square.f1);

                    // update the castle permissions.
                    boardDefs.GameStateInformationPerPly[ply + 1].current_white_castle_rights = 0;

                }
                else
                {
                    // Update the board.
                    BoardDefs.Board[Square.e8] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.g8] = Piece.BK;
                    BoardDefs.Board[Square.h8] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.f8] = Piece.BR;

                    // Update the bit boards.
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << Square.e8) ^ (1UL << Square.h8) ^ (1UL << Square.g8) ^ (1UL << Square.f8);
                    Piece.Pieces_BB[Piece.BK] ^= (1UL << Square.e8) ^ (1UL << Square.g8);
                    Piece.Pieces_BB[Piece.BR] ^= (1UL << Square.h8) ^ (1UL << Square.f8);

                    // update the castle permissions.
                    boardDefs.GameStateInformationPerPly[ply + 1].current_black_castle_rights = 0;

                }


            }

            else if (moveType == CommonDefs.MOVETYPE_QUEEN_CASTLE)
            {
                if (pieceFrom == Piece.WK)
                {
                    // Update the board.
                    BoardDefs.Board[Square.e1] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.c1] = Piece.WK;
                    BoardDefs.Board[Square.a1] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.d1] = Piece.WR;

                    // Update the bit boards.
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << Square.e1) ^ (1UL << Square.c1) ^ (1UL << Square.a1) ^ (1UL << Square.d1);
                    Piece.Pieces_BB[Piece.WK] ^= (1UL << Square.e1) ^ (1UL << Square.c1);
                    Piece.Pieces_BB[Piece.WR] ^= (1UL << Square.a1) ^ (1UL << Square.d1);

                    // update the castle permissions.
                    boardDefs.GameStateInformationPerPly[ply + 1].current_white_castle_rights = 0;
                }
                else
                {
                    // Update the board.
                    BoardDefs.Board[Square.e8] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.c8] = Piece.BK;
                    BoardDefs.Board[Square.a8] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.d8] = Piece.BR;

                    // Update the bit boards.
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << Square.e8) ^ (1UL << Square.c8) ^ (1UL << Square.d8) ^ (1UL << Square.a8);
                    Piece.Pieces_BB[Piece.BK] ^= (1UL << Square.e8) ^ (1UL << Square.c8);
                    Piece.Pieces_BB[Piece.BR] ^= (1UL << Square.a8) ^ (1UL << Square.d8);

                    // update the castle permissions.
                    boardDefs.GameStateInformationPerPly[ply + 1].current_black_castle_rights = 0;
                }

            }

            else if (moveType == CommonDefs.MOVETYPE_ENPASSANT)
            {
                // Check for fromsquare and remove the white pawn from the board and bitboard.
                BoardDefs.Board[sqFrom] = Piece.NO_PIECE;
                BoardDefs.Board[sqTo] = pieceFrom;

                Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << sqFrom);
                Piece.Pieces_BB[pieceFrom] ^= (1UL << sqFrom) ^ (1UL << sqTo);
                Piece.Pieces_BB[pieceTo] ^= (1UL << sqTo);

                // if the pawn is white, then the captured piece is black pawn, else the captured piece is white pawn.
                if (pieceFrom == Piece.WP)
                {
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << (sqTo + DirectionMasks.South));
                    BoardDefs.Board[sqTo + DirectionMasks.South] = Piece.NO_PIECE;
                    Piece.Pieces_BB[Piece.BP] ^= (1UL << (sqTo + DirectionMasks.South));

                }
                else
                {
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << (sqTo + DirectionMasks.North));
                    BoardDefs.Board[sqTo + DirectionMasks.North] = Piece.NO_PIECE;
                    Piece.Pieces_BB[Piece.WP] ^= (1UL << (sqTo + DirectionMasks.North));
                }

            }

            else
            {
                BoardDefs.Board[sqFrom] = Piece.NO_PIECE;
                BoardDefs.Board[sqTo] = pieceFrom;

                Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << sqFrom);
                Piece.Pieces_BB[pieceFrom] ^= (1UL << sqFrom) ^ (1UL << sqTo);
                Piece.Pieces_BB[pieceTo] ^= (1UL << sqTo);

                // Update the game information:
                // Make copy of previous information:

                if (Math.Abs(sqFrom - sqTo) == 2 * DirectionMasks.North)
                {
                    if (pieceFrom == Piece.WP && (AttackMaps.WHITE_PAWN_ATTACK_MASK[sqTo - DirectionMasks.North] & Piece.Pieces_BB[Piece.BP]) > 0)
                    {
                        boardDefs.GameStateInformationPerPly[ply + 1].current_enpassant_sq = sqTo - DirectionMasks.North;
                    }
                    else if (pieceFrom == Piece.BP && (AttackMaps.BLACK_PAWN_ATTACK_MASK[sqTo - DirectionMasks.South] & Piece.Pieces_BB[Piece.WP]) > 0)
                    {
                        boardDefs.GameStateInformationPerPly[ply + 1].current_enpassant_sq = sqTo - DirectionMasks.South;
                    }
                }

                // If the king is moving then update the current castle rights to 0.
                if (pieceFrom == Piece.WK)
                {
                    boardDefs.GameStateInformationPerPly[ply + 1].current_white_castle_rights = 0;
                }

                else if (pieceFrom == Piece.BK)
                {
                    boardDefs.GameStateInformationPerPly[ply + 1].current_black_castle_rights = 0;
                }

            }

            // If the rook is moving, check and update the castle rights.
            if ((sqFrom == Square.h1) || (sqTo == Square.h1))
            {
                boardDefs.GameStateInformationPerPly[ply + 1].current_white_castle_rights &= ~1;
            }
            if ((sqFrom == Square.a1) || (sqTo == Square.a1))
            {
                boardDefs.GameStateInformationPerPly[ply + 1].current_white_castle_rights &= ~2;
            }
            if ((sqFrom == Square.h8) || (sqTo == Square.h8))
            {
                boardDefs.GameStateInformationPerPly[ply + 1].current_black_castle_rights &= ~4;
            }
            if ((sqFrom == Square.a8) || (sqTo == Square.a8))
            {
                boardDefs.GameStateInformationPerPly[ply + 1].current_black_castle_rights &= ~8;
            }

            // PerftManager.DebugAssertions("After Make Move in generate moves", move);

        }

        public void UndoMove(long move, int ply)
        {
            int sqFrom = (int)(move & MoveEncodingMasks.SQ_FROM_MASK);
            int sqTo = (int)((move >> MoveEncodingMasks.SQ_TO_SHIFT) & MoveEncodingMasks.SQ_TO_MASK);
            int pieceFrom = (int)((move >> MoveEncodingMasks.PIECE_FROM_SHIFT) & MoveEncodingMasks.PIECE_FROM_MASK);
            int pieceTo = (int)((move >> MoveEncodingMasks.PIECE_TO_SHIFT) & MoveEncodingMasks.PIECE_TO_MASK);
            int moveType = (int)((move >> MoveEncodingMasks.TYPE_OF_MOVE_SHIFT) & MoveEncodingMasks.MOVE_TYPE_MASK);
            int promotedPiece = (int)((move >> MoveEncodingMasks.PROMOTED_PIECE_SHIFT) & MoveEncodingMasks.PROMOTED_PIECE_MASK);

            if (moveType == CommonDefs.MOVETYPE_PROMOTION)
            {
                // Update the board.
                BoardDefs.Board[sqFrom] = pieceFrom;
                BoardDefs.Board[sqTo] = pieceTo;

                // Update the bit boards.
                // Remove the set bit for the pieceFrom (sqFrom is now empty)
                Piece.Pieces_BB[pieceFrom] ^= (1UL << sqFrom);
                Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << sqFrom);

                Piece.Pieces_BB[promotedPiece] ^= (1UL << sqTo);
                Piece.Pieces_BB[pieceTo] ^= (1UL << sqTo);

            }

            else if (moveType == CommonDefs.MOVETYPE_KING_CASTLE)
            {
                if (pieceFrom == Piece.WK)
                {
                    // Update the board.
                    BoardDefs.Board[Square.e1] = Piece.WK;
                    BoardDefs.Board[Square.g1] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.h1] = Piece.WR;
                    BoardDefs.Board[Square.f1] = Piece.NO_PIECE;

                    // Update the bit boards.
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << Square.e1) ^ (1UL << Square.h1) ^ (1UL << Square.g1) ^ (1UL << Square.f1);
                    Piece.Pieces_BB[Piece.WK] ^= (1UL << Square.e1) ^ (1UL << Square.g1);
                    Piece.Pieces_BB[Piece.WR] ^= (1UL << Square.h1) ^ (1UL << Square.f1);

                    // update the castle permissions.
                    boardDefs.GameStateInformationPerPly[ply + 1].current_white_castle_rights = boardDefs.GameStateInformationPerPly[ply + 1].previous_white_castle_rights;

                }

                else if (pieceFrom == Piece.BK)
                {
                    // Update the board.
                    BoardDefs.Board[Square.e8] = Piece.BK;
                    BoardDefs.Board[Square.g8] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.h8] = Piece.BR;
                    BoardDefs.Board[Square.f8] = Piece.NO_PIECE;

                    // Update the bit boards.
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << Square.e8) ^ (1UL << Square.h8) ^ (1UL << Square.g8) ^ (1UL << Square.f8);
                    Piece.Pieces_BB[Piece.BK] ^= (1UL << Square.e8) ^ (1UL << Square.g8);
                    Piece.Pieces_BB[Piece.BR] ^= (1UL << Square.h8) ^ (1UL << Square.f8);

                    // update the castle permissions.
                    boardDefs.GameStateInformationPerPly[ply + 1].current_black_castle_rights = boardDefs.GameStateInformationPerPly[ply + 1].previous_black_castle_rights;
                }
            }

            else if (moveType == CommonDefs.MOVETYPE_QUEEN_CASTLE)
            {
                if (pieceFrom == Piece.WK)
                {
                    // Update the board.
                    BoardDefs.Board[Square.e1] = Piece.WK;
                    BoardDefs.Board[Square.c1] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.a1] = Piece.WR;
                    BoardDefs.Board[Square.d1] = Piece.NO_PIECE;

                    // Update the bit boards.
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << Square.a1) ^ (1UL << Square.c1) ^ (1UL << Square.d1) ^ (1UL << Square.e1);
                    Piece.Pieces_BB[Piece.WK] ^= (1UL << Square.e1) ^ (1UL << Square.c1);
                    Piece.Pieces_BB[Piece.WR] ^= (1UL << Square.a1) ^ (1UL << Square.d1);

                    // update the castle permissions.
                    boardDefs.GameStateInformationPerPly[ply + 1].current_white_castle_rights = boardDefs.GameStateInformationPerPly[ply + 1].previous_white_castle_rights;
                }

                else if (pieceFrom == Piece.BK)
                {
                    // Update the board.
                    BoardDefs.Board[Square.e8] = Piece.BK;
                    BoardDefs.Board[Square.c8] = Piece.NO_PIECE;
                    BoardDefs.Board[Square.a8] = Piece.BR;
                    BoardDefs.Board[Square.d8] = Piece.NO_PIECE;

                    // Update the bit boards.
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << Square.a8) ^ (1UL << Square.c8) ^ (1UL << Square.d8) ^ (1UL << Square.e8);
                    Piece.Pieces_BB[Piece.BK] ^= (1UL << Square.e8) ^ (1UL << Square.c8);
                    Piece.Pieces_BB[Piece.BR] ^= (1UL << Square.a8) ^ (1UL << Square.d8);

                    // update the castle permissions.
                    boardDefs.GameStateInformationPerPly[ply + 1].current_black_castle_rights = boardDefs.GameStateInformationPerPly[ply + 1].previous_black_castle_rights;

                }


            }

            else if (moveType == CommonDefs.MOVETYPE_ENPASSANT)
            {
                // Check for fromsquare and remove the white pawn from the board and bitboard.
                BoardDefs.Board[sqFrom] = pieceFrom;
                BoardDefs.Board[sqTo] = Piece.NO_PIECE;

                Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << sqFrom);
                Piece.Pieces_BB[pieceFrom] ^= (1UL << sqFrom) ^ (1UL << sqTo);
                Piece.Pieces_BB[pieceTo] ^= (1UL << sqTo);

                // if the pawn is white, then the captured piece is black pawn, else the captured piece is white pawn.
                if (pieceFrom == Piece.WP)
                {
                    Piece.Pieces_BB[Piece.BP] ^= (1UL << (sqTo + DirectionMasks.South));
                    BoardDefs.Board[sqTo + DirectionMasks.South] = Piece.BP;
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << (sqTo + DirectionMasks.South));
                }
                else
                {
                    Piece.Pieces_BB[Piece.WP] ^= (1UL << (sqTo + DirectionMasks.North));
                    BoardDefs.Board[sqTo + DirectionMasks.North] = Piece.WP;
                    Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << (sqTo + DirectionMasks.North));
                }
            }

            else
            {
                BoardDefs.Board[sqFrom] = pieceFrom;
                BoardDefs.Board[sqTo] = pieceTo;

                Piece.Pieces_BB[Piece.NO_PIECE] ^= (1UL << sqFrom);
                Piece.Pieces_BB[pieceFrom] ^= (1UL << sqFrom) ^ (1UL << sqTo);
                Piece.Pieces_BB[pieceTo] ^= (1UL << sqTo);

            }

            boardDefs.GameStateInformationPerPly[ply + 1].current_white_castle_rights = boardDefs.GameStateInformationPerPly[ply + 1].previous_white_castle_rights;
            boardDefs.GameStateInformationPerPly[ply + 1].current_black_castle_rights = boardDefs.GameStateInformationPerPly[ply + 1].previous_black_castle_rights;
            boardDefs.GameStateInformationPerPly[ply + 1].current_enpassant_sq = boardDefs.GameStateInformationPerPly[ply + 1].previous_enpassant_sq;
        }


        public int GenerateAllMoves(int ply, bool turn, int n)
        {
            if (turn)
            {
                ulong opponentPieces = Piece.Pieces_BB[Piece.BP] | Piece.Pieces_BB[Piece.BN] | Piece.Pieces_BB[Piece.BB] | Piece.Pieces_BB[Piece.BR] | Piece.Pieces_BB[Piece.BQ];
                ulong notOurPieces = Piece.Pieces_BB[Piece.NO_PIECE] | opponentPieces;

                n = GenerateWhitePawnNonCapture(ply, n);
                n = GenerateWhitePawnCapture(ply, n, opponentPieces);
                n = GenerateKnightMoves(ply, n, Piece.WN, CommonDefs.MOVETYPE_ORDINARY, notOurPieces);
                n = GenerateKingMoves(ply, n, Piece.WK, CommonDefs.MOVETYPE_ORDINARY, notOurPieces);
                n = GenerateCastleMoves(n, ply, Piece.WK);
                n = GenerateBishopMoves(ply, n, Piece.WB, CommonDefs.MOVETYPE_ORDINARY, notOurPieces);
                n = GenerateRookMoves(ply, n, Piece.WR, CommonDefs.MOVETYPE_ORDINARY, notOurPieces);
                n = GenerateQueenMoves(ply, n, Piece.WQ, CommonDefs.MOVETYPE_ORDINARY, notOurPieces);
            }
            else
            {
                ulong opponentPieces = Piece.Pieces_BB[Piece.WP] | Piece.Pieces_BB[Piece.WN] | Piece.Pieces_BB[Piece.WB] | Piece.Pieces_BB[Piece.WR] | Piece.Pieces_BB[Piece.WQ];
                ulong notOurPieces = Piece.Pieces_BB[Piece.NO_PIECE] | opponentPieces;

                n = GenerateBlackPawnNonCapture(ply, n);
                n = GenerateBlackPawnCapture(ply, n, opponentPieces);
                n = GenerateKnightMoves(ply, n, Piece.BN, CommonDefs.MOVETYPE_CAPTURE, notOurPieces);
                n = GenerateKingMoves(ply, n, Piece.BK, CommonDefs.MOVETYPE_CAPTURE, notOurPieces);
                n = GenerateCastleMoves(n, ply, Piece.BK);
                n = GenerateBishopMoves(ply, n, Piece.BB, CommonDefs.MOVETYPE_CAPTURE, notOurPieces);
                n = GenerateRookMoves(ply, n, Piece.BR, CommonDefs.MOVETYPE_CAPTURE, notOurPieces);
                n = GenerateQueenMoves(ply, n, Piece.BQ, CommonDefs.MOVETYPE_CAPTURE, notOurPieces);
            }

            MOVES_AT_PLY[ply] = n;
            return n;
        }

    }
}

