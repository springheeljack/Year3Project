using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public abstract class Unit : IHealth
    {
        public Texture2D Texture { get; }
        public string Name { get; }
        Point Size { get; }
        Point DrawingSize { get; }
        public Vector2 Position { get; set; }
        public Vector2 Destination { get; set; }
        public bool HasDestination { get; set; } = false;
        public Rectangle Rectangle { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; }
        public float Speed { get; }
        public IAttacker LastAttacker { get; set; }

        public Unit(Vector2 Position, Point Size, Texture2D Texture, string Name, int MaxHealth, float Speed)
        {
            this.Position = Position;
            this.Size = Size;
            this.Texture = Texture;
            this.Name = Name;
            DrawingSize = new Point(Size.X * Game.GameScale, Size.Y * Game.GameScale);
            this.MaxHealth = MaxHealth;
            Health = MaxHealth;
            this.Speed = Speed;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (HasDestination)
            {
                Vector2 pos = Position;
                Vector2 move = Destination - pos;
                float length = move.Length();
                if (length > Speed)
                {
                    move.Normalize();
                    move *= Speed;
                }
                pos += move;
                Position = pos;
                if (length <= 0.1f)
                    HasDestination = false;
            }

            Rectangle = new Rectangle(Position.ToPoint() - Size, DrawingSize);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
        public void SetDestination(Vector2 Destination)
        {
            HasDestination = true;
            this.Destination = Destination;
        }

        public void DrawDestinationFlag(SpriteBatch spriteBatch)
        {
            Texture2D flagTexture = TextureManager.UITextures["Flag"];
            spriteBatch.Draw(flagTexture, new Rectangle(Destination.ToPoint() + TextureManager.ReticleAndFlagOffset, flagTexture.Bounds.Size), Color.White);
        }

        void IHealth.Damage(IAttacker Attacker)
        {
            Health -= Attacker.AttackDamage;
            LastAttacker = Attacker;
        }
    }

    public class UnitMelee : Unit, IAttacker
    {
        public int AttackDamage { get; }
        public float AttackSpeed { get; }
        public float AttackTimer { get; set; } = 0.0f;
        public IHealth AttackTarget { get; set; } = null;

        public UnitMelee(Vector2 Position, Point Size, Texture2D Texture, string Name, int MaxHealth, int AttackDamage, float AttackSpeed, float Speed) : base(Position, Size, TextureManager.UnitTextures[Name], Name, MaxHealth, Speed)
        {
            this.AttackDamage = AttackDamage;
            this.AttackSpeed = AttackSpeed;
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////// Moving with collision
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime ///////////////////missing a bracket lol look above
        {
            if (AttackTarget != null)
            {
                if (AttackTarget.Health <= 0)
                    AttackTarget = null;
                else
                {
                    Vector2 pos = Position;
                    Vector2 move = AttackTarget.Rectangle.Center.ToVector2() - pos;
                    float length = move.Length();
                    if (length > Speed)
                    {
                        move.Normalize();
                        move *= Speed;
                    }
                    pos += move;
                    Position = pos;
                    if (length <= 0.1f)
                    {
                        if (AttackTimer <= 0.0f)
                        {
                            AttackTimer += AttackSpeed;
                            AttackTarget.Damage(this);
                        }
                    }
                }
            }

            if (AttackTimer > 0.0f)
                AttackTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public void DrawAttackReticle(SpriteBatch spriteBatch)
        {
            Texture2D reticleTexture = TextureManager.UITextures["Reticle"];
            spriteBatch.Draw(reticleTexture, new Rectangle(AttackTarget.Rectangle.Center + TextureManager.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
        }
    }

    public class UnitCreep : UnitMelee
    {
        new static string Name = "Creep";
        static Point Size = new Point(8);
        new static int MaxHealth = 50;
        new static int AttackDamage = 5;
        new static float AttackSpeed = 1.0f;
        new static float Speed = 2.0f;

        public UnitCreep(Vector2 Position) : base(Position, Size, TextureManager.UnitTextures[Name], Name, MaxHealth, AttackDamage, AttackSpeed, Speed)
        {
        }
    }
}