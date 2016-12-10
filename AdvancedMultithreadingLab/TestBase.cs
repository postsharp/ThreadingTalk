using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace AdvancedMultithreadingLab
{

    internal abstract class TestCollectionBase
    {
        private int n;
        public ManualResetEventSlim startEvent = new ManualResetEventSlim();
        Barrier startBarrier;

        public TestCollectionBase(int slowFactor = 1)
        {
            n = 100000000 / slowFactor;
        }

        public void Start(int threads = 2)
        {
            this.AddItems( 10 );
            this.ConsumeItems( 10 );

            Stopwatch stopwatch = Stopwatch.StartNew();

            Thread[] producerThreads = Enumerable.Range(0,threads).Select( i => new Thread( this.ProducerThread ) ).ToArray();
            Thread[] consumerThreads = Enumerable.Range( 0, threads ).Select( i => new Thread( this.ConsumerThread) ).ToArray();
            Thread[] allThreads = producerThreads.Union( consumerThreads ).ToArray();

            startBarrier = new Barrier( allThreads.Length + 1 );

            foreach ( var t in allThreads ) t.Start();

            startBarrier.SignalAndWait();
            stopwatch.Start();


            foreach ( var t in allThreads ) t.Join();

            GC.Collect();
            stopwatch.Stop();

            Console.WriteLine( this.GetType().Name + ": {0:0.0} MT/s ({1:0} ns/T)", 1e-6*n*Stopwatch.Frequency/stopwatch.ElapsedTicks,
                               1e9/((double) n*Stopwatch.Frequency/stopwatch.ElapsedTicks) );
        }

        protected abstract void AddItems( int count );
        protected abstract void ConsumeItems( int count );

        protected virtual void ProducerThread()
        {
            this.startBarrier.SignalAndWait();
            this.AddItems( n );
        }

        protected virtual void ConsumerThread()
        {
            this.startBarrier.SignalAndWait();
            this.ConsumeItems( n );
        }
    }
}