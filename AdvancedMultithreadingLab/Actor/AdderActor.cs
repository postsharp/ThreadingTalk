#region Copyright (c) 2012 by SharpCrafters s.r.o.

// Copyright (c) 2012, SharpCrafters s.r.o.
// All rights reserved.
// 
// For licensing terms, see file License.txt

#endregion

using System;
using System.Threading;

namespace AdvancedMultithreadingLab.Actor
{
    internal sealed class AdderActor : PostSharp.Patterns.Threading.Actor
    {
        private int count;

        public void Add( int a )
        {
            this.count += a;
        }

        public void Complete( int expectedValue, CountdownEvent countdownEvent )
        {
            if ( this.count != expectedValue ) throw new Exception();
            countdownEvent.Signal();
        }
    }
}