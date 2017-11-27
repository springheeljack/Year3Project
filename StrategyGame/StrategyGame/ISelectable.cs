using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public interface ISelectable
    {
        string Name { get; }
        Texture2D Texture { get; }
        Rectangle DrawingRectangle { get; }
    }
}