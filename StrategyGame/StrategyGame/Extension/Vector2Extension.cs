using Microsoft.Xna.Framework;

namespace StrategyGame
{
    public static class Vector2Extension
    {
        public static float Distance(this Vector2 v1, Vector2 v2)
        {
            return (v2 - v1).Length();
        }
    }
}