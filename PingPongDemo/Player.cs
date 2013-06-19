using System;
using System.Threading;
using System.Threading.Tasks;
using PostSharp.Toolkit.Threading;

namespace TestAsync
{
    class Player : Actor
    {
        readonly string name;
        private int counter;

        public Player(string name)
        {
            this.name = name;
        }

        public async Task<int> GetCounter()
        {
            return this.counter;
        }

        public async Task Ping(Player peer, int countdown)
        {
            Console.WriteLine("{0}.Ping({1}) from thread {2}", this.name, countdown, Thread.CurrentThread.ManagedThreadId);

            if (countdown > 1)
            {
                await peer.Ping(this, countdown - 1);
            }

            this.counter++;
        }

        [ThreadSafe]
        public override string ToString()
        {
            return this.name;
        }

    }
}
