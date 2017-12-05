using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StrategyGame
{
    public abstract class UnitBase
    {
        public string Name { get; }
        public int MaxHealth { get; }
        public float Speed { get; }
        public Texture2D Texture { get; }
        public Point Size { get; }
        public Type UnitType { get; }
        public UnitBase(Type UnitType, string Name, Point Size, int MaxHealth, float Speed)
        {
            this.UnitType = UnitType;
            this.Name = Name;
            this.Size = Size;
            this.MaxHealth = MaxHealth;
            this.Speed = Speed;
            Texture = Art.UnitTextures[Name];
        }
        public static void Initialize()
        {
            UnitBaseMelee.Dictionary.Add("Creep", new UnitBaseMelee("Creep", new Point(32), 50, 5, 1.0f, 2.0f));
            UnitBaseGatherer.Dictionary.Add("Miner", new UnitBaseGatherer("Miner", new Point(32), 25, 1.0f, 1.0f, 10));
            UnitBaseBuilder.Dictionary.Add("Builder", new UnitBaseBuilder("Builder", new Point(32), 25, 1.0f, 1.0f));
        }
    }

    public class UnitBaseMelee : UnitBase
    {
        public static Dictionary<string, UnitBaseMelee> Dictionary = new Dictionary<string, UnitBaseMelee>();
        public int AttackDamage { get; }
        public float AttackSpeed { get; }
        public UnitBaseMelee(string Name, Point Size, int MaxHealth, int AttackDamage, float AttackSpeed, float Speed) : base(typeof(UnitMelee),Name, Size, MaxHealth, Speed)
        {
            this.AttackDamage = AttackDamage;
            this.AttackSpeed = AttackSpeed;
        }
    }

    public class UnitBaseGatherer : UnitBase
    {
        public static Dictionary<string, UnitBaseGatherer> Dictionary = new Dictionary<string, UnitBaseGatherer>();
        public float GatherSpeed { get; }
        public int MaxCapacity { get; }
        public UnitBaseGatherer(string Name, Point Size, int MaxHealth, float Speed, float GatherSpeed, int MaxCapacity) : base(typeof(UnitGatherer),Name, Size, MaxHealth, Speed)
        {
            this.GatherSpeed = GatherSpeed;
            this.MaxCapacity = MaxCapacity;
        }
    }

    public class UnitBaseBuilder : UnitBase, IHasBuildRecipe
    {
        static List<BuildingRecipe> Recipes = new List<BuildingRecipe>();
        public List<BuildingRecipe> GetBuildingRecipes() { return Recipes; }
        public static Dictionary<string, UnitBaseBuilder> Dictionary = new Dictionary<string, UnitBaseBuilder>();
        public float BuildSpeed { get; }
        public UnitBaseBuilder(string Name, Point Size, int MaxHealth, float Speed, float BuildSpeed) : base(typeof(UnitBuilder), Name, Size, MaxHealth, Speed)
        {
            this.BuildSpeed = BuildSpeed;
        }
    }

    public abstract class Unit : IHealth
    {
        public Texture2D Texture { get { return Base.Texture; } }
        public string Name { get { return Base.Name; } }
        //Point Size { get; }
        //Point DrawingSize { get; }
        public Vector2 Position { get; set; }
        public Vector2 Destination { get; set; }
        public bool HasDestination { get; set; } = false;
        public Rectangle Rectangle { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get { return Base.MaxHealth; } }
        //public float Speed { get; }
        public IAttacker LastAttacker { get; set; }
        public UnitBase Base { get; }

        public Unit(Vector2 Position, UnitBase Base)
        {
            this.Base = Base;
            this.Position = Position;
            Health = Base.MaxHealth;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (HasDestination)
            {
                Vector2 pos = Position;
                Vector2 move = Destination - pos;
                float length = move.Length();
                if (length > Base.Speed)
                {
                    move.Normalize();
                    move *= Base.Speed;
                }
                pos += move;
                Position = pos;
                if (length <= 0.1f)
                    HasDestination = false;
            }

            Rectangle = new Rectangle(Position.ToPoint() - Base.Size.Half(), Base.Size);
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
            Texture2D flagTexture = Art.UITextures["Flag"];
            spriteBatch.Draw(flagTexture, new Rectangle(Destination.ToPoint() + Art.ReticleAndFlagOffset, flagTexture.Bounds.Size), Color.White);
        }

        void IHealth.Damage(IAttacker Attacker)
        {
            Health -= Attacker.Base.AttackDamage;
            LastAttacker = Attacker;
        }
    }

    public class UnitMelee : Unit, IAttacker
    {
        public float AttackTimer { get; set; } = 0.0f;
        public IHealth AttackTarget { get; set; } = null;
        public float AttackSpeed { get; }
        public new UnitBaseMelee Base { get { return base.Base as UnitBaseMelee; } }

        public UnitMelee(Vector2 Position, UnitBaseMelee UnitBase) : base(Position, UnitBase)
        {

        }

        //////////////TODO moving with collision
        public override void Update(GameTime gameTime)
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
                    if (length > Base.Speed)
                    {
                        move.Normalize();
                        move *= Base.Speed;
                    }
                    pos += move;
                    Position = pos;
                    if (length <= 0.1f)
                    {
                        if (AttackTimer <= 0.0f)
                        {
                            AttackTimer += Base.AttackSpeed;
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
            Texture2D reticleTexture = Art.UITextures["Reticle"];
            spriteBatch.Draw(reticleTexture, new Rectangle(AttackTarget.Rectangle.Center + Art.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
        }
    }

    public class UnitGatherer : Unit, IGatherer
    {
        new public UnitBaseGatherer Base { get { return base.Base as UnitBaseGatherer; } }
        public int CarriedResources { get; set; }
        public float GatherSpeed { get; set; }
        public ResourceNode GatherTarget { get; set; } = null;
        public IResourceDeposit DepositTarget { get; set; } = null;
        public IRectangleObject CurrentTarget { get; set; } = null;
        public UnitGatherer(Vector2 Position, UnitBaseGatherer UnitBase) : base(Position, UnitBase) { }
        public override void Update(GameTime gameTime)
        {
            if (GatherTarget != null && !GatherTarget.Gatherable)
            {
                GatherTarget = null;
                CurrentTarget = null;
            }
            if (CurrentTarget == null)
            {
                if (GatherTarget != null && CarriedResources < Base.MaxCapacity)
                    CurrentTarget = GatherTarget;
                else if (DepositTarget != null && CarriedResources > 0)
                    CurrentTarget = DepositTarget;
            }

            if (CurrentTarget != null)
            {
                Vector2 pos = Position;
                Vector2 move = CurrentTarget.Rectangle.Center.ToVector2() - pos;
                float length = move.Length();
                if (length > Base.Speed)
                {
                    move.Normalize();
                    move *= Base.Speed;
                }
                pos += move;
                Position = pos;
                if (length <= 0.1f)
                {
                    if (CurrentTarget == DepositTarget)
                    {
                        DepositTarget.Deposit(this);
                        if (GatherTarget != null)
                            CurrentTarget = GatherTarget;
                    }
                    else
                    {
                        if (GatherSpeed<=0.0f)
                        {
                            GatherSpeed += Base.GatherSpeed;
                            if (!GatherTarget.Gatherable)
                            {
                                GatherTarget = null;
                                CurrentTarget = DepositTarget;
                            }
                            GatherTarget.Gather(this);
                            if (CarriedResources == Base.MaxCapacity)
                                CurrentTarget = DepositTarget;
                        }
                    }
                }
            }
            

            if (GatherSpeed > 0.0f)
                GatherSpeed -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }
        public void DrawGatherReticle(SpriteBatch spriteBatch)
        {
            Texture2D reticleTexture = Art.UITextures["Reticle"];
            spriteBatch.Draw(reticleTexture, new Rectangle(GatherTarget.Rectangle.Center + Art.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
        }
        public void DrawDepositReticle(SpriteBatch spriteBatch)
        {
            Texture2D reticleTexture = Art.UITextures["GreenFlag"];
            spriteBatch.Draw(reticleTexture, new Rectangle(DepositTarget.Rectangle.Center + Art.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
        }
    }

    public class UnitBuilder
    {

    }
}