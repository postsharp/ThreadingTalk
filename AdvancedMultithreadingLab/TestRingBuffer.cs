using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    public class TestRingBuffer : TestCollectionBase
    {
        private readonly RingBuffer<int> bag = new RingBuffer<int>(64000);


      
        protected override void AddItems( int count )
        {
            for ( int i = 0; i < count; i++ )
            {
                this.bag.Add( i );
            }
        }

        protected override void ConsumeItems( int count )
        {
            SpinWait spinWait = new SpinWait();
            int value;

            for ( int i = 0; i < count; )
            {
                if ( this.bag.TryTake( out value ) )
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