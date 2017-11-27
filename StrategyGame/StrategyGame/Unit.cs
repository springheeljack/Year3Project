using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public abstract class Unit : ISelectable
    {
        public Texture2D Texture { get; }
        public string Name { get; }
        Point Size { get; }
        Vector2 Position { get; }
        public Rectangle DrawingRectangle { get; }
        

        public Unit(Vector2 Position,Texture2D Texture,string Name)
        {

        }

        public abstract void Update();
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DrawingRectangle, Color.White);
        }
    }
}
