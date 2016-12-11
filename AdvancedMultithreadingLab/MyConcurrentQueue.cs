using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedMultithreadingLab
{

    // http://www.cs.rochester.edu/research/synchronization/pseudocode/queues.html#nbq
    // http://www.cs.rochester.edu/u/scott/papers/1998_JPDC_nonblocking.pdf

    class MyConcurrentQueue<T>
    {
        private volatile Node head;
        // The next fields are to make sure the head and the tail are situated on different cache lines.
        private Node o1, o2, o3, o4, o5, o6, o7, o8, o9, o10, o11, o12, o13, o14, o15;
        private volatile Node tail;

        public MyConcurrentQueue()
        {
            this.head = this.tail = new Node();
        }

        public void Enqueue(T value)
        {
            SpinWait wait = new SpinWait();

            Node localTail;
            Node node = new Node { Value = value };

            while ( true )
            {
                localTail = this.tail;
                Node localNext = localTail.Next;

                if ( localTail == this.tail ) // Are localTail and localNext consistent?
                {
                    // Was Tail pointing to the last node?
                    if ( localNext == null )
                    {
                        // Try to link node at the end of the linked list
                        if ( Interlocked.CompareExchange( ref localTail.Next, node, localNext ) == localNext )
                        {
                            // Enqueuing is done.
                            break;
                        }
                    }
                    else // Tail was not pointing to the last node
                    {
                        // Try to swing Tail to the next node
                        if ( Interlocked.CompareExchange( ref this.tail, localNext, localTail ) == localTail )
                        {
                            // Continue without waiting.
                            continue;
                        }
                    }

                }

                wait.SpinOnce();
            }

            Interlocked.CompareExchange( ref this.tail, node, localTail );

        }

        public bool TryDequeue( out T value )
        {
            SpinWait wait = new SpinWait();

            while ( true )
            {
                Node localHead = this.head;
                Node localTail = this.tail;
                Node localNext = localHead.Next;

                if ( localHead == this.head ) // Are localHead, localTail, and localNext consistent?
                {
                    if ( localHead == localTail ) // Is queue empty or Tail falling behind?
                    {
                        if ( localNext == null )
                        {
                            value = default( T );
                            return false;
                        }
                        else
                        {

                            // Tail is falling behind.  Try to advance it
                            Interlocked.CompareExchange( ref this.tail, localNext, localTail );
                            continue; // Continue without waiting.
                        }
                    }
                    else
                    {
#if DEBUG
                        if ( localNext == null )
                            throw new Exception( "Invariant broken." );
#endif

                        if ( Interlocked.CompareExchange( ref this.head, localNext, localHead ) == localHead )
                        {
                            value = localNext.Value;
                            return true;
                        }
                    }
                }

                wait.SpinOnce();
            }
        }

        class Node
        {
            public T Value;
            public volatile Node Next;
        }
    }
}
