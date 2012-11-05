using System.Collections.Generic;
using PostSharp.Toolkit.Threading;

namespace AdvancedMultithreadingLab.ThreadUnsafe
{
    [ThreadUnsafeObject]
    class ThreadUnsafeObject
    {
        readonly List<int> list = new List<int>();

        public void Add(int x)
        {
            this.list.Add( x );
        }
    }
}
