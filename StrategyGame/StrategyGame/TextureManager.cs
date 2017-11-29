using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StrategyGame
{
    public static class TextureManager
    {
        public static SpriteFont SpriteFont { get; set; }
        public static Dictionary<string, Texture2D> TileTextures { get; } = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> UITextures { get; } = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> BuildingTextures { get; } = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> UnitTextures { get; } = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> ResourceNodeTextures { get; } = new Dictionary<string, Texture2D>();

        public static string[] buildingPaths = { "Stockpile", "Town Center" };
        public static string[] tilePaths = { "Grass", "Water", "Test" };
        static string[] uiPaths = { "Button", "Fade", "Selector", "Flag","Reticle" };
        public static string[] unitPaths = { "Creep","Miner" };
        public static string[] resourceNodePaths = { "Iron Rock", "Tree" };

        public static Point ReticleAndFlagOffset { get; } = new Point(-16);

        static TextureManager()
        {
        }

        public static void LoadContent(ContentManager Content)
        {
            SpriteFont = Content.Load<SpriteFont>("Font/spriteFont");

            foreach (string s in tilePaths)
                TileTextures.Add(s, Content.Load<Texture2D>("Tile/" + s));
            foreach (string s in uiPaths)
                UITextures.Add(s, Content.Load<Texture2D>("UI/" + s));
            foreach (string s in buildingPaths)
                BuildingTextures.Add(s, Content.Load<Texture2D>("Building/" + s));
            foreach (string s in unitPaths)
                UnitTextures.Add(s, Content.Load<Texture2D>("Unit/" + s));
            foreach (string s in resourceNodePaths)
                ResourceNodeTextures.Add(s, Content.Load<Texture2D>("ResourceNode/" + s));
        }

        public static Vector2 CenterString(Rectangle rectangle, SpriteFont spriteFont, string text)
        {
            Vector2 vector = rectangle.Center.ToVector2();
            return vector - spriteFont.MeasureString(text)/2;
        }
    }
}