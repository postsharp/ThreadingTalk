using System;
using System.Threading;
using System.Threading.Tasks;
using PostSharp.Patterns.Threading;

namespace TestAsync
{
    class Player : Actor
    {
        readonly string name;
        readonly Random random = new Random();
        private readonly double skills;

        private int counter;
        

        public Player(string name, double skills )
        {
            this.name = name;
            this.skills = skills;
        }

        public async Task<int> GetCounter()
        {
            return this.counter;
        }

        public async Task<Player> Ping( Player peer, string color )
        {
            Console.WriteLine("{0}.Ping( color={1} ) from thread {2}", this.name, color, Thread.CurrentThread.ManagedThreadId);

            if ( random.NextDouble() <= skills )
            {
                this.counter++;
                return await peer.Ping(this, color);
            }
            else
            {
                return peer;
            }
        }

        [ExplicitlySynchronized]
        public override string ToString()
        {
            return this.name;
        }

    }
}
