using System.Threading;

namespace AdvancedMultithreadingLab
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