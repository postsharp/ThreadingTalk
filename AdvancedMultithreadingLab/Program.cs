#define HIGH_PRECISION

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using System;
using System.Diagnostics;
using System.Threading;


namespace AdvancedMultithreadingLab
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            RunTest<TestMyConcurrentQueue>();
            RunTest<TestMyConcurrentStack>();
            RunTest<TestSystemConcurrentStack>();
            RunTest<TestSystemConcurrentQueue>();
            RunTest<TestSystemConcurrentBag>();
            RunTest<TestBlockingCollection>();
        }

        static void RunTest<T>() where T : TestCollectionBase, new()
        {
#if HIGH_PRECISION
            ManualConfig config = new ManualConfig();
            config.Add( new Job( "DefaultJob", RunMode.Default, EnvMode.RyuJitX64 )
            {
                Env = { Runtime = Runtime.Clr },
                Accuracy = { MaxStdErrRelative = 0.05, RemoveOutliers = true },
            } );
            config.Add( ConsoleLogger.Default );
            config.Add( CsvExporter.Default, HtmlExporter.Default );


            var summary = BenchmarkRunner.Run<T>( config );
            
#else
            for ( int threads = 1; threads <= 2; threads++ )
            {
                T test = new T() { Threads = threads };
                test.Setup();
                test.RunTests();
                GC.Collect();
                Stopwatch stopwatch = Stopwatch.StartNew();
                const int n = 10;
                for ( int i = 0; i < n; i++ )
                {
                    test.RunTests();
                }
                GC.Collect();
                stopwatch.Stop();
                test.Dispose();
                long operations = n * TestCollectionBase.Iterations;
                double ns = ((double) stopwatch.ElapsedTicks / operations) / (1E-9 * Stopwatch.Frequency);
                Console.WriteLine( $"{typeof( T ).Name} with {threads} threads on each end: {ns:0.0} ns/op, {threads * 1E3 / ns:0.0} MT/s" );
            }
#endif
        }

    }
}