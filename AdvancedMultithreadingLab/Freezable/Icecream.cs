using System;
using System.Collections.Generic;

namespace AdvancedMultithreadingLab.Freezable
{
    [Freezable]
    class Icecream 
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


    }
}