using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    //public abstract class Recipe
    //{

    //}

    public interface IHasSpawnRecipe
    {
        List<SpawnRecipe> GetSpawnRecipes();
    }

    public class SpawnRecipe/* : Recipe*/
    {
        public static Dictionary<string, SpawnRecipe> UnitRecipes = new Dictionary<string, SpawnRecipe>();

        public static void Initialize()
        {
            UnitRecipes.Add("Creep", new SpawnRecipe(MeleeUnitBase.BaseDict["Creep"]));
            UnitRecipes.Add("Miner", new SpawnRecipe(GathererUnitBase.BaseDict["Miner"]));
        }

        public UnitBase RecipeOutput { get; }

        public SpawnRecipe(UnitBase RecipeOutput)
        {
            this.RecipeOutput = RecipeOutput;
        }

        public void Output(Vector2 Position)
        {
            if (RecipeOutput is MeleeUnitBase)
                Play.Units.Add(new UnitMelee(Position, RecipeOutput as MeleeUnitBase));
            if (RecipeOutput is GathererUnitBase)
                Play.Units.Add(new UnitMiner(Position, RecipeOutput as GathererUnitBase));
        }
    }
}