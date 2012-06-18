#region Copyright (c) 2012 by SharpCrafters s.r.o.

// Copyright (c) 2012, SharpCrafters s.r.o.
// All rights reserved.
// 
// For licensing terms, see file License.txt

#endregion

using System.Threading;

namespace AdvancedMultithreadingLab.TrivialConcurrentStack
{
    internal sealed class ConcurrentStack<T>
    {
        private Node head;

        public void Push( T value )
        {
            Node node = new Node {Value = value};

            for ( ;; )
            {
                Node localHead = this.head;
                node.Next = localHead;

                if ( Interlocked.CompareExchange( ref this.head, node, localHead ) == localHead )
                    return;
            }
        }

        public bool TryPop( out T value )
        {
            for ( ;; )
            {
                Node localHead = this.head;

                if ( localHead == null )
                {
                    value = default(T);
                    return false;
                }

                if ( Interlocked.CompareExchange( ref this.head, localHead.Next, localHead ) == localHead )
                {
                    value = localHead.Value;
                    return true;
                }
            }
        }

        #region Nested type: Node

        private sealed class Node
        {
            public Node Next;
            public T Value;
        }

        #endregion
    }
}