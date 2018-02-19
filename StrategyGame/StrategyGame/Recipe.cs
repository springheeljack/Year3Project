using System.Collections.Generic;

namespace StrategyGame
{
    public class Recipe
    {
        public static Dictionary<string, Recipe> Dictionary { get; private set; }

        public static void Initialize()
        {
            Dictionary = new Dictionary<string, Recipe>
            {
                {"IronAxe",     new Recipe(BuildingType.Forge,      ItemType.IronAxe,       new List<ItemType>{ItemType.IronIngot,  ItemType.Log},  5) },
                {"IronPickaxe", new Recipe(BuildingType.Forge,      ItemType.IronPickaxe,   new List<ItemType>{ItemType.IronIngot,  ItemType.Log},  5) },
                {"IronIngot",   new Recipe(BuildingType.Smelter,    ItemType.IronIngot,     new List<ItemType>{ItemType.IronOre,    ItemType.Coal}, 5) },
            };
        }

        public BuildingType Type    { get; private set; }
        public ItemType Output      { get; private set; }
        public List<ItemType> Input { get; private set; }
        public float Duration       { get; private set; }

        public Recipe(BuildingType type, ItemType output, List<ItemType> input, float duration = 1)
        {
            Type = type;
            Output = output;
            Input = input;
            Duration = duration;
        }
    }
}
//    public interface IHasRecipe
//    {
//        List<Recipe> Recipes { get; }
//    }

//    public class Recipe
//    {
//        public static Dictionary<string, Recipe> Dictionary = new Dictionary<string, Recipe>();
//        public static void Initialize()
//        {
//            Dictionary.Add("Creep", new Recipe(UnitBaseMelee.Dictionary["Creep"], 50));
//            Dictionary.Add("Miner", new Recipe(UnitBaseGatherer.Dictionary["Miner"], 25));
//            Dictionary.Add("Builder", new Recipe(UnitBaseBuilder.Dictionary["Builder"], 25));
//            Dictionary.Add("Stockpile", new Recipe(BuildingBase.Dictionary["Stockpile"], 100));

//            BuildingTownCenter.recipes.Add(Dictionary["Creep"]);
//            BuildingTownCenter.recipes.Add(Dictionary["Miner"]);
//            BuildingTownCenter.recipes.Add(Dictionary["Builder"]);

//            UnitBuilder.recipes.Add(Dictionary["Stockpile"]);
//        }

//        public Recipe(EntityBase Output, int Cost)
//        {
//            this.Output = Output;
//            this.Cost = Cost;
//        }

//        public EntityBase Output { get; }
//        public int Cost { get; }
//        public void Complete(Vector2 Position)
//        {
//            Game.Resources -= Cost;
//            EntityManager.Entities.Add(Activator.CreateInstance(Output.EntityType, Output, Position) as Entity);
//        }
//    }
//}