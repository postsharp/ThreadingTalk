#region Copyright (c) 2012 by SharpCrafters s.r.o.

// Copyright (c) 2012, SharpCrafters s.r.o.
// All rights reserved.
// 
// For licensing terms, see file License.txt

#endregion

using System;
using System.Diagnostics;
using System.Threading;

namespace AdvancedMultithreadingLab.Actor
{
    internal sealed class TestActor
    {
        private const int n = 10000;

        private readonly AdderActor[] actors = new AdderActor[500];

        public void Start()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            for ( int i = 0; i < this.actors.Length; i++ )
            {
                this.actors[i] = new AdderActor();
            }

            stopwatch.Start();

            for ( int i = 0; i < n; i++ )
            {
                foreach ( AdderActor actor in this.actors )
                {
                    actor.Add( 1 );
                }
            }

            CountdownEvent countdownEvent = new CountdownEvent( this.actors.Length );
            for ( int i = 0; i < this.actors.Length; i++ )
            {
                this.actors[i].Complete( n, countdownEvent );
            }
            countdownEvent.Wait();


            Console.WriteLine( "Actor: {0:0.0} MT/s ({1:0} ns/T)", 1e-6*n*this.actors.Length*Stopwatch.Frequency/stopwatch.ElapsedTicks,
                               1e9/((double) n*this.actors.Length*Stopwatch.Frequency/stopwatch.ElapsedTicks) );
        }
    }
}