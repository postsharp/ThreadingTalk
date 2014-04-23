using System;
using System.Collections.Generic;
using System.Threading;
using PostSharp.Patterns.Threading;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;


namespace AdvancedMultithreadingLab.ThreadUnsafe
{

    
    [ThreadUnsafe]
    class ThreadUnsafeObject
    {
        [Child]
        readonly AdvisableCollection<int> list = new AdvisableCollection<int>();

        public void Add(int x)
        {
            this.list.Add( x );
        }
    }
}
