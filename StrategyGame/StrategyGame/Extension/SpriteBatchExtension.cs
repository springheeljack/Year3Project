using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Extension
{
    public static class SpriteBatchExtension
    {
        public static void DrawDoubleString(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(spriteFont, text, position + Vector2.One, Color.DarkGray);
            spriteBatch.DrawString(spriteFont, text, position, color);
        }
    }
}