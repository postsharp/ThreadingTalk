using System;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    internal class TestTrivialConcurrentStack : TestCollectionBase
    {
        private readonly TrivialConcurrentStack<int> stack = new TrivialConcurrentStack<int>();

        protected override void AddItems( int count )
        {
            for ( int i = 0; i < count; i++)
            {
                this.stack.Push( i );
            }
        }

        protected override void ConsumeItems( int count )
        {
            SpinWait spinWait = new SpinWait();
            int value;

            for ( int i = 0; i < count; )
            {
                if ( this.stack.TryPop( out value ) )
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