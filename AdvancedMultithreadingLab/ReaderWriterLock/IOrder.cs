using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvancedMultithreadingLab.ReaderWriterLock
{
    interface IOrder
    {
        void Set( int amount, int discount );
        int Amount { get; }
        int Discount { get; }
        int AmountAfterDiscount { get; }
    }
}
