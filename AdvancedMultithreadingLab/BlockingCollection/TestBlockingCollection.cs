#region Copyright (c) 2012 by SharpCrafters s.r.o.

// Copyright (c) 2012, SharpCrafters s.r.o.
// All rights reserved.
// 
// For licensing terms, see file License.txt

#endregion

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab.BlockingCollection
{
    internal class TestBlockingCollection
    {
        private const int n = 2000000;
        private readonly BlockingCollection<int> stack = new BlockingCollection<int>();

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

            Console.WriteLine( "BlockingCollection: {0:0.0} MT/s ({1:0} ns/T)", 1e-6*n*Stopwatch.Frequency/stopwatch.ElapsedTicks,
                               1e9/((double) n*Stopwatch.Frequency/stopwatch.ElapsedTicks) );
        }

        private void ThreadPush()
        {
            for ( int i = 0; i < n; i++ )
            {
                this.stack.Add( i );
            }
            this.stack.CompleteAdding();
        }

        private void ThreadPop()
        {
            for ( int i = 0; i < n; )
            {
                int value;
                if ( this.stack.TryTake( out value ) )
                    i++;
            }
        }
    }
}