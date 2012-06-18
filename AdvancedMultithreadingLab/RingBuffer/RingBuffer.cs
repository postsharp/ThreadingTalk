#region Copyright (c) 2012 by SharpCrafters s.r.o.

// Copyright (c) 2012, SharpCrafters s.r.o.
// All rights reserved.
// 
// For licensing terms, see file License.txt

#endregion

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AdvancedMultithreadingLab.RingBuffer
{
    public sealed class RingBuffer<T> : IProducerConsumerCollection<T>
    {
        private readonly T[] items;

#pragma warning disable 649 // Fields don't need to be initialized.
        private PaddedUInt32 head;
        private PaddedUInt32 tailFree;
        private PaddedUInt32 tailBusy;
#pragma warning restore 649

        private volatile uint capacity;

        public RingBuffer( int capacity )
        {
            this.capacity = RoundUpPower2( (uint) capacity );
            this.items = new T[this.capacity];
        }

        private static uint RoundUpPower2( uint value )
        {
            for ( int i = 0; i < 32; i++ )
            {
                int bit = 1 << i;
                if ( value == bit ) return value;
                if ( value < bit )
                    return 1u << i;
            }

            throw new ArgumentOutOfRangeException();
        }

        public void Add( T item )
        {
            if ( !this.TryAdd( item ) )
                throw new InvalidOperationException();
        }


        public bool TryAdd( T item )
        {
            while ( true )
            {
                // Find a free slot.
                uint position = this.tailFree.Value;
                uint localHead = this.head.Value;

                if ( this.tailFree.Value != position ) continue;

                if ( position - localHead >= this.capacity )
                    return false;

                uint nextPosition = position + 1;

                // We have to do a CompareExchange, and not an Increment, to make sure we don't overflow.
                if ( this.tailFree.CompareExchange( nextPosition, position ) == position )
                {
                    // We got a free slot. Write into it.

                    this.items[position & (this.capacity - 1)] = item;

                    // Wait until other threads have written their value and updated tailBusy.
                    while ( true )
                    {
                        if ( this.tailBusy.CompareExchange( nextPosition, position ) == position )
                            return true;
                    }
                }
            }
        }

        public bool TryTake( out T item )
        {
            while ( true )
            {
                uint position = this.head.Value;
                uint tail = this.tailBusy.Value;

                if ( position == tail )
                {
                    item = default(T);
                    return false;
                }

                uint nextPosition = position + 1;

                uint index = position & (this.capacity - 1);
                item = this.items[index];

                if ( this.head.CompareExchange( nextPosition, position ) == position )
                {
                    return true;
                }
            }
        }

        public bool TryPeek( out T item )
        {
            uint index = this.head.Value;

            if ( index == this.tailBusy.Value )
            {
                item = default(T);
                return false;
            }

            item = this.items[index & (this.capacity - 1)];
            return true;
        }

        public override string ToString()
        {
            return string.Format( "RingBuffer<{0}>, Capacity=0x{1:x}, Head=0x{2:x}, TailFree=0x{3:x}, TailBusy=0x{4:x}",
                                  typeof(T), this.capacity, this.head.Value, this.tailFree.Value, this.tailBusy.Value );
        }

        #region Not implemented

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo( Array array, int index )
        {
            throw new NotImplementedException();
        }


        public bool IsEmpty
        {
            get
            {
                uint localHead;
                uint localTailBusy;

                // Get a consistent snapsot of head and tailBusy.
                while ( true )
                {
                    localHead = this.head.Value;
                    localTailBusy = this.tailBusy.Value;

                    if ( localHead == this.head.Value )
                        break;
                }

                return localHead == localTailBusy;
            }
        }

        public bool IsFull
        {
            get
            {
                uint localHead;
                uint localTailFree;

                // Get a consistent snapsot of head and tailBusy.
                while ( true )
                {
                    localHead = this.head.Value;
                    localTailFree = this.tailFree.Value;

                    if ( localHead == this.head.Value )
                        break;
                }

                return (localHead - localTailFree >= this.capacity);
            }
        }

        public int Count
        {
            get
            {
                // This operation is not atomic, so the result count be inaccurate.
                return (int) (this.tailFree.Value - this.head.Value);
            }
        }

        public int Capacity
        {
            get { return (int) this.capacity; }
        }

        object ICollection.SyncRoot
        {
            get { throw new InvalidOperationException(); }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        void IProducerConsumerCollection<T>.CopyTo( T[] array, int index )
        {
            throw new NotImplementedException();
        }


        T[] IProducerConsumerCollection<T>.ToArray()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}