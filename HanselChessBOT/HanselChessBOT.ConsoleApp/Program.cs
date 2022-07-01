using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace HanselChessBOT.ConsoleApp
{
    public class Program
    {
        public static ulong totalNodes = 0UL;
        public static ulong totalTime = 0UL;
        static void Main(string[] args)
        {
            BoardDefs.InitOnce();
            string fen = @"r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";

            Debug(5, fen);

            Console.WriteLine("Avg:");
            Console.WriteLine("total nodes : {0}\t total time : {1}\t nps : {2}", totalNodes, totalTime, 1000 * (totalNodes / totalTime));
        }

        public static void Debug(int depth, string fenPos)
        {
            Stopwatch sw = new Stopwatch();
            BoardDefs.InitOnce();

            IO.SetPosition(fenPos);
            sw.Start();
            ulong nodesActual = PerftManager.Perft(depth, CurrentStateInformation.turn);
            sw.Stop();
            Console.WriteLine("depth : {0}\t nodes : {1}\t time : {2}\t nps : {3}", depth, nodesActual, sw.ElapsedMilliseconds, 1000 * (nodesActual / (ulong)sw.ElapsedMilliseconds));

            totalNodes += nodesActual;
            totalTime += (ulong)sw.ElapsedMilliseconds;

        }
    }
}

