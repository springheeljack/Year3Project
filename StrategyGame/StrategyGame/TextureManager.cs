using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public static class TextureManager
    {
        static SpriteFont spriteFont;
        public static SpriteFont SpriteFont
        {
            get { return spriteFont; }
        }

        static Dictionary<string, Texture2D> tileTextures;
        public static Dictionary<string, Texture2D> TileTextures
        {
            get { return tileTextures; }
        }

        static Dictionary<string, Texture2D> uiTextures;
        public static Dictionary<string, Texture2D> UITextures
        {
            get { return uiTextures; }
        }

        static string[] tilePaths = { "Grass", "Water", "Test" };
        static string[] uiPaths = { "Button", "Fade" };

        static TextureManager()
        {
            tileTextures = new Dictionary<string, Texture2D>();
            uiTextures = new Dictionary<string, Texture2D>();
        }

        public static void LoadContent(ContentManager Content)
        {
            spriteFont = Content.Load<SpriteFont>("Font/spriteFont");

            foreach (string s in tilePaths)
                tileTextures.Add(s, Content.Load<Texture2D>("Tile/"+s));
            foreach (string s in uiPaths)
                uiTextures.Add(s, Content.Load<Texture2D>("UI/" + s));
        }

        public static Vector2 CenterString(Rectangle rectangle, SpriteFont spriteFont, string text)
        {
            Vector2 vector = rectangle.Center.ToVector2();
            return vector - spriteFont.MeasureString(text)/2;
        }
    }
}