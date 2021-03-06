using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    public class TestMyConcurrentQueue : TestCollectionBase
    {
        private readonly MyConcurrentQueue<int> queue = new MyConcurrentQueue<int>();

   

        protected override void AddItems( int count )
        {
            for ( int i = 0; i < count; i++ )
            {
                this.queue.Enqueue( i );
            }
        }

        protected override void ConsumeItems( int count )
        {
            SpinWait spinWait = new SpinWait();
            int value;

            for ( int i = 0; i < count; )
            {
                if ( this.queue.TryDequeue( out value ) )
                {
                    i++;

                    spinWait.Reset();
                }
                else
                {
                    spinWait.SpinOnce();
                }
            }

        }

    }
}