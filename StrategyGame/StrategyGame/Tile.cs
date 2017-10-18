using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class Tile
    {
        Texture2D texture;
        int cost;
        Point position;
        Rectangle rectangle;

        public Tile(Texture2D texture, int X, int Y, int Cost)
        {
            this.texture = texture;
            position = new Point(X, Y);
            cost = Cost;
            rectangle = texture.Bounds;
            rectangle.Offset(position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, texture.Bounds, Color.White);
        }
    }
}
