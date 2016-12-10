using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab
{
    internal class TestBlockingCollection : TestCollectionBase
    {
        private readonly BlockingCollection<int> collection = new BlockingCollection<int>();

        protected override void AddItems( int count )
        {
            for ( int i = 0; i < count; i++ )
            {
                this.collection.Add( i );
            }
        }

        protected override void ConsumeItems( int count )
        {
            var enumerator = this.collection.GetConsumingEnumerable().GetEnumerator();
            for ( int i = 0; i < count; i++ )
            {
                enumerator.MoveNext();    
            }
            
        }
        
    }
}