using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    internal class TestBlockingCollection
    {
        private const int n = 5000000;
        private readonly BlockingCollection<int> collection = new BlockingCollection<int>();

        public void Start()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Thread threadPush = new Thread( this.ThreadPush );
            Thread threadPop = new Thread( this.ThreadPop );

            stopwatch.Start();

            threadPush.Start();
            threadPop.Start();

            threadPush.Join();
            threadPop.Join();

            GC.Collect();

            Console.WriteLine( "BlockingCollection: {0:0.0} MT/s ({1:0} ns/T)", 1e-6*n*Stopwatch.Frequency/stopwatch.ElapsedTicks,
                               1e9/((double) n*Stopwatch.Frequency/stopwatch.ElapsedTicks) );
        }

        private void ThreadPush()
        {
            for ( int i = 0; i < n; i++ )
            {
                this.collection.Add( i );
            }
            this.collection.CompleteAdding();
        }

        private void ThreadPop()
        {
            foreach (int i in this.collection.GetConsumingEnumerable())
            {

            }
        }
    }
}