using System;
using System.Collections.Generic;
using System.Threading;
using PostSharp.Patterns.Threading;


namespace AdvancedMultithreadingLab.ThreadUnsafe
{

    [ThreadUnsafe]
    class ThreadUnsafeObject
    {
        readonly List<int> list = new List<int>();

        public void Add(int x)
        {
#if DEBUG
            if ( !Monitor.TryEnter(this))
                throw new Exception();
#endif
            this.list.Add( x );
#if DEBUG
            Monitor.Exit(this);
#endif
        }
    }
}
