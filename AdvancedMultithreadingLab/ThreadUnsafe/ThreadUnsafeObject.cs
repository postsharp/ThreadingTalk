using System.Collections.Generic;
using PostSharp.Toolkit.Threading.Dispatching;

namespace AdvancedMultithreadingLab.ThreadUnsafe
{
    [ThreadUnsafeClass]
    class ThreadUnsafeObject
    {
        readonly List<int> list = new List<int>();

        public void Add(int x)
        {
            this.list.Add( x );
        }
    }
}
