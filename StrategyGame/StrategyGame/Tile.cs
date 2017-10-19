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
            position = new Point(X*Game.TileSize*Game.GameScale, Y * Game.TileSize * Game.GameScale);
            cost = Cost;
            rectangle = texture.Bounds;
            rectangle.Offset(position);
            rectangle.Width *= Game.GameScale;
            rectangle.Height *= Game.GameScale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
