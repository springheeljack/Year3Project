using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
