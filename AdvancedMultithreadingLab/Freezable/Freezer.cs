namespace AdvancedMultithreadingLab.Freezable
{
    public static class Freezer
    {
        public static void Main()
        {
            Icecream icecream = new Icecream();
            icecream.AddIngredient( Ingredient.Milk, 1 );
            icecream.AddIngredient( Ingredient.Sugar, 0.3 );
            icecream.AddIngredient( Ingredient.VanillaFlavour, 0.05 );
            icecream.Freeze();
            icecream.AddIngredient(Ingredient.VanillaFlavour, 0.05);
        }
        
    
    }
}