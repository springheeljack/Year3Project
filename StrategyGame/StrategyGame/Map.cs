using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace StrategyGame
{
    public static class Map
    {
        static Tile[,] tiles;
        static int width, height;
        public static int Width { get { return width; } }
        public static int Height { get { return height; } }
        public static List<string> MapList { get; } = new List<string>();
        public static bool Loaded { get; private set; } = false;

        public static void Initialize()
        {
            UpdateMapList();
        }

        public static void UpdateMapList()
        {
            MapList.Clear();
            foreach (string file in Directory.GetFiles("Content/Map"))
            {
                //string s = TexturePath + "/" + Path.GetFileName(dir) + "/" + Path.GetFileNameWithoutExtension(file);
                //Textures.Add(Path.GetFileNameWithoutExtension(file), Content.Load<Texture2D>(s));
                MapList.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        public static bool LoadMap(string filePath)
        {   if (!System.IO.File.Exists("Content/Map/" + filePath + ".sgmap"))
                throw new System.Exception("Could not find default map.");
            string[] read;
            read = System.IO.File.ReadAllLines("Content/Map/" + filePath + ".sgmap");
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
                        tiles[x, y] = new Tile(Art.Textures[Art.TilePaths[tileTexture]], x, y, cost, tileTexture);
                        x++;
                    }
                    else
                    {
                        temp += read[y + 2][i];
                    }
                }
                temp = "";
            }

            EntityManager.ToAdd.Add(new BuildingTownCenter(BuildingBase.Dictionary["Town Center"], new Vector2(100)));

            Loaded = true;
            return true;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile t in tiles)
                t.Draw(spriteBatch);
        }

        public static void ChangeTile(int X, int Y, string Tile,int TileIndex)
        {
            tiles[X, Y].Texture = Art.Textures[Tile];
            tiles[X, Y].TextureIndex = TileIndex;
        }

        public static void SaveMap(string filePath)
        {
            System.IO.File.Delete("Content/Map/" + filePath + ".sgmap");
            string data = "";
            data += width.ToString() + "\r\n";
            data += height.ToString() + "\r\n";
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    data += tiles[x, y].TextureIndex.ToString() + "," + tiles[x, y].Cost + "|";
                }
                data += "\r\n";
            }
            System.IO.File.WriteAllText("Content/Map/" + filePath + ".sgmap", data);
        }
    }
}