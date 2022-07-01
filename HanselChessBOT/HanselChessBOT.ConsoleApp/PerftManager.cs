using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanselChessBOT.ConsoleApp
{

    public static class PerftManager
    {

        static int ply = 0;
        public static ulong Perft(int depth, bool turn)
        {

            int n = 0;
            ulong nodes = 0;
            if (depth <= 0)
            {
                return 1;
            }

            MoveGeneration.GenerateAllMoves(ply, turn, n);

            for (int i = 0; i < MoveGeneration.MOVES_AT_PLY[ply]; i++)
            {
                ulong nodes_divide = 0UL;
                Move m = MoveGeneration.moveList[MoveGeneration.MAX_PLY * ply + i];
                MoveGeneration.MakeMove(m.move, ply);
                ply++;
                if (LegalMoveManager.IsKingInCheck(turn))
                {
                    MoveGeneration.UndoMove(m.move, ply);
                    ply--;
                    continue;
                }
                //IO.PrintMove(m.move);
                nodes_divide = Divide(depth - 1, !turn);
                nodes += nodes_divide;
                //IO.PrintMoveWithNodes(m.move, nodes_divide);
                MoveGeneration.UndoMove(m.move, ply);

                ply--;

            }
            return nodes;
        }

        public static ulong Divide(int depth, bool turn)
        {
            int n = 0;
            ulong nodes = 0;


            if (depth <= 0)
            {
                return 1;
            }

            MoveGeneration.GenerateAllMoves(ply, turn, n);

            for (int i = 0; i < MoveGeneration.MOVES_AT_PLY[ply]; i++)
            {
                ulong nodes_divide = 0UL;
                Move m = MoveGeneration.moveList[MoveGeneration.MAX_PLY * ply + i];
                MoveGeneration.MakeMove(m.move, ply);

                ply++;
                if (LegalMoveManager.IsKingInCheck(turn))
                {
                    MoveGeneration.UndoMove(m.move, ply);
                    ply--;
                    continue;
                }


                nodes_divide = Divide2(depth - 1, !turn);
                nodes += nodes_divide;
                //Console.Write("\t",m.move);
                //IO.PrintMoveWithNodes(m.move, nodes_divide);
                MoveGeneration.UndoMove(m.move, ply);
                ply--;

            }
            return nodes;
        }

        public static ulong Divide2(int depth, bool turn)
        {
            int n = 0;
            ulong nodes = 0;
            if (depth <= 0)
            {
                return 1;
            }


            MoveGeneration.GenerateAllMoves(ply, turn, n);

            for (int i = 0; i < MoveGeneration.MOVES_AT_PLY[ply]; i++)
            {
                ulong nodes_divide = 0UL;
                Move m = MoveGeneration.moveList[MoveGeneration.MAX_PLY * ply + i];

                MoveGeneration.MakeMove(m.move, ply);

                ply++;
                if (LegalMoveManager.IsKingInCheck(turn))
                {
                    MoveGeneration.UndoMove(m.move, ply);
                    ply--;
                    continue;
                }

                nodes_divide = Divide3(depth - 1, !turn);
                nodes += nodes_divide;
                //Console.Write("\t\t",m.move);
                //IO.PrintMoveWithNodes(m.move, nodes_divide);

                MoveGeneration.UndoMove(m.move, ply);
                ply--;

            }
            return nodes;
        }

        public static ulong Divide3(int depth, bool turn)
        {
            int n = 0;
            ulong nodes = 0;
            if (depth <= 0)
            {
                return 1;
            }

            MoveGeneration.GenerateAllMoves(ply, turn, n);

            for (int i = 0; i < MoveGeneration.MOVES_AT_PLY[ply]; i++)
            {
                ulong nodes_divide = 0UL;
                Move m = MoveGeneration.moveList[MoveGeneration.MAX_PLY * ply + i];

                MoveGeneration.MakeMove(m.move, ply);

                ply++;
                if (LegalMoveManager.IsKingInCheck(turn))
                {
                    MoveGeneration.UndoMove(m.move, ply);
                    ply--;
                    continue;
                }

                nodes_divide = Divide3(depth - 1, !turn);
                nodes += nodes_divide;

                //Console.WriteLine("\t\t\t",m.move);
                //IO.PrintMoveWithNodes(m.move, nodes_divide);
                MoveGeneration.UndoMove(m.move, ply);

                ply--;

            }
            return nodes;
        }
    }
}
