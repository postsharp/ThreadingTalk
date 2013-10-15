using System;
using System.Collections.Generic;

namespace AdvancedMultithreadingLab.Freezable
{
    class Icecream : IFreezable
    {
        private readonly Dictionary<Ingredient, double> composition = new Dictionary<Ingredient, double>();

        
        public double TotalVolume { get; private set; }

        public void AddIngredient( Ingredient ingredient, double volume )
        {
            double currentIngredientVolume;
            this.composition.TryGetValue( ingredient, out currentIngredientVolume );
            this.TotalVolume += volume;
            this.composition[ingredient] = currentIngredientVolume + volume;
        }


        #region Implementation of IFreezable
        public bool CanFreeze { get { return !this.IsFrozen; } }
        public bool IsFrozen { get; private set; }


        public void Freeze()
        {
            if ( !this.CanFreeze )
                throw new InvalidOperationException();

            this.IsFrozen = true;
        }
        #endregion

    }
}