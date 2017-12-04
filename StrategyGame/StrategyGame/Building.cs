using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StrategyGame
{
    //public interface IBuildingBase
    //{
    //    string Name { get; }
    //    Point Size { get; }
    //    int MaxHealth { get; }
    //    Texture2D Texture { get; }
    //}

    //public static class StockpileBuildingBase : IBuildingBase, IHasRecipes
    //{
    //    public static string Name = "Stockpile";
    //    public static Point Size = new Point(1);
    //    public static int MaxHealth = 100;
    //    public static Texture2D Texture = TextureManager.BuildingTextures[Name];
    //    public static List<Recipe> Recipes { get; set; }

    //    public StockpileBuildingBase(string Name, Point Size, int MaxHealth, int AttackDamage, float AttackSpeed, float Speed)
    //    {
    //        this.Name = Name;
    //        this.Size = Size;
    //        this.MaxHealth = MaxHealth;
    //        Texture = TextureManager.BuildingTextures[Name];

    //        Recipes.Add(new RecipeSpawn(UnitBases.Melee["Creep"]));
    //    }
    //}

    public interface IResourceDeposit : IRectangleObject
    {
        void Deposit(IGatherer Gatherer);
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
            BuildingTownCenter.Recipes.Add(SpawnRecipe.UnitRecipes["Creep"]);
            BuildingTownCenter.Recipes.Add(SpawnRecipe.UnitRecipes["Miner"]);
        }

        //Point TilePosition { get; }
        //Point DrawingPosition { get; }
        //Point TileSize { get; }
        //Point DrawingSize { get; }
        public Texture2D Texture { get; }
        public Rectangle Rectangle { get; }
        public string Name { get; }
        public int Health { get; set; }
        public int MaxHealth { get; }
        public IAttacker LastAttacker { get; set; }
        //public IBuildingBase Base { get; }

        public Building(Point TilePosition, Point TileSize, Texture2D Texture, string Name, int MaxHealth)
        {
            //this.TilePosition = TilePosition;
            //DrawingPosition = new Point(TilePosition.X * Game.TileSizeScaled, TilePosition.Y * Game.TileSizeScaled);
            //this.TileSize = TileSize;
            //DrawingSize = new Point(TileSize.X * Game.TileSizeScaled, TileSize.Y * Game.TileSizeScaled);
            //Rectangle = new Rectangle(DrawingPosition, DrawingSize);

            Rectangle = new Rectangle(
                new Point(TilePosition.X * Game.TileSizeScaled, TilePosition.Y * Game.TileSizeScaled),
                new Point(TileSize.X * Game.TileSizeScaled, TileSize.Y * Game.TileSizeScaled)
                );
            this.Texture = Texture;
            this.Name = Name;
            this.MaxHealth = MaxHealth;
            Health = MaxHealth;
        }

        public abstract void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }

        void IHealth.Damage(IAttacker Attacker)
        {
            Health -= Attacker.UnitBase.AttackDamage;
            LastAttacker = Attacker;
        }
    }

    public class BuildingStockpile : Building, IHasSpawnRecipe, IResourceDeposit
    {
        public static List<SpawnRecipe> Recipes { get; set; } = new List<SpawnRecipe>();
        new static string Name = "Stockpile";
        new static int MaxHealth = 100;
        static Point TileSize = new Point(1);
        //static StockpileBuildingBase Base = 
        public BuildingStockpile(Point TilePosition) : base(TilePosition, TileSize, TextureManager.BuildingTextures[Name], Name, MaxHealth)
        {
        }

        public override void Update(GameTime gameTime)
        {
            //throw new System.NotImplementedException();
        }

        public List<SpawnRecipe> GetSpawnRecipes()
        {
            return Recipes;
        }
        public void Deposit(IGatherer Gatherer)
        {
            BuildingExtension.Deposit(this, Gatherer);
        }
    }

    public class BuildingTownCenter : Building, IHasSpawnRecipe, IResourceDeposit
    {
        public static List<SpawnRecipe> Recipes { get; set; } = new List<SpawnRecipe>();
        new static string Name = "Town Center";
        new static int MaxHealth = 1000;
        static Point TileSize = new Point(4);
        public BuildingTownCenter(Point TilePosition) : base(TilePosition, TileSize, TextureManager.BuildingTextures[Name], Name, MaxHealth)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public List<SpawnRecipe> GetSpawnRecipes()
        {
            return Recipes;
        }
        public void Deposit(IGatherer Gatherer)
        {
            BuildingExtension.Deposit(this,Gatherer);
        }
    }
}