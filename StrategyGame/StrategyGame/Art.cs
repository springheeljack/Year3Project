﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace StrategyGame
{
    public static class Art
    {
        public static SpriteFont SpriteFont { get; set; }
        public static Dictionary<string, Texture2D> Textures { get; } = new Dictionary<string, Texture2D>();

        public static string[] TilePaths = { "Grass", "Water", "Test" };

        public static Point ReticleAndFlagOffset { get; } = new Point(-16);

        public static string TexturePath { get; } = "Texture";

        public static void LoadContent(ContentManager Content)
        {
            SpriteFont = Content.Load<SpriteFont>("Font/spriteFont");

            foreach (string dir in Directory.GetDirectories("Content/"+TexturePath))
                foreach (string file in Directory.GetFiles(dir))
                {
                   string s = TexturePath + "/"+Path.GetFileName(dir) + "/"+Path.GetFileNameWithoutExtension(file);
                   Textures.Add(Path.GetFileNameWithoutExtension(file), Content.Load<Texture2D>(s));
                }

        }

        public static Vector2 CenterString(Rectangle rectangle, SpriteFont spriteFont, string text)
        {
            Vector2 vector = rectangle.Center.ToVector2();
            return vector - spriteFont.MeasureString(text) / 2;
        }
    }
}