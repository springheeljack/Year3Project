using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public abstract class Building : IHealth
    {
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

        public Building(Point TilePosition,Point TileSize,Texture2D Texture,string Name,int MaxHealth)
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

        public abstract void Update();
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }

        void IHealth.Damage(IAttacker Attacker)
        {
            Health -= Attacker.AttackDamage;
            LastAttacker = Attacker;
        }
    }

    public class BuildingStockpile : Building
    {
        new static string Name = "Stockpile";
        new static int MaxHealth = 100;
        static Point TileSize = new Point(1);
        public BuildingStockpile(Point TilePosition) : base(TilePosition, TileSize,TextureManager.BuildingTextures[Name],Name,MaxHealth)
        {
        }

        public override void Update()
        {
            //throw new System.NotImplementedException();
        }
    }

    public class BuildingTownCenter : Building
    {
        new static string Name = "Town Center";
        new static int MaxHealth = 1000;
        static Point TileSize = new Point(4);
        public BuildingTownCenter(Point TilePosition) : base(TilePosition, TileSize, TextureManager.BuildingTextures[Name], Name,MaxHealth)
        {
        }

        public override void Update()
        {
            //throw new System.NotImplementedException();
        }
    }
}