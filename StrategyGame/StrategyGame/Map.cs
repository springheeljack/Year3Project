using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class Map
    {
        Tile[,] tiles;
        int width, height;

        public bool LoadMap(string mapPath)
        {
            width = 40;
            height = 20;

            tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    tiles[x, y] = new Tile(TextureManager.TileTextures["Grass"], x, y, 1);

            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile t in tiles)
                t.Draw(spriteBatch);
        }
    }
}
