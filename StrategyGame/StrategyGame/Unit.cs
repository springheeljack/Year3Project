﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StrategyGame
{
    public static class UnitExtension
    {
        public static void DrawGatherReticles(this UnitGatherer Unit, SpriteBatch spriteBatch)
        {
            if (Unit.GatherTarget != null)
            {
                Texture2D reticleTexture = Art.Textures["Reticle"];
                spriteBatch.Draw(reticleTexture, new Rectangle(Unit.GatherTarget.Rectangle.Center + Art.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
            }
            if (Unit.DepositTarget != null)
            {
                Texture2D reticleTexture = Art.Textures["GreenFlag"];
                spriteBatch.Draw(reticleTexture, new Rectangle(Unit.DepositTarget.Rectangle.Center + Art.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
            }
        }
    }

    public abstract class UnitBase : EntityBase
    {
        public int MaxHealth { get; }
        public float Speed { get; }
        public UnitBase(Type UnitType, string Name, Point Size, int MaxHealth, float Speed, bool Selectable) : base(UnitType, Name, Size, Selectable)
        {
            this.MaxHealth = MaxHealth;
            this.Speed = Speed;
        }
        public static void Initialize()
        {
            UnitBaseMelee.Dictionary.Add("Creep", new UnitBaseMelee("Creep", new Point(32), 50, 5, 1.0f, 2.0f, true));
            UnitBaseGatherer.Dictionary.Add("Miner", new UnitBaseGatherer("Miner", new Point(32), 25, 1.0f, 1.0f, 10, true));
            UnitBaseBuilder.Dictionary.Add("Builder", new UnitBaseBuilder("Builder", new Point(32), 25, 1.0f, 1.0f, true));
        }
    }

    public class UnitBaseMelee : UnitBase
    {
        public static Dictionary<string, UnitBaseMelee> Dictionary = new Dictionary<string, UnitBaseMelee>();
        public int AttackDamage { get; }
        public float AttackSpeed { get; }
        public UnitBaseMelee(string Name, Point Size, int MaxHealth, int AttackDamage, float AttackSpeed, float Speed, bool Selectable) : base(typeof(UnitMelee), Name, Size, MaxHealth, Speed, Selectable)
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
        public UnitBaseGatherer(string Name, Point Size, int MaxHealth, float Speed, float GatherSpeed, int MaxCapacity, bool Selectable) : base(typeof(UnitGatherer), Name, Size, MaxHealth, Speed, Selectable)
        {
            this.GatherSpeed = GatherSpeed;
            this.MaxCapacity = MaxCapacity;
        }
    }

    public class UnitBaseBuilder : UnitBase, IHasRecipe
    {
        static List<Recipe> recipes = new List<Recipe>();
        public List<Recipe> Recipes { get { return Recipes; } }
        public static Dictionary<string, UnitBaseBuilder> Dictionary = new Dictionary<string, UnitBaseBuilder>();
        public float BuildSpeed { get; }
        public UnitBaseBuilder(string Name, Point Size, int MaxHealth, float Speed, float BuildSpeed, bool Selectable) : base(typeof(UnitBuilder), Name, Size, MaxHealth, Speed, Selectable)
        {
            this.BuildSpeed = BuildSpeed;
        }
    }

    public abstract class Unit : Entity, IHealth
    {
        public Vector2 Destination { get; set; }
        public bool HasDestination { get; set; } = false;
        public int Health { get; set; }
        public int MaxHealth { get { return Base.MaxHealth; } }
        public IAttacker LastAttacker { get; set; }
        new public UnitBase Base { get { return base.Base as UnitBase; } }

        public Unit(UnitBase Base, Vector2 Position) : base(Base, Position)
        {
            Health = Base.MaxHealth;
        }

        public override void Update(GameTime gameTime)
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

            base.Update(gameTime);
        }
        public void SetDestination(Vector2 Destination)
        {
            HasDestination = true;
            this.Destination = Destination;
        }

        public void DrawDestinationFlag(SpriteBatch spriteBatch)
        {
            Texture2D flagTexture = Art.Textures["Flag"];
            spriteBatch.Draw(flagTexture, new Rectangle(Destination.ToPoint() + Art.ReticleAndFlagOffset, flagTexture.Bounds.Size), Color.White);
        }

        void IHealth.Damage(IAttacker Attacker)
        {
            Health -= Attacker.Base.AttackDamage;
            LastAttacker = Attacker;
        }

        public override bool ToRemove()
        {
            return Health <= 0;
        }
    }

    public class UnitMelee : Unit, IAttacker
    {
        public float AttackTimer { get; set; } = 0.0f;
        public IHealth AttackTarget { get; set; } = null;
        public float AttackSpeed { get; }
        public new UnitBaseMelee Base { get { return base.Base as UnitBaseMelee; } }

        public UnitMelee(UnitBaseMelee Base, Vector2 Position) : base(Base, Position)
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
            Texture2D reticleTexture = Art.Textures["Reticle"];
            spriteBatch.Draw(reticleTexture, new Rectangle(AttackTarget.Rectangle.Center + Art.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
        }
    }

    public class UnitGatherer : Unit, IGatherer
    {
        new public UnitBaseGatherer Base { get { return base.Base as UnitBaseGatherer; } }
        public int CarriedResources { get; set; }
        public float GatherSpeed { get; set; }
        public ResourceNode GatherTarget { get; set; } = null;
        public Building DepositTarget { get; set; } = null;
        public Entity CurrentTarget { get; set; } = null;
        public UnitGatherer(UnitBaseGatherer Base, Vector2 Position) : base(Base, Position) { }
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
                        if (GatherSpeed <= 0.0f)
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
    }

    public class UnitBuilder : Unit, IHasRecipe
    {
        public static List<Recipe> recipes = new List<Recipe>();
        public List<Recipe> Recipes { get { return recipes; } }
        public UnitBuilder(UnitBaseBuilder Base, Vector2 Position) : base(Base, Position) { }
    }
}