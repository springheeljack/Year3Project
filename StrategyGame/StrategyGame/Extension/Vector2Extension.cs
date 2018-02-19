using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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