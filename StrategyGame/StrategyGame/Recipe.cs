using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public interface IHasRecipe
    {
        List<Recipe> Recipes { get; }
    }

    public class Recipe
    {
        public static Dictionary<string, Recipe> Dictionary = new Dictionary<string, Recipe>();
        public static void Initialize()
        {
            Dictionary.Add("Creep", new Recipe(UnitBaseMelee.Dictionary["Creep"], 50));
            Dictionary.Add("Miner", new Recipe(UnitBaseGatherer.Dictionary["Miner"], 25));
            Dictionary.Add("Builder", new Recipe(UnitBaseBuilder.Dictionary["Builder"], 25));
            Dictionary.Add("Stockpile", new Recipe(BuildingBase.Dictionary["Stockpile"], 100));

            BuildingTownCenter.recipes.Add(Dictionary["Creep"]);
            BuildingTownCenter.recipes.Add(Dictionary["Miner"]);
            BuildingTownCenter.recipes.Add(Dictionary["Builder"]);

            UnitBuilder.recipes.Add(Dictionary["Stockpile"]);
        }

        public Recipe(EntityBase Output, int Cost)
        {
            this.Output = Output;
            this.Cost = Cost;
        }

        public EntityBase Output { get; }
        public int Cost { get; }
        public void Complete(Vector2 Position)
        {
            Game.Resources -= Cost;
            EntityManager.Entities.Add(Activator.CreateInstance(Output.EntityType, Output, Position) as Entity);
        }
    }
}