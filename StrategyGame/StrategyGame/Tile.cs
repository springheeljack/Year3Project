using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public class Tile
    {
        Texture2D texture;
        public Texture2D Texture { set { texture = value; } }
        int textureIndex;
        public int TextureIndex { get { return textureIndex; } set { textureIndex = value; } }
        int cost;
        public int Cost { get { return cost; } }
        Point position;
        Rectangle rectangle;

        public Tile(Texture2D texture, int X, int Y, int Cost, int TextureIndex)
        {
            this.texture = texture;
            position = new Point(X*Game.TileSize*Game.GameScale, Y * Game.TileSize * Game.GameScale);
            cost = Cost;
            rectangle = texture.Bounds;
            rectangle.Offset(position);
            rectangle.Width *= Game.GameScale;
            rectangle.Height *= Game.GameScale;
            textureIndex = TextureIndex;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}