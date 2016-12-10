using System;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    internal class Program
    {
        private static void Main( string[] args )
        {

            //MiscBenchmarks.ExecuteAll();

            new TestTrivialConcurrentStack().Start();
            Thread.Sleep( 5000 ); // Separate peaks in CPU monitor.

            new TestSystemConcurrentStack().Start();
            Thread.Sleep( 5000 ); // Separate peaks in CPU monitor.

            new TestSystemConcurrentQueue().Start();
            Thread.Sleep( 5000 ); // Separate peaks in CPU monitor.

            new TestSystemConcurrentBag().Start();
            Thread.Sleep( 5000 ); // Separate peaks in CPU monitor.


            new TestBlockingCollection().Start();

            //new TestInterlocked().Start();
            
            Console.WriteLine("Done.");
        }
    }
}