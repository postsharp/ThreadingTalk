using System.Linq;
using System.Text;

namespace AdvancedMultithreadingLab.Freezable
{
    public interface IFreezable
    {
        bool CanFreeze { get; }
        bool IsFrozen { get; }
        void Freeze();
    }
}
