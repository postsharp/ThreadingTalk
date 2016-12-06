using System;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            //Freezer.Main();
          // new TestReaderWriterLock<ManuallySynchronizedOrder>().Test();

            //MiscBenchmarks.ExecuteAll();
            new TestConcurrentStack().Start();
            Thread.Sleep( 5000 ); // Separate peaks in CPU monitor.
            new TestRingBuffer().Start();
            Thread.Sleep( 5000 ); // Separate peaks in CPU monitor.
            new TestBlockingCollection().Start();

            //new TestInterlocked().Start();
            
            Console.WriteLine("Done.");
        }
    }
}