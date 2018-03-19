using Microsoft.Xna.Framework;

namespace StrategyGame
{
    public static class PointExtension
    {
        public static Point Half (this Point p)
        {
            return new Point(p.X / 2, p.Y / 2);
        }
    }
}