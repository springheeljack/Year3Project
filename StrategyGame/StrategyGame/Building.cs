using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StrategyGame
{
    public interface IResourceDeposit : IRectangleObject
    {
        void Deposit(IGatherer Gatherer);
    }

    public class BuildingBase
    {
        public static Dictionary<string, BuildingBase> BaseDict = new Dictionary<string, BuildingBase>();
        public string Name { get; }
        public Point Size { get; }
        public int MaxHealth { get; }
        public Texture2D Texture { get; }
        public Type BuildingType { get; }
        public BuildingBase(Type BuildingType, string Name, Point Size, int MaxHealth)
        {
            this.Name = Name;
            this.Size = Size;
            this.MaxHealth = MaxHealth;
            Texture = Art.BuildingTextures[Name];
        }
        public static void Initialize()
        {
            BaseDict.Add("Town Center", new BuildingBase(typeof(BuildingTownCenter), "Town Center", new Point(128), 1000));
            BaseDict.Add("Stockpile", new BuildingBase(typeof(BuildingStockpile), "Stockpile", new Point(32), 100));
        }
    }

    public static class BuildingExtension
    {
        public static void Deposit(this IResourceDeposit resourceDeposit, IGatherer Gatherer)
        {
            Play.Resources += Gatherer.CarriedResources;
            Gatherer.CarriedResources = 0;
        }
    }

    public abstract class Building : IHealth
    {

        public static void InitializeRecipes()
        {
            BuildingTownCenter.Recipes.Add(UnitRecipe.UnitRecipes["Creep"]);
            BuildingTownCenter.Recipes.Add(UnitRecipe.UnitRecipes["Miner"]);
            BuildingTownCenter.Recipes.Add(UnitRecipe.UnitRecipes["Builder"]);
        }

        public Texture2D Texture { get { return Base.Texture; } }
        public Rectangle Rectangle { get; }
        public string Name { get { return Base.Name; } }
        public int Health { get; set; }
        public int MaxHealth { get { return Base.MaxHealth; } }
        public IAttacker LastAttacker { get; set; }
        public BuildingBase Base { get; }

        public Building(Point Position, BuildingBase Base)
        {
            this.Base = Base;
            Rectangle = new Rectangle(Position, Base.Size);
            Health = MaxHealth;
        }

        public abstract void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }

        void IHealth.Damage(IAttacker Attacker)
        {
            Health -= Attacker.Base.AttackDamage;
            LastAttacker = Attacker;
        }
    }

    public class BuildingStockpile : Building, IResourceDeposit
    {
        public static List<UnitRecipe> Recipes { get; set; } = new List<UnitRecipe>();
        public BuildingStockpile(Point Position) : base(Position, BuildingBase.BaseDict["Stockpile"])
        {
        }

        public override void Update(GameTime gameTime)
        {

        }

        //public List<SpawnRecipe> GetSpawnRecipes()
        //{
        //    return Recipes;
        //}
        public void Deposit(IGatherer Gatherer)
        {
            BuildingExtension.Deposit(this, Gatherer);
        }
    }

    public class BuildingTownCenter : Building, IHasUnitRecipe, IResourceDeposit
    {
        public static List<UnitRecipe> Recipes = new List<UnitRecipe>();
        public List<UnitRecipe> GetUnitRecipes() { return Recipes; }

        public BuildingTownCenter(Point Position) : base(Position, BuildingBase.BaseDict["Town Center"])
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
        public void Deposit(IGatherer Gatherer)
        {
            BuildingExtension.Deposit(this, Gatherer);
        }
    }
}