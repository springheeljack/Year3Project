using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StrategyGame
{
    public abstract class UnitBase
    {
        public string Name { get; }
        public Point Size { get; }
        public int MaxHealth { get; }
        public float Speed { get; }
        public Texture2D Texture { get; }
        public Point DrawingSize { get; }
        public UnitBase(string Name, Point Size, int MaxHealth, float Speed)
        {
            this.Name = Name;
            this.Size = Size;
            this.MaxHealth = MaxHealth;
            this.Speed = Speed;
            this.Texture = Texture;
            DrawingSize = new Point(Size.X * Game.GameScale, Size.Y * Game.GameScale);
            Texture = TextureManager.UnitTextures[Name];
        }
    }

    public class MeleeUnitBase : UnitBase
    {
        public static Dictionary<string, MeleeUnitBase> BaseDict = new Dictionary<string, MeleeUnitBase>();
        public int AttackDamage { get; }
        public float AttackSpeed { get; }
        public MeleeUnitBase(string Name, Point Size, int MaxHealth, int AttackDamage, float AttackSpeed, float Speed) : base(Name, Size, MaxHealth, Speed)
        {
            this.AttackDamage = AttackDamage;
            this.AttackSpeed = AttackSpeed;
        }
        public static void Initialize()
        {
           BaseDict.Add("Creep", new MeleeUnitBase("Creep", new Point(8), 50, 5, 1.0f, 2.0f));
        }
    }

    public class GathererUnitBase : UnitBase
    {
        public static Dictionary<string, GathererUnitBase> BaseDict = new Dictionary<string, GathererUnitBase>();
        public float GatherSpeed { get; }
        public int MaxCapacity { get; }
        public GathererUnitBase(string Name, Point Size, int MaxHealth, float Speed, float GatherSpeed, int MaxCapacity) : base(Name, Size, MaxHealth, Speed)
        {
            this.GatherSpeed = GatherSpeed;
            this.MaxCapacity = MaxCapacity;
        }
        public static void Initialize()
        {
            BaseDict.Add("Miner", new GathererUnitBase("Miner", new Point(8), 25, 1.0f, 1.0f, 10));
        }
    }

    public abstract class Unit : IHealth
    {
        public Texture2D Texture { get { return UnitBase.Texture; } }
        public string Name { get { return UnitBase.Name; } }
        //Point Size { get; }
        //Point DrawingSize { get; }
        public Vector2 Position { get; set; }
        public Vector2 Destination { get; set; }
        public bool HasDestination { get; set; } = false;
        public Rectangle Rectangle { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get { return UnitBase.MaxHealth; } }
        //public float Speed { get; }
        public IAttacker LastAttacker { get; set; }
        public UnitBase UnitBase { get; }

        public static void Initialize()
        {
            MeleeUnitBase.Initialize();
            GathererUnitBase.Initialize();
        }

        public Unit(Vector2 Position, UnitBase UnitBase)
        {
            this.UnitBase = UnitBase;
            this.Position = Position;
            Health = UnitBase.MaxHealth;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (HasDestination)
            {
                Vector2 pos = Position;
                Vector2 move = Destination - pos;
                float length = move.Length();
                if (length > UnitBase.Speed)
                {
                    move.Normalize();
                    move *= UnitBase.Speed;
                }
                pos += move;
                Position = pos;
                if (length <= 0.1f)
                    HasDestination = false;
            }

            Rectangle = new Rectangle(Position.ToPoint() - UnitBase.Size, UnitBase.DrawingSize);
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
            Health -= Attacker.UnitBase.AttackDamage;
            LastAttacker = Attacker;
        }
    }

    public class UnitMelee : Unit, IAttacker
    {
        public float AttackTimer { get; set; } = 0.0f;
        public IHealth AttackTarget { get; set; } = null;
        new public MeleeUnitBase UnitBase { get; }

        public UnitMelee(Vector2 Position, MeleeUnitBase UnitBase) : base(Position, UnitBase)
        {
            this.UnitBase = UnitBase;
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
                    if (length > UnitBase.Speed)
                    {
                        move.Normalize();
                        move *= UnitBase.Speed;
                    }
                    pos += move;
                    Position = pos;
                    if (length <= 0.1f)
                    {
                        if (AttackTimer <= 0.0f)
                        {
                            AttackTimer += UnitBase.AttackSpeed;
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

    public class UnitMiner : Unit, IGatherer
    {
        new public GathererUnitBase UnitBase { get; }
        public int CarriedResources { get; set; }
        public float GatherTimer { get; set; }
        public ResourceNode GatherTarget { get; set; } = null;
        public IResourceDeposit DepositTarget { get; set; } = null;
        public IRectangleObject CurrentTarget { get; set; } = null;
        public UnitMiner(Vector2 Position, GathererUnitBase UnitBase) : base(Position, UnitBase)
        {
            this.UnitBase = UnitBase;
        }
        public override void Update(GameTime gameTime)
        {
            if (GatherTarget != null && !GatherTarget.Gatherable)
            {
                GatherTarget = null;
                CurrentTarget = null;
            }
            if (CurrentTarget == null)
            {
                if (GatherTarget != null && CarriedResources < UnitBase.MaxCapacity)
                    CurrentTarget = GatherTarget;
                else if (DepositTarget != null && CarriedResources > 0)
                    CurrentTarget = DepositTarget;
            }

            if (CurrentTarget != null)
            {
                Vector2 pos = Position;
                Vector2 move = CurrentTarget.Rectangle.Center.ToVector2() - pos;
                float length = move.Length();
                if (length > UnitBase.Speed)
                {
                    move.Normalize();
                    move *= UnitBase.Speed;
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
                        if (GatherTimer<=0.0f)
                        {
                            GatherTimer += UnitBase.GatherSpeed;
                            if (!GatherTarget.Gatherable)
                            {
                                GatherTarget = null;
                                CurrentTarget = DepositTarget;
                            }
                            GatherTarget.Gather(this);
                            if (CarriedResources == UnitBase.MaxCapacity)
                                CurrentTarget = DepositTarget;
                        }
                    }
                }
            }
            

            if (GatherTimer > 0.0f)
                GatherTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }
        public void DrawGatherReticle(SpriteBatch spriteBatch)
        {
            Texture2D reticleTexture = TextureManager.UITextures["Reticle"];
            spriteBatch.Draw(reticleTexture, new Rectangle(GatherTarget.Rectangle.Center + TextureManager.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
        }
        public void DrawDepositReticle(SpriteBatch spriteBatch)
        {
            Texture2D reticleTexture = TextureManager.UITextures["GreenFlag"];
            spriteBatch.Draw(reticleTexture, new Rectangle(DepositTarget.Rectangle.Center + TextureManager.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
        }
    }
}