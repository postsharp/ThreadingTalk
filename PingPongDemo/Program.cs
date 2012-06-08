using System;
using System.Threading.Tasks;

namespace TestAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().Wait();
        }

        private static async Task AsyncMain()
        {

            Player ping = new Player("Sarkozy");
            Player pong = new Player("Hollande");

            Task pingTask = ping.Ping(pong, 4);
            Task pongTask = pong.Ping(ping, 4);

            await pingTask;
            await pongTask;

            Console.WriteLine("{0} Counter={1}", ping, await ping.GetCounter());
            Console.WriteLine("{0} Counter={1}", pong, await pong.GetCounter());
        }
    }
}
