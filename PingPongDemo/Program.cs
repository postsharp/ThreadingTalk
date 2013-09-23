#region Copyright (c) 2004-2010 by SharpCrafters s.r.o.

// This file is part of PostSharp source code and is the property of SharpCrafters s.r.o.
// 
// Source code is provided to customers under strict non-disclosure agreement (NDA). 
// YOU MUST HAVE READ THE NDA BEFORE HAVING RECEIVED ACCESS TO THIS SOURCE CODE. 
// Severe financial penalties apply in case of non respect of the NDA.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAsync
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            AsyncMain().Wait();
        }

        private static async Task AsyncMain()
        {
            // Create a logger;
            ConsoleLogger consoleLogger = new ConsoleLogger();

            // Create two players.
            Player sarkozy = new Player( consoleLogger,  "Sarkozy", 0.95 );
            Player hollande = new Player( consoleLogger,  "Hollande", 0.9 );

            // Start several concurrent games between players.
            ConsoleColor[] colors = new[] { ConsoleColor.Cyan, ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.Magenta, ConsoleColor.Yellow,ConsoleColor.Red,  };
            Task<Player>[] games = colors.Select( color => sarkozy.Ping( hollande, color ) ).ToArray();

            await Task.WhenAll( games );

        
            consoleLogger.WriteLine("We are all done");
            foreach ( Task<Player> game in games )
            {
                consoleLogger.WriteLine(string.Format( "Winner: {0}", game.Result));
            }

            consoleLogger.WriteLine( string.Format( "{0} totally received {1} balls", sarkozy, await sarkozy.GetCounter()));
            consoleLogger.WriteLine( string.Format( "{0} totally received {1} balls", hollande, await hollande.GetCounter()));

            await consoleLogger.Flush();
        }
    }
}