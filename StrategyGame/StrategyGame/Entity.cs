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
        public Rectangle Rectangle { get; set; }
        public Vector2 Position { get; protected set; }
        public string Name { get; protected set; }
        public Point Size { get; protected set; }
        public Texture2D Texture { get; protected set; }

        public Entity(Vector2 position,string name,Point size, Texture2D texture)
        {
            Position = position;
            Name = name;
            Size = size;
            Texture = texture;
            UpdateRectangle();
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateRectangle();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle,Color.White);
        }

        private void UpdateRectangle()
        {
            Rectangle = new Rectangle(Position.ToPoint() - Size.Half(), Size);
        }
    }

    public static class EntityManager
    {
        public static List<Entity> Entities { get; set; } = new List<Entity>();
        public static List<Entity> ToRemove { get; set; } = new List<Entity>();
        public static List<Entity> ToAdd { get; set; } = new List<Entity>();
        public static void Update(GameTime gameTime)
        {
            foreach (Entity e in Entities)
            {
                e.Update(gameTime);
                //if (e.ToRemove())
                //    ToRemove.Add(e);
            }
            foreach (Entity e in ToRemove)
                Entities.Remove(e);
            ToRemove.Clear();
            foreach (Entity e in ToAdd)
                Entities.Add(e);
            ToAdd.Clear();
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity e in Entities)
                e.Draw(spriteBatch);
        }

        public static List<Building> GetBuildings()
        {
            return Entities.OfType<Building>().ToList();
        }

        public static List<ResourceNode> GetResourceNodes()
        {
            return Entities.OfType<ResourceNode>().ToList();
        }
    }

    //public abstract class Entity : IRectangleObject
    //{
    //    public EntityBase Base { get; }
    //    public Rectangle Rectangle { get; set; }
    //    public Vector2 Position { get; set; }
    //    public Color Color { get; set; }
    //    public Entity(EntityBase Base, Vector2 Position)
    //    {
    //        this.Base = Base;
    //        this.Position = Position;
    //        Color = Color.White;
    //        UpdateRectangle();
    //    }
    //    public virtual void Update(GameTime gameTime)
    //    {
    //        if (Input.MouseState.IsLeftClicked() && Input.MouseRectangle.Intersects(Rectangle))
    //            if (this is Building || this is Unit)
    //                Game.SelectedEntity = this;

    //        UpdateRectangle();
    //    }
    //    public virtual void Draw(SpriteBatch spriteBatch)
    //    {
    //        spriteBatch.Draw(Base.Texture, Rectangle, null, Color, 0.0f, Vector2.Zero, SpriteEffects.None, Base.LayerDepth);

    //        if (Game.SelectedEntity == this)
    //            spriteBatch.Draw(Art.Textures["Reticle"], Rectangle, null, Color, 0.0f, Vector2.Zero, SpriteEffects.None, Game.SelectedLayerDepth);
    //    }
    //    void UpdateRectangle()
    //    {
    //        Rectangle = new Rectangle(Position.ToPoint() - Base.Size.Half(), Base.Size);
    //    }
    //    public void Remove()
    //    {
    //        EntityManager.ToRemove.Add(this);
    //    }
    //}

    //public abstract class EntityBase
    //{
    //    public Texture2D Texture { get; }
    //    public string Name { get; }
    //    public Point Size { get; }
    //    public Type EntityType { get; }
    //    public bool Selectable { get; }
    //    public float LayerDepth { get; }
    //    public EntityBase(Type EntityType, string Name, Point Size, bool Selectable, float LayerDepth)
    //    {
    //        this.EntityType = EntityType;
    //        this.Name = Name;
    //        this.Size = Size;
    //        this.Selectable = Selectable;
    //        this.LayerDepth = LayerDepth;
    //        Texture = Name == "Text" || Name == "Selector List" ? null : Art.Textures[Name];
    //    }
    //}

    //public static class EntityManager
    //{
    //    public static List<Entity> Entities { get; set; } = new List<Entity>();
    //    public static List<Entity> ToRemove { get; set; } = new List<Entity>();
    //    public static List<Entity> ToAdd { get; set; } = new List<Entity>();
    //    public static void Update(GameTime gameTime)
    //    {
    //        foreach (Entity e in Entities)
    //        {
    //            e.Update(gameTime);
    //            //if (e.ToRemove())
    //            //    ToRemove.Add(e);
    //        }
    //        foreach (Entity e in ToRemove)
    //            Entities.Remove(e);
    //        ToRemove.Clear();
    //        foreach (Entity e in ToAdd)
    //            Entities.Add(e);
    //        ToAdd.Clear();
    //    }
    //    public static void Draw(SpriteBatch spriteBatch)
    //    {
    //        foreach (Entity e in Entities)
    //            e.Draw(spriteBatch);
    //    }
    //}
}