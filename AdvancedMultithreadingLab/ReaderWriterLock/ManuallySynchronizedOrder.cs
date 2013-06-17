using System;
using System.Threading;

namespace AdvancedMultithreadingLab.ReaderWriterLock
{
    class ManuallySynchronizedOrder : IOrder
    {
        ReaderWriterLockSlim  @lock = new ReaderWriterLockSlim();

        public void Set(int amount, int discount)
        {
            if (amount < discount) throw new InvalidOperationException();

            this.@lock.EnterWriteLock();
            this.Amount = amount;
            this.Discount = discount;
            this.@lock.ExitWriteLock();
        }

        public int Amount { get; private set; }
        public int Discount { get; private set; }

        public int AmountAfterDiscount
        {
            get
            {
                this.@lock.EnterWriteLock();
                int result = this.Amount - this.Discount;
                this.@lock.ExitWriteLock();
                return result;

            }
        }
    }
}