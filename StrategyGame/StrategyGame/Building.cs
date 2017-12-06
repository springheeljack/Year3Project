﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StrategyGame
{
    public class BuildingBase : EntityBase
    {
        public static Dictionary<string, BuildingBase> Dictionary = new Dictionary<string, BuildingBase>();
        public int MaxHealth { get; }
        public bool Depositable { get; }
        public BuildingBase(Type BuildingType, string Name, Point Size, int MaxHealth,bool Depositable,bool Selectable) : base(BuildingType, Name, Size,Selectable)
        {
            this.MaxHealth = MaxHealth;
            this.Depositable = Depositable;
        }
        public static void Initialize()
        {
            Dictionary.Add("Town Center", new BuildingBase(typeof(BuildingTownCenter), "Town Center", new Point(128), 1000,true,true));
            Dictionary.Add("Stockpile", new BuildingBase(typeof(BuildingStockpile), "Stockpile", new Point(32), 100,true,true));
        }
    }

    public abstract class Building : Entity, IHealth
    {
        public int Health { get; set; }
        public int MaxHealth { get { return (Base as BuildingBase).MaxHealth; } }
        public IAttacker LastAttacker { get; set; }

        public Building(BuildingBase Base, Vector2 Position) : base(Base,Position)
        {
            Health = MaxHealth;
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

    public class BuildingStockpile : Building
    {
        public static List< Recipe> Recipes { get; set; } = new List<Recipe>();
        public BuildingStockpile(BuildingBase Base, Vector2 Position) : base(Base, Position)
        {
        }


        public void Deposit(IGatherer Gatherer)
        {
            BuildingExtension.Deposit(this, Gatherer);
        }
    }

    public class BuildingTownCenter : Building, IHasRecipe
    {
        public static List<Recipe> recipes = new List<Recipe>();
        public List<Recipe> Recipes { get { return recipes; } }

        public BuildingTownCenter(BuildingBase Base, Vector2 Position) : base(Base, Position)
        {

        }

        public override void Update(GameTime gameTime)
        {
        }
        public void Deposit(IGatherer Gatherer)
        {
            BuildingExtension.Deposit(this, Gatherer);
        }
    }

    public static class BuildingExtension
    {
        public static void Deposit(this Building building, IGatherer Gatherer)
        {
            Play.Resources += Gatherer.CarriedResources;
            Gatherer.CarriedResources = 0;
        }
    }
}