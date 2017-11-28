using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public interface ISelectable:IRectangleObject
    {
        string Name { get; }
    }
}