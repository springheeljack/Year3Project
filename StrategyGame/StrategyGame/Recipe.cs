using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public interface IHasSpawnRecipe
    {
        List<SpawnRecipe> GetSpawnRecipes();
    }

    public class SpawnRecipe
    {
        public static Dictionary<string, SpawnRecipe> UnitRecipes = new Dictionary<string, SpawnRecipe>();

        public static void Initialize()
        {
            UnitRecipes.Add("Creep", new SpawnRecipe(MeleeUnitBase.BaseDict["Creep"],50));
            UnitRecipes.Add("Miner", new SpawnRecipe(GathererUnitBase.BaseDict["Miner"],25));
        }

        public UnitBase RecipeOutput { get; }
        public int Cost { get; }

        public SpawnRecipe(UnitBase RecipeOutput,int Cost)
        {
            this.RecipeOutput = RecipeOutput;
            this.Cost = Cost;
        }

        public void Output(Vector2 Position)
        {
            Play.Resources -= Cost;
            if (RecipeOutput is MeleeUnitBase)
                Play.Units.Add(new UnitMelee(Position, RecipeOutput as MeleeUnitBase));
            if (RecipeOutput is GathererUnitBase)
                Play.Units.Add(new UnitMiner(Position, RecipeOutput as GathererUnitBase));
        }
    }
}