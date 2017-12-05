using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public interface IHasUnitRecipe
    {
        List<UnitRecipe> GetUnitRecipes();
    }

    public interface IHasBuildRecipe
    {
        List<BuildingRecipe> GetBuildingRecipes();
    }

    public class BuildingRecipe
    {
        public static Dictionary<string, BuildingRecipe> BuildingRecipes = new Dictionary<string, BuildingRecipe>();

        public static void Initialize()
        {
            BuildingRecipes.Add("Stockpile", new BuildingRecipe(BuildingBase.BaseDict["Stockpile"], 100));
        }

        public BuildingBase RecipeOutput { get; }
        public int Cost { get; }
        public BuildingRecipe(BuildingBase RecipeOutput,int Cost)
        {
            this.RecipeOutput = RecipeOutput;
            this.Cost = Cost;
        }

        public void Output(Point Position)
        {
            Play.Resources -= Cost;
            Play.Buildings.Add(Activator.CreateInstance(RecipeOutput.BuildingType, Position) as Building);
        }
    }

    public class UnitRecipe
    {
        public static Dictionary<string, UnitRecipe> UnitRecipes = new Dictionary<string, UnitRecipe>();

        public static void Initialize()
        {
            UnitRecipes.Add("Creep", new UnitRecipe(UnitBaseMelee.Dictionary["Creep"],50));
            UnitRecipes.Add("Miner", new UnitRecipe(UnitBaseGatherer.Dictionary["Miner"],25));
            UnitRecipes.Add("Builder", new UnitRecipe(UnitBaseBuilder.Dictionary["Builder"], 25));
        }

        public UnitBase RecipeOutput { get; }
        public int Cost { get; }

        public UnitRecipe(UnitBase RecipeOutput,int Cost)
        {
            this.RecipeOutput = RecipeOutput;
            this.Cost = Cost;
        }

        public void Output(Vector2 Position)
        {
            Play.Resources -= Cost;
            Play.Units.Add(Activator.CreateInstance(RecipeOutput.UnitType, Position, RecipeOutput) as Unit);
        }
    }
}