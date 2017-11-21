using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public interface ISelectable
    {
        Texture2D GetTexture();
        string GetName();
        Rectangle GetDrawingRectangle();
    }
}