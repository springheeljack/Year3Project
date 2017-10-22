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

        static string[] tileTextures = { "Grass", "Water", "Test" };

        public bool LoadMap(string mapPath)
        {
            string[] read;
            read = System.IO.File.ReadAllLines("Content/Map/" + mapPath + ".sgmap");
            width = int.Parse(read[0]);
            height = int.Parse(read[1]);

            tiles = new Tile[width, height];

            string temp = "";
            int tileTexture = 0;
            int cost;
            for (int y = 0; y < height; y++)
            {
                int x = 0;
                for (int i = 0; i < read[y + 2].Length; i++)
                {
                    if (read[y + 2][i] == ',')
                    {
                        tileTexture = int.Parse(temp);
                        temp = "";
                    }
                    else if (read[y + 2][i] == '|')
                    {
                        cost = int.Parse(temp);
                        temp = "";
                        tiles[x, y] = new Tile(TextureManager.TileTextures[tileTextures[tileTexture]], x, y, cost);
                        x++;
                    }
                    else
                    {
                        temp += read[y + 2][i];
                    }
                }
                temp = "";
            }

            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile t in tiles)
                t.Draw(spriteBatch);
        }
    }
}