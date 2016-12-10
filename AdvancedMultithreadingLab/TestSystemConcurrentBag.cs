using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    internal class TestSystemConcurrentBag
    {
        private const int n = 50000000;
        private readonly ConcurrentBag<int> stack = new ConcurrentBag<int>();

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

            Console.WriteLine( "SystemConcurrentQueue: {0:0.0} MT/s ({1:0} ns/T)", 1e-6*n*Stopwatch.Frequency/stopwatch.ElapsedTicks,
                               1e9/((double) n*Stopwatch.Frequency/stopwatch.ElapsedTicks) );
        }

        private void ThreadPush()
        {
            for ( int i = 0; i < n; i++ )
            {
                this.stack.Add( i );
            }
        }

        private void ThreadPop()
        {
            SpinWait spinWait = new SpinWait();

            int value;

            for ( int i = 0; i < n; )
            {
                if ( this.stack.TryTake( out value ) )
                {
                    i++;

                    spinWait.Reset();
                }
                else
                {
                    spinWait.SpinOnce();
                }
            }

            if ( this.stack.TryTake( out value ))
            {
                throw new Exception( "Data structure corrupted." );
            }
        }
    }
}