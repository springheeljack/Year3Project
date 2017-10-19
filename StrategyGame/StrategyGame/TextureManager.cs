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
        static Dictionary<string, Texture2D> tileTextures;
        public static Dictionary<string, Texture2D> TileTextures
        {
            get { return tileTextures; }
        }

        static Dictionary<string, Texture2D> uiTextures;
        public static Dictionary<string, Texture2D> UITextures;

        static string[] tilePaths = { "Grass", "Water", "Test" };

        static TextureManager()
        {
            tileTextures = new Dictionary<string, Texture2D>();
        }

        public static void LoadContent(ContentManager Content)
        {
            foreach (string s in tilePaths)
                tileTextures.Add(s, Content.Load<Texture2D>("Tile/"+s));
        }
    }
}