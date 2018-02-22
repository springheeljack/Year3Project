using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame.Extension
{
    public static class SpriteBatchExtension
    {
        public static void DrawDoubleString(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            //spriteBatch.DrawString(spriteFont, text, position + new Vector2(0,1), Color.Black);
            //spriteBatch.DrawString(spriteFont, text, position + new Vector2(1,0), Color.Black);
            spriteBatch.DrawString(spriteFont, text, position, color);
        }
    }
}