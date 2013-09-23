#region Copyright (c) 2004-2010 by SharpCrafters s.r.o.

// This file is part of PostSharp source code and is the property of SharpCrafters s.r.o.
// 
// Source code is provided to customers under strict non-disclosure agreement (NDA). 
// YOU MUST HAVE READ THE NDA BEFORE HAVING RECEIVED ACCESS TO THIS SOURCE CODE. 
// Severe financial penalties apply in case of non respect of the NDA.

#endregion

using System;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab.ActorModel
{
    internal sealed class TestActor
    {
        private const int n = 100000;

        private readonly AdderActor[] actors = new AdderActor[500];

        public void Start()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Create all actors.
            for ( int i = 0; i < this.actors.Length; i++ )
            {
                this.actors[i] = new AdderActor();
            }

            stopwatch.Start();


            // Feed actors with work items from each core.
            Thread[] threads = new Thread[Environment.ProcessorCount];
            for ( int i = 0; i < Environment.ProcessorCount; i++ )
            {
                threads[i] = new Thread( () =>
                                           {
                                               for ( int j = 0; j < n/Environment.ProcessorCount; j++ )
                                               {
                                                   foreach ( AdderActor actor in this.actors )
                                                   {
                                                       actor.Add( 1 );
                                                   }
                                               }
                                           } );
                threads[i].Start();
            }

            // Wait until we've enqueued all accesses.
            foreach ( Thread thread in threads )
            {
                thread.Join();
            }

            // Wait until all actors processed their queue.
            CountdownEvent countdownEvent = new CountdownEvent( this.actors.Length );
            for ( int i = 0; i < this.actors.Length; i++ )
            {
                this.actors[i].Complete( n, countdownEvent );
            }
            countdownEvent.Wait();


            // Write performance metrics.
            Console.WriteLine( "Actor: {0:0.0} MT/s ({1:0} ns/T)", 1e-6*n*this.actors.Length*Stopwatch.Frequency/stopwatch.ElapsedTicks,
                               1e9/((double) n*this.actors.Length*Stopwatch.Frequency/stopwatch.ElapsedTicks) );
        }
    }
}