using System;
using PostSharp.Patterns.Threading;

namespace AdvancedMultithreadingLab.ReaderWriterLock
{
    
    [ReaderWriterSynchronized]
    class SynchronizedOrder : IOrder
    {
        [Writer]
        public void Set(int amount, int discount)
        {
            if (amount < discount) throw new InvalidOperationException();
            this.Amount = amount;
            this.Discount = discount;
        }

        public int Amount { get; private set; }

        public int Discount { get; private set; }

        public int AmountAfterDiscount
        {
             get { return this.Amount - this.Discount; }
        }
    }
}