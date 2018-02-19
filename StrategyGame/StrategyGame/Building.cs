using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StrategyGame
{
    public enum BuildingType
    {
        Stockpile,
        Forge,
        Smelter
    }

    public class BuildingBase
    {
        public static void Initialize()
        {
            Bases.Add("Stockpile", new BuildingBase(BuildingType.Stockpile, "Stockpile", new Point(64), Art.Textures["Stockpile"], -1));
            Bases.Add("Forge", new BuildingBase(BuildingType.Forge, "Forge", new Point(64), Art.Textures["Forge"], 25));
            Bases.Add("Smelter", new BuildingBase(BuildingType.Smelter, "Smelter", new Point(64), Art.Textures["Smelter"], 25));
        }

        public static Dictionary<string, BuildingBase> Bases = new Dictionary<string, BuildingBase>();

        public string Name { get; private set; }
        public Point Size { get; private set; }
        public Texture2D Texture { get; private set; }
        public int InventoryCapacity { get; private set; }
        public BuildingType BuildingType { get; private set; }

        public BuildingBase(BuildingType buildingType, string name, Point size, Texture2D texture, int inventoryCapacity)
        {
            BuildingType = buildingType;
            Name = name;
            Size = size;
            Texture = texture;
            InventoryCapacity = inventoryCapacity;
        }
    }

    public class Building : Entity
    {
        public BuildingType BuildingType { get; private set; }
        public Inventory Inventory { get; private set; }
        public Building(BuildingBase buildingBase, Vector2 position) : base(position, buildingBase.Name, buildingBase.Size, buildingBase.Texture)
        {
            Inventory = buildingBase.InventoryCapacity == 0 ? null : new Inventory(buildingBase.InventoryCapacity);
            BuildingType = buildingBase.BuildingType;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}

//public class BuildingBase : EntityBase
//{
//    new static readonly float LayerDepth = 0.5f;
//    public static Dictionary<string, BuildingBase> Dictionary = new Dictionary<string, BuildingBase>();
//    public int MaxHealth { get; }
//    public bool Depositable { get; }
//    public BuildingBase(Type BuildingType, string Name, Point Size, int MaxHealth, bool Depositable, bool Selectable) : base(BuildingType, Name, Size, Selectable, LayerDepth)
//    {
//        this.MaxHealth = MaxHealth;
//        this.Depositable = Depositable;
//    }
//    public static void Initialize()
//    {
//        Dictionary.Add("Town Center", new BuildingBase(typeof(BuildingTownCenter), "Town Center", new Point(128), 1000, true, true));
//        Dictionary.Add("Stockpile", new BuildingBase(typeof(BuildingStockpile), "Stockpile", new Point(32), 100, true, true));
//    }
//}

//public abstract class Building : Entity, IHealth
//{
//    public int Health { get; set; }
//    public int MaxHealth { get { return (Base as BuildingBase).MaxHealth; } }
//    public IAttacker LastAttacker { get; set; }

//    public Building(BuildingBase Base, Vector2 Position) : base(Base, Position)
//    {
//        Health = MaxHealth;
//    }

//    void IHealth.Damage(IAttacker Attacker)
//    {
//        Health -= Attacker.Base.AttackDamage;
//        LastAttacker = Attacker;
//    }
//}

//public class BuildingStockpile : Building
//{
//    public static List<Recipe> Recipes { get; set; } = new List<Recipe>();
//    public BuildingStockpile(BuildingBase Base, Vector2 Position) : base(Base, Position)
//    {
//    }


//    public void Deposit(IGatherer Gatherer)
//    {
//        BuildingExtension.Deposit(this, Gatherer);
//    }
//}

//public class BuildingTownCenter : Building, IHasRecipe
//{
//    public static List<Recipe> recipes = new List<Recipe>();
//    public List<Recipe> Recipes { get { return recipes; } }

//    public BuildingTownCenter(BuildingBase Base, Vector2 Position) : base(Base, Position)
//    {

//    }

//    public override void Update(GameTime gameTime)
//    {
//    }
//    public void Deposit(IGatherer Gatherer)
//    {
//        BuildingExtension.Deposit(this, Gatherer);
//    }
//}

//public static class BuildingExtension
//{
//    public static void Deposit(this Building building, IGatherer Gatherer)
//    {
//        Game.Resources += Gatherer.CarriedResources;
//        Gatherer.CarriedResources = 0;
//    }
//}