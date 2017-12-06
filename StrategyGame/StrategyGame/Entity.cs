using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public abstract class Entity : IRectangleObject
    {
        public EntityBase Base { get; }
        public Rectangle Rectangle { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Entity(EntityBase Base,Vector2 Position)
        {
            this.Base = Base;
            this.Position = Position;
            Color = Color.White;
            UpdateRectangle();
        }
        public virtual void Update(GameTime gameTime)
        {
            UpdateRectangle();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Base.Texture, Rectangle, Color);
        }
        void UpdateRectangle()
        {
            Rectangle = new Rectangle(Position.ToPoint()-Base.Size.Half(), Base.Size);
        }
        public abstract bool ToRemove();
    }

    public abstract class EntityBase
    {
        public Texture2D Texture { get; }
        public string Name { get; }
        public Point Size { get; }
        public Type EntityType { get; }
        public bool Selectable { get; }
        public EntityBase(Type EntityType, string Name, Point Size,bool Selectable)
        {
            this.EntityType = EntityType;
            this.Name = Name;
            this.Size = Size;
            this.Selectable = Selectable;
            Texture = Art.Textures[Name];
        }
    }

    public static class EntityManager
    {
        public static List<Entity> Entities { get; set; } = new List<Entity>();
        public static List<Entity> ToRemove { get; set; } = new List<Entity>();
        public static void Update(GameTime gameTime)
        {
            foreach (Entity e in Entities)
            {
                e.Update(gameTime);
                if (e.ToRemove())
                    ToRemove.Add(e);
            }
            foreach (Entity e in ToRemove)
                Entities.Remove(e);
            ToRemove.Clear();
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity e in Entities)
                e.Draw(spriteBatch);
        }
    }
}