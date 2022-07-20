using System.Diagnostics;

namespace HanselChessBOT.ConsoleApp
{
    public class Program
    {
        public static ulong totalNodes = 0UL;
        public static ulong totalTime = 0UL;
        FenManager fenManager = new FenManager();
        IO inOut = new();
        PerftManager perftManager = new();

        static void Main(string[] args)
        {
            Program p = new Program();
            BoardDefs boardDefs = new BoardDefs();
            boardDefs.InitOnce();
            string fen = @"r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";

            p.Debug(5, fen, ref boardDefs);

            Console.WriteLine("Avg:");
            Console.WriteLine("total nodes : {0}\t total time : {1}\t nps : {2}", totalNodes, totalTime, 1000 * (totalNodes / totalTime));
        }

        public static void CreateInstancesOnlyOnce()
        {

        }
        public void Debug(int depth, string fenPos, ref BoardDefs boardDefs)
        {
            Stopwatch sw = new Stopwatch();
            MoveGeneration moveGeneration = new MoveGeneration(boardDefs);
            boardDefs.InitOnce();

            inOut.SetPosition(fenPos, ref boardDefs, fenManager);
            sw.Start();
            ulong nodesActual = perftManager.Perft(depth, CurrentStateInformation.turn, ref moveGeneration);
            sw.Stop();
            Console.WriteLine("depth : {0}\t nodes : {1}\t time : {2}\t nps : {3}", depth, nodesActual, sw.ElapsedMilliseconds, 1000 * (nodesActual / (ulong)sw.ElapsedMilliseconds));

            totalNodes += nodesActual;
            totalTime += (ulong)sw.ElapsedMilliseconds;

        }
    }
}

