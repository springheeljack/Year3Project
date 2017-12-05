using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public abstract class Entity
    {
        public EntityBase Base { get; }
        public Rectangle Rectangle { get; }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }

    public abstract class EntityBase
    {
        Texture2D Texture { get; }
        string Name { get; }
        Point Size { get; }
        EntityBase(string Name, Point Size, Texture2D Texture)
        {
            this.Name = Name;
            this.Size = Size;
            this.Texture = Texture;
        }
    }
}