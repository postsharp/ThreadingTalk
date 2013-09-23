using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Patterns.Threading;

namespace TestAsync
{
    class ConsoleLogger : Actor
    {
        public void WriteLine( string message, ConsoleColor color = ConsoleColor.White )
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }

        public async Task Flush()
        {
            
        }
    }
}
