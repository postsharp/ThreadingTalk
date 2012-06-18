using System;
using System.Threading;

namespace AdvancedMultithreadingLab.ThreadUnsafe
{
    sealed class TestThreadUnsafeObject
    {
        readonly Thread[] threads = new Thread[Environment.ProcessorCount];
        readonly ThreadUnsafeObject o = new ThreadUnsafeObject();

        public void Start()
        {
            for ( int i = 0; i < this.threads.Length; i++)
            {
                this.threads[i] = new Thread( this.ThreadMain );
                this.threads[i].Start();
            }

            foreach ( Thread t in this.threads )
            {
                t.Join();
            }
        }

        private void ThreadMain()
        {
            try
            {
                for ( int i = 0; i < 100000; i++ )
                {
                    this.o.Add( 1 );
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.GetType().Name + ": " + e.Message);
            }
        }
    }
}