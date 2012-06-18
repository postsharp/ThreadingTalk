using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace BenchmarkThreadSync
{
    class Program
    {
        static readonly object sync = new object();
        static volatile int volatileField = 0;
         
        static void Main(string[] args)
        {
            foreach (int i in new[] { 1, 2, 6 })
            {
                int i1 = i;
                RunParallel(() => TestAlloc(string.Format("Alloc with {0} threads", i1)), i1);
            }

            TestIncrement();
            TestUncontented();
            TestInterlocked("Uncontended interlocked");
            RunParallel(() => TestInterlocked(string.Format("Interlocked with {0} threads", 2)), 2);
            foreach ( int i in new[] { 1, 2, 6})
            {
                int i1 = i;
                RunParallel( () => TestInterlocked( string.Format( "Interlocked with {0} threads", i1 ) ), i1 );
            }

            
            RunParallel(TestContendedEnter, 2);
            TestSetEvent();
            TestWaitEvent();
            TestCreateEvent();
            TestSleep0();

        }

        private static void RunParallel(Action action, int threads)
        {
            ManualResetEvent[] handles = new ManualResetEvent[threads];
            for ( int i = 0; i < threads; i++)
            {
                handles[i] = new ManualResetEvent( false );
                int i1 = i;
                Thread t1 = new Thread(() =>
                {
                    try
                    {
                        action();
                    }
                    finally
                {
                    handles[i1].Set();
                }});
                t1.Start();
            }
            WaitHandle.WaitAll( handles );
        }

        private static void TestUncontented()
        {
            const int n = 100000000;

            Stopwatch stopwatch = Stopwatch.StartNew();
            for ( int i = 0; i < n; i++ )
            {
                Monitor.Enter( sync );
                Monitor.Exit( sync );
            }
            stopwatch.Stop();

            WriteTime("Uncontended lock", n, stopwatch.ElapsedTicks);
        }

        private static void TestAlloc(string name)
        {
            const int n = 1000000000;

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < n; i++)
            {
                new object();
            }
            GC.WaitForFullGCComplete();
            stopwatch.Stop();

            WriteTime(name, n, stopwatch.ElapsedTicks);
        }

        private static void TestInterlocked(string name)
        {
            const int n = 100000000;

             Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < n; i++)
            {
                Interlocked.CompareExchange( ref volatileField, volatileField + 1, volatileField );
            }
            stopwatch.Stop();

            WriteTime(name, n, stopwatch.ElapsedTicks);
            
        }

        private static void TestIncrement()
        {
            int localVariable = 0;
            const int n = 1000000000;
            int a = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
//            for (int i = 0; i < n / 32; i++)
//            {
//                a ++;
//            }
            Console.WriteLine(a);
          //  long overhead = stopwatch.ElapsedTicks;
       //     WriteTime("Overhead", n/32,  overhead);
            stopwatch.Restart();

            for (int i = 0; i < n/32; i++)
            {
                volatileField = volatileField + 1;
                volatileField = volatileField + 2;
                volatileField = volatileField + 3;
                volatileField = volatileField + 4;
                volatileField = volatileField + 5;
                volatileField = volatileField + 6;
                volatileField = volatileField + 7;
                volatileField = volatileField + 8;
                volatileField = volatileField + 9;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;
                volatileField = volatileField + 1;

              
                
            }
            stopwatch.Stop();

            WriteTime("Single-core increment", n, stopwatch.ElapsedTicks );

        }

        static void TestContendedEnter()
        {
            const int n = 100000000;
           
            Stopwatch stopwatch = Stopwatch.StartNew();
            int j = 0;
            for (int i = 0; i < n; i++)
            {
                lock ( sync )
                {
                    j++;
                }
                
                
            }
            stopwatch.Stop();

            WriteTime("Contented Monitor", n, stopwatch.ElapsedTicks);
            
        }

        static ManualResetEvent manualResetEvent = new ManualResetEvent( false );
        static void TestSetEvent()
        {
            const int n = 1000000;

            Stopwatch stopwatch = Stopwatch.StartNew();
            int j = 0;
            for (int i = 0; i < n; i++)
            {
                manualResetEvent.Reset();
                manualResetEvent.Set();
                
            }
            stopwatch.Stop();

            WriteTime("Set Event", n, stopwatch.ElapsedTicks);

        }

        static void TestCreateEvent()
        {
            const int n = 1000000;

            Stopwatch stopwatch = Stopwatch.StartNew();
            int j = 0;
            for (int i = 0; i < n; i++)
            {
                using ( new ManualResetEvent( false )){}

            }
            stopwatch.Stop();

            WriteTime("Create Event", n, stopwatch.ElapsedTicks);

        }

        static void TestWaitEvent()
        {
            const int n = 1000000;

            manualResetEvent.Reset();
            Stopwatch stopwatch = Stopwatch.StartNew();
            int j = 0;
            for (int i = 0; i < n; i++)
            {
                manualResetEvent.WaitOne( 0 );

            }
            stopwatch.Stop();

            WriteTime("Wait Event", n, stopwatch.ElapsedTicks);

        }

        static void TestSleep0()
        {
            const int n = 1000000;

            manualResetEvent.Reset();
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < n; i++)
            {
                Thread.Sleep( 0 );

            }
            stopwatch.Stop();

            WriteTime("Sleep(0)", n, stopwatch.ElapsedTicks);

        }

        private static void WriteTime( string name, int n, long ticks )
        {
            double mhz = 1e-6*n*Stopwatch.Frequency/ticks;
            Console.WriteLine("{0}: {1} MHz - {2} ns", name, mhz, 1E3/mhz );
        }
    }
}
