using System;

namespace AdvancedMultithreadingLab.ReaderWriterLock
{
    // TODO: Add custom attributes.

    class SynchronizedOrder : IOrder
    {
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