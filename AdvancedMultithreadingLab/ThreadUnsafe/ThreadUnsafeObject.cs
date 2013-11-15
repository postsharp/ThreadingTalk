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
            this.list.Add( x );
        }
    }
}
