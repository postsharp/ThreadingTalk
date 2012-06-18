#region Copyright (c) 2012 by SharpCrafters s.r.o.

// Copyright (c) 2012, SharpCrafters s.r.o.
// All rights reserved.
// 
// For licensing terms, see file License.txt

#endregion

using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace AdvancedMultithreadingLab.RingBuffer
{
    [StructLayout( LayoutKind.Explicit, Size = 128 )]
    public struct PaddedUInt32
    {
        [FieldOffset( 64 )] private volatile int value;

        public uint Value
        {
            get { return (uint) this.value; }
        }

        public uint CompareExchange( uint value, uint comparand )
        {
#pragma warning disable 420
            return (uint) Interlocked.CompareExchange( ref this.value, (int) value, (int) comparand );
#pragma warning restore 420
        }

        public override string ToString()
        {
            return this.Value.ToString( CultureInfo.InvariantCulture );
        }
    }
}