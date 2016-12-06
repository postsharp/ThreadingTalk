using System;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    internal class TestConcurrentStack
    {
        private const int n = 50000000;
        private readonly ConcurrentStack<int> stack = new ConcurrentStack<int>();

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

            Console.WriteLine( "ConcurrentStack: {0:0.0} MT/s ({1:0} ns/T)", 1e-6*n*Stopwatch.Frequency/stopwatch.ElapsedTicks,
                               1e9/((double) n*Stopwatch.Frequency/stopwatch.ElapsedTicks) );
        }

        private void ThreadPush()
        {
            for ( int i = 0; i < n; i++ )
            {
                this.stack.Push( i );
            }
        }

        private void ThreadPop()
        {
            for ( int i = 0; i < n; )
            {
                int value;
                if ( this.stack.TryPop( out value ) )
                    i++;
            }
        }
    }
}