using System;
using System.Threading;

namespace AdvancedMultithreadingLab.ReaderWriterLock
{
    class TestReaderWriterLock<T> where T : IOrder, new()
    {
        T order = new T();
        public void Test()
        {
            Thread thread1 = new Thread( this.ThreadMain );
            Thread thread2 = new Thread(this.ThreadMain);
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

        }

        private void ThreadMain()
        {
            Random random = new Random(Thread.CurrentThread.ManagedThreadId);
            for ( int i = 0; i < 1000000; i++ )
            {
                int amount = random.Next( 100 );
                int discount = random.Next( amount );
                this.order.Set( amount, discount );
                if ( this.order.AmountAfterDiscount < 0 )
                    throw new Exception();
            }
        }
    }
}