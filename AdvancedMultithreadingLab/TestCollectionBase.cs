using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;
using System;
using System.Linq;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    [HtmlExporter]
    [MemoryDiagnoser]
    public abstract class TestCollectionBase : IDisposable
    {
        public const int Iterations = 1000000;
        public ManualResetEventSlim startEvent = new ManualResetEventSlim();
        Barrier startBarrier, completedBarrier;
        Thread[] allThreads;
        volatile bool disposed;

        public TestCollectionBase( )
        {
        }

        [Params(1,2)]
        public int Threads { get; set; }

        [Setup]
        public void Setup()
        {
            Thread[] producerThreads = Enumerable.Range( 0, this.Threads ).Select( i => new Thread( this.ProducerThread ) ).ToArray();
            Thread[] consumerThreads = Enumerable.Range( 0, this.Threads ).Select( i => new Thread( this.ConsumerThread ) ).ToArray();
            this.allThreads = producerThreads.Union( consumerThreads ).ToArray();

            startBarrier = new Barrier( allThreads.Length + 1 );
            completedBarrier = new Barrier( allThreads.Length + 1 );

            foreach ( var t in allThreads ) t.Start();
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public void RunTests()
        {
            startBarrier.SignalAndWait();

            completedBarrier.SignalAndWait();
        }

        protected abstract void AddItems( int count );
        protected abstract void ConsumeItems( int count );

        private void ProducerThread()
        {
            while ( true )
            {
                this.startBarrier.SignalAndWait();
                if ( this.disposed )  break;
                this.AddItems( Iterations );
                this.completedBarrier.SignalAndWait();
            }
        }

        private void ConsumerThread()
        {
            while ( true )
            {
                this.startBarrier.SignalAndWait();
                if ( this.disposed ) break;
                this.ConsumeItems( Iterations );
                this.completedBarrier.SignalAndWait();
            }
        }

        [Cleanup]
        public void Dispose()
        {
            this.disposed = true;
            this.startBarrier.SignalAndWait();
        }
    }
}