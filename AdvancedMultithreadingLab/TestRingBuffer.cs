using System;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    internal class TestRingBuffer
    {
        private const int n = 50000000;
        private readonly RingBuffer<int> buffer = new RingBuffer<int>( 1024*64 );

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

            Console.WriteLine( "RingBuffer: {0:0.0} MT/s ({1:0} ns/T)", 1e-6*n*Stopwatch.Frequency/stopwatch.ElapsedTicks,
                               1e9/((double) n*Stopwatch.Frequency/stopwatch.ElapsedTicks) );
        }

        private void ThreadPush()
        {
            for ( int i = 0; i < n; i++ )
            {
                while ( !this.buffer.TryAdd( i ) )
                {
                }
            }
        }

        private void ThreadPop()
        {
            for ( int i = 0; i < n; )
            {
                int value;
                if ( this.buffer.TryTake( out value ) )
                    i++;
            }
        }
    }
}