namespace HanselChessBOT.ConsoleApp
{

    public class PerftManager
    {

        static int ply = 0;
        public ulong Perft(int depth, bool turn, ref MoveGeneration moveGen)
        {

            int n = 0;
            ulong nodes = 0;
            if (depth <= 0)
            {
                return 1;
            }

            moveGen.GenerateAllMoves(ply, turn, n);

            for (int i = 0; i < MoveGeneration.MOVES_AT_PLY[ply]; i++)
            {
                ulong nodes_divide = 0UL;
                Move m = MoveGeneration.moveList[ply][i];
                moveGen.MakeMove(m.move, ply);
                ply++;
                if (LegalMoveManager.IsKingInCheck(turn))
                {
                    moveGen.UndoMove(m.move, ply);
                    ply--;
                    continue;
                }
                //IO.PrintMove(m.move);
                nodes_divide = Divide(depth - 1, !turn, ref moveGen);
                nodes += nodes_divide;
                //IO.PrintMoveWithNodes(m.move, nodes_divide);
                moveGen.UndoMove(m.move, ply);

                ply--;

            }
            return nodes;
        }

        public ulong Divide(int depth, bool turn, ref MoveGeneration moveGen)
        {
            int n = 0;
            ulong nodes = 0;


            if (depth <= 0)
            {
                return 1;
            }

            moveGen.GenerateAllMoves(ply, turn, n);

            for (int i = 0; i < MoveGeneration.MOVES_AT_PLY[ply]; i++)
            {
                ulong nodes_divide = 0UL;
                Move m = MoveGeneration.moveList[ply][i];
                moveGen.MakeMove(m.move, ply);

                ply++;
                if (LegalMoveManager.IsKingInCheck(turn))
                {
                    moveGen.UndoMove(m.move, ply);
                    ply--;
                    continue;
                }


                nodes_divide = Divide2(depth - 1, !turn, ref moveGen);
                nodes += nodes_divide;
                //Console.Write("\t",m.move);
                //IO.PrintMoveWithNodes(m.move, nodes_divide);
                moveGen.UndoMove(m.move, ply);
                ply--;

            }
            return nodes;
        }

        public ulong Divide2(int depth, bool turn, ref MoveGeneration moveGen)
        {
            int n = 0;
            ulong nodes = 0;
            if (depth <= 0)
            {
                return 1;
            }


            moveGen.GenerateAllMoves(ply, turn, n);

            for (int i = 0; i < MoveGeneration.MOVES_AT_PLY[ply]; i++)
            {
                ulong nodes_divide = 0UL;
                Move m = MoveGeneration.moveList[ply][i];

                moveGen.MakeMove(m.move, ply);

                ply++;
                if (LegalMoveManager.IsKingInCheck(turn))
                {
                    moveGen.UndoMove(m.move, ply);
                    ply--;
                    continue;
                }

                nodes_divide = Divide3(depth - 1, !turn, ref moveGen);
                nodes += nodes_divide;
                //Console.Write("\t\t",m.move);
                //IO.PrintMoveWithNodes(m.move, nodes_divide);

                moveGen.UndoMove(m.move, ply);
                ply--;

            }
            return nodes;
        }

        public ulong Divide3(int depth, bool turn, ref MoveGeneration moveGen)
        {
            int n = 0;
            ulong nodes = 0;
            if (depth <= 0)
            {
                return 1;
            }

            moveGen.GenerateAllMoves(ply, turn, n);

            for (int i = 0; i < MoveGeneration.MOVES_AT_PLY[ply]; i++)
            {
                ulong nodes_divide = 0UL;
                Move m = MoveGeneration.moveList[ply][i];

                moveGen.MakeMove(m.move, ply);

                ply++;
                if (LegalMoveManager.IsKingInCheck(turn))
                {
                    moveGen.UndoMove(m.move, ply);
                    ply--;
                    continue;
                }

                nodes_divide = Divide3(depth - 1, !turn, ref moveGen);
                nodes += nodes_divide;

                //Console.WriteLine("\t\t\t",m.move);
                //IO.PrintMoveWithNodes(m.move, nodes_divide);
                moveGen.UndoMove(m.move, ply);

                ply--;

            }
            return nodes;
        }
    }
}
