using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Extensibility;

namespace AdvancedMultithreadingLab.Freezable
{
    [Serializable]
    [IntroduceInterface(typeof(IFreezable))]
    public class FreezableAttribute : InstanceLevelAspect, IFreezable
    {
        [IntroduceMember]
        public bool CanFreeze { get { return !this.IsFrozen; } }

        [IntroduceMember]
        public bool IsFrozen { get; private set; }

        [IntroduceMember]
        public void Freeze()
        {
            if ( !this.CanFreeze )
                throw new InvalidOperationException();

            this.IsFrozen = true;

            // TODO: Freeze "owned" objects. We could use reflection or have an ownership aspect system.
        }

        [OnLocationSetValueAdvice, MulticastPointcut(Targets = MulticastTargets.Field, Attributes = MulticastAttributes.Instance)]
        public void OnSetField( LocationInterceptionArgs args )
        {
            if ( this.IsFrozen )
                throw new InvalidOperationException("The object is frozen.");

            args.ProceedSetValue();
        }
    }
}
