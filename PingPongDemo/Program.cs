#region Copyright (c) 2004-2010 by SharpCrafters s.r.o.

// This file is part of PostSharp source code and is the property of SharpCrafters s.r.o.
// 
// Source code is provided to customers under strict non-disclosure agreement (NDA). 
// YOU MUST HAVE READ THE NDA BEFORE HAVING RECEIVED ACCESS TO THIS SOURCE CODE. 
// Severe financial penalties apply in case of non respect of the NDA.

#endregion

using System;
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
            Player sarkozy = new Player( "Sarkozy", 0.9 );
            Player hollande = new Player( "Hollande", 0.8 );

            Task<Player> game1 = sarkozy.Ping( hollande, "blue" );
            Task<Player> game2 = hollande.Ping( sarkozy, "red" );

            Player game1Winner = await game1;
            Player gamer2Winner = await game2;

            Console.WriteLine( "Game1 winner = {0}, Game2 winner ={1}", game1Winner, gamer2Winner );
            Console.WriteLine( "{0} totally received {1} balls", sarkozy, await sarkozy.GetCounter() );
            Console.WriteLine( "{0} totally received {1} balls", hollande, await hollande.GetCounter());
        }
    }
}