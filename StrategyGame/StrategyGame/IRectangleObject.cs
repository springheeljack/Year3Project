using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public interface IRectangleObject
    {
        Texture2D Texture { get; }
        Rectangle Rectangle { get; }
    }
}