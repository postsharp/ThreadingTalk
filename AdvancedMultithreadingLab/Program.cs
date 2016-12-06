#region Copyright (c) 2012 by SharpCrafters s.r.o.

// Copyright (c) 2012, SharpCrafters s.r.o.
// All rights reserved.
// 
// For licensing terms, see file License.txt

#endregion

using System;
using AdvancedMultithreadingLab.Benchmarks;

namespace AdvancedMultithreadingLab
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            //Freezer.Main();
          // new TestReaderWriterLock<ManuallySynchronizedOrder>().Test();

            MiscBenchmarks.ExecuteAll();
            //new TestConcurrentStack().Start();
            //new TestRingBuffer().Start();
            //new TestBlockingCollection().Start();
            //new TestThreadUnsafeObject().Start();
            //new TestActor().Start();
            //new TestInterlocked().Start();
            
            Console.WriteLine("Done.");
        }
    }
}