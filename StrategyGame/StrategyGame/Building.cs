using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public abstract class Building : ISelectable
    {
        Point TilePosition { get; }
        Point DrawingPosition { get; }
        Point TileSize { get; }
        Point DrawingSize { get; }
        Texture2D Texture { get; }
        public Rectangle DrawingRectangle { get; }
        string Name { get; }

        public Building(Point TilePosition,Point TileSize,Texture2D Texture,string Name)
        {
            this.TilePosition = TilePosition;
            DrawingPosition = new Point(TilePosition.X * Game.TileSizeScaled, TilePosition.Y * Game.TileSizeScaled);
            this.TileSize = TileSize;
            DrawingSize = new Point(TileSize.X * Game.TileSizeScaled, TileSize.Y * Game.TileSizeScaled);
            this.Texture = Texture;
            DrawingRectangle = new Rectangle(DrawingPosition, DrawingSize);
            this.Name = Name;
        }

        public abstract void Update();
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DrawingRectangle, Color.White);
        }

        public Texture2D GetTexture()
        {
            return Texture;
        }

        public string GetName()
        {
            return Name;
        }

        public Rectangle GetDrawingRectangle()
        {
            return DrawingRectangle;
        }
    }

    public class BuildingStockpile : Building
    {
        static string Name = "Stockpile";
        static Point TileSize = new Point(1);
        public BuildingStockpile(Point TilePosition) : base(TilePosition, TileSize,TextureManager.BuildingTextures[Name],Name)
        {
        }

        public override void Update()
        {
            //throw new System.NotImplementedException();
        }
    }

    public class BuildingTownCenter : Building
    {
        static string Name = "Town Center";
        static Point TileSize = new Point(4);
        public BuildingTownCenter(Point TilePosition) : base(TilePosition, TileSize, TextureManager.BuildingTextures[Name], Name)
        {
        }

        public override void Update()
        {
            //throw new System.NotImplementedException();
        }
    }
}