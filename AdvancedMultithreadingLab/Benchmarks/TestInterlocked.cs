#region Copyright (c) 2012 by SharpCrafters s.r.o.

// Copyright (c) 2012, SharpCrafters s.r.o.
// All rights reserved.
// 
// For licensing terms, see file License.txt

#endregion

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace AdvancedMultithreadingLab.Benchmarks
{
    internal class TestInterlocked
    {
        private const int n = 200000000;
        private int field;

        public void Start()
        {
            for (int i = -1; i < Environment.ProcessorCount; i++)
            {
                for ( int j = 0; j < Environment.ProcessorCount; j++ )
                {

                    Console.Write( "{0:0.0}", this.Start( i, j ) );
                    Console.Write( "; " );

                }

                Console.WriteLine();
            }
        }
        private double Start(int processorId1, int processorId2)
        {

            Stopwatch stopwatch = Stopwatch.StartNew();

            Thread thread1 = new Thread( this.ThreadMain);
            Thread thread2 = new Thread( this.ThreadMain);

            stopwatch.Start();

            thread1.Start(processorId1);
            thread2.Start(processorId2);

            thread1.Join();
            thread2.Join();

            double ns = 1e9/((double) n*Stopwatch.Frequency/stopwatch.ElapsedTicks);
            return ns;
            Console.WriteLine("TestInterlocked({2},{3}): {0:0.0} MT/s ({1:0} ns/T)", 1e-6 * n * Stopwatch.Frequency / stopwatch.ElapsedTicks,
                               ns, processorId1, processorId2 );
        }

        private void ThreadMain(object state)
        {
            int processorId = (int) state;
            if (processorId < 0) return;
            SetThreadAffinityMask( GetCurrentThread(), (IntPtr) (1 << processorId) );
            
            // If the new thread affinity mask does not specify the processor 
            // that is currently running the thread, the thread is rescheduled on one of the allowable processors.

            for ( int i = 0; i < n; i++ )
            {
                Interlocked.Increment( ref this.field );
            }
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr SetThreadAffinityMask(IntPtr hThread, IntPtr dwThreadAffinityMask);

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr GetCurrentThread();

    }
}