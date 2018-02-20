using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using StrategyGame.GOAP.Actions;

namespace StrategyGame
{
    public class UnitBase
    {
        public string Name { get; private set; }
        public Point Size { get; private set; }
        public Texture2D Texture { get; private set; }
        public int InventoryCapacity { get; private set; }
        public float MoveSpeed { get; private set; }
        public List<GOAPAction> Actions { get; private set; }

        public static Dictionary<string, UnitBase> Bases = new Dictionary<string, UnitBase>();

        public static void Initialize()
        {
            //Woodcutter
            List<GOAPAction> Actions = new List<GOAPAction>
            {
                new StoreItem(ItemType.Log),
                new GatherResource(Gather.Dictionary["Sticks"]),
                new GatherResource(Gather.Dictionary["Tree"]),
                new PickUpItem(ItemType.IronAxe)
            };
            Bases.Add("Woodcutter", new UnitBase("Woodcutter", new Point(32), Art.Textures["Woodcutter"], 10, 100, Actions));

            //Miner
            Actions = new List<GOAPAction>
            {
                new GatherResource(Gather.Dictionary["IronRock"]),
                new GatherResource(Gather.Dictionary["IronRockNoPickaxe"]),
                new GatherResource(Gather.Dictionary["CoalRock"]),
                new GatherResource(Gather.Dictionary["CoalRockNoPickaxe"]),
                new StoreItem(ItemType.IronOre),
                new StoreItem(ItemType.Coal),
                new PickUpItem(ItemType.IronPickaxe)
            };
            Bases.Add("Miner", new UnitBase("Miner", new Point(32), Art.Textures["Miner"], 10, 100, Actions));

            //Blacksmith
            Actions = new List<GOAPAction>
            {
                new PickUpItem(ItemType.IronOre),
                new PickUpItem(ItemType.Log),
                new PickUpItem(ItemType.Coal),
                new CreateItem(Recipe.Dictionary["IronAxe"]),
                new CreateItem(Recipe.Dictionary["IronPickaxe"]),
                new CreateItem(Recipe.Dictionary["IronIngot"]),
                new CreateItem(Recipe.Dictionary["Scythe"]),
                new StoreItem(ItemType.IronAxe),
                new StoreItem(ItemType.IronPickaxe),
                new StoreItem(ItemType.Scythe),
            };
            Bases.Add("Blacksmith", new UnitBase("Blacksmith", new Point(32), Art.Textures["Blacksmith"], 10, 50, Actions));

            //Farmer
            Actions = new List<GOAPAction>
            {
                new GatherResource(Gather.Dictionary["FarmNoScythe"]),
                new GatherResource(Gather.Dictionary["Farm"]),
                new StoreItem(ItemType.Wheat),
                new PickUpItem(ItemType.Scythe)
            };
            Bases.Add("Farmer", new UnitBase("Farmer", new Point(32), Art.Textures["Farmer"], 10, 75, Actions));
        }

        public UnitBase(string name, Point size, Texture2D texture, int inventoryCapacity, float moveSpeed, List<GOAPAction> actions)
        {
            Name = name;
            Size = size;
            Texture = texture;
            InventoryCapacity = inventoryCapacity;
            MoveSpeed = moveSpeed;
            Actions = actions;
        }
    }

    public abstract class Unit : GOAPAgent, IGOAP
    {
        private bool Started = false;
        public Inventory Inventory { get; private set; }
        public float MoveSpeed { get; private set; }
        public List<GOAPAction> Actions { get; private set; }

        public Unit(UnitBase unitBase, Vector2 position) : base(position, unitBase.Name, unitBase.Size, unitBase.Texture)
        {
            Inventory = new Inventory(unitBase.InventoryCapacity);
            Actions = unitBase.Actions;
            MoveSpeed = unitBase.MoveSpeed;
        }

        public override void Update()
        {
            if (!Started)
            {
                Start();
                Started = true;
            }
            UpdateAgent();
            base.Update();
        }

        public Dictionary<Tuple<string, object>, object> GetWorldState()
        {
            Dictionary<Tuple<string, object>, object> WorldData = new Dictionary<Tuple<string, object>, object>();

            foreach (ItemType i in Enum.GetValues(typeof(ItemType)))
            {
                WorldData.Add(new Tuple<string, object>("HasItem", i), Inventory.Items.ContainsKey(i));
            }

            return WorldData;
        }

        public void PlanFailed(Dictionary<Tuple<string, object>, object> failedGoal) { }

        public void PlanFound(Dictionary<Tuple<string, object>, object> goal, Queue<GOAPAction> actions) { }

        public void ActionsFinished()
        {
            AddThought("Plan completed!", Color.Green);
            Start();
        }

        public void PlanAborted(GOAPAction aborter) { }

        public bool MoveAgent(GOAPAction nextAction)
        {
            Vector2 delta = nextAction.Target.Position - Position;
            if (delta.Length() == 0.0f)
            {
                nextAction.InRange = true;
                return true;
            }
            float step = MoveSpeed * (float)Global.gameTime.ElapsedGameTime.TotalSeconds;
            delta.Normalize();
            delta *= step;
            Position += delta;

            if ((nextAction.Target.Position - Position).Length() < 16)
            {
                nextAction.InRange = true;
                return true;
            }
            else
                return false;
        }

        public abstract Dictionary<Tuple<string, object>, object> CreateGoalState();
    }

    public class Miner : Unit
    {
        public Miner(Vector2 position) : base(UnitBase.Bases["Miner"], position) { }

        public override Dictionary<Tuple<string, object>, object> CreateGoalState()
        {
            Dictionary<Tuple<string, object>, object> goal = new Dictionary<Tuple<string, object>, object>();
            int ironOre = 0;
            int coal = 0;
            List<Building> stockpiles = EntityManager.GetBuildings().Where(x => x.BuildingType == BuildingType.Stockpile).ToList();
            foreach (Building b in stockpiles)
            {
                Dictionary<ItemType, int> items = b.Inventory.Items;
                if (items.ContainsKey(ItemType.Coal))
                    coal += items[ItemType.Coal];
                if (items.ContainsKey(ItemType.IronOre))
                    ironOre += items[ItemType.IronOre];
            }
            if (coal < ironOre)
                goal.Add(new Tuple<string, object>("StoreItem", ItemType.Coal), true);
            else
                goal.Add(new Tuple<string, object>("StoreItem", ItemType.IronOre), true);
            return goal;
        }
    }

    public class Woodcutter : Unit
    {
        public Woodcutter(Vector2 position) : base(UnitBase.Bases["Woodcutter"], position) { }

        public override Dictionary<Tuple<string, object>, object> CreateGoalState()
        {
            Dictionary<Tuple<string, object>, object> goal = new Dictionary<Tuple<string, object>, object>
            {
                { new Tuple<string, object>("StoreItem", ItemType.Log), true }
            };
            return goal;
        }
    }

    public class Blacksmith : Unit
    {
        public Blacksmith(Vector2 position) : base(UnitBase.Bases["Blacksmith"], position) { }

        public override Dictionary<Tuple<string, object>, object> CreateGoalState()
        {
            Dictionary<Tuple<string, object>, object> goal = new Dictionary<Tuple<string, object>, object>();

            ItemType[] items = { ItemType.Scythe, ItemType.IronAxe, ItemType.IronPickaxe };
            List<Building> stockpiles = EntityManager.GetBuildings().Where(x => x.BuildingType == BuildingType.Stockpile).ToList();
            int lowest = -1;
            ItemType lowestItem = 0;
            foreach (ItemType i in items)
            {
                int item = 0;
                foreach (Building b in stockpiles)
                {
                    if (b.Inventory.Items.ContainsKey(i))
                        item += b.Inventory.Items[i];
                }
                if (lowest == -1)
                {
                    lowest = item;
                    lowestItem = i;
                }
                else if (item < lowest)
                {
                    lowest = item;
                    lowestItem = i;
                }
            }
            goal.Add(new Tuple<string, object>("StoreItem", ItemType.Scythe), true);
            //int axes = 0;
            //int pickaxes = 0;
            //List<Building> stockpiles = EntityManager.GetBuildings().Where(x => x.BuildingType == BuildingType.Stockpile).ToList();
            //foreach (Building b in stockpiles)
            //{
            //    Dictionary<ItemType, int> items = b.Inventory.Items;
            //    if (items.ContainsKey(ItemType.IronAxe))
            //        axes += items[ItemType.IronAxe];
            //    if (items.ContainsKey(ItemType.IronPickaxe))
            //        pickaxes += items[ItemType.IronPickaxe];
            //}
            //if (pickaxes < axes)
            //    goal.Add(new Tuple<string, object>("StoreItem", ItemType.IronPickaxe), true);
            //else
            //    goal.Add(new Tuple<string, object>("StoreItem", ItemType.IronAxe), true);
            return goal;
        }
    }

    public class Farmer : Unit
    {
        public Farmer(Vector2 position) : base(UnitBase.Bases["Farmer"], position) { }

        public override Dictionary<Tuple<string, object>, object> CreateGoalState()
        {
            Dictionary<Tuple<string, object>, object> goal = new Dictionary<Tuple<string, object>, object>();
            goal.Add(new Tuple<string, object>("StoreItem", ItemType.Wheat), true);
            return goal;
        }
    }

    public class Miller : Unit
    {
        public Miller(Vector2 position) : base(UnitBase.Bases["Miller"], position) { }

        public override Dictionary<Tuple<string, object>, object> CreateGoalState()
        {
            Dictionary<Tuple<string, object>, object> goal = new Dictionary<Tuple<string, object>, object>();
            goal.Add(new Tuple<string, object>("StoreItem", ItemType.Flour), true);
            return goal;
        }
    }

    public class Baker : Unit
    {
        public Baker(Vector2 position) : base(UnitBase.Bases["Baker"], position) { }

        public override Dictionary<Tuple<string, object>, object> CreateGoalState()
        {
            Dictionary<Tuple<string, object>, object> goal = new Dictionary<Tuple<string, object>, object>();
            goal.Add(new Tuple<string, object>("StoreItem", ItemType.Bread), true);
            return goal;
        }
    }
}

//public static class UnitExtension
//{
//    public static void DrawGatherReticles(this UnitGatherer Unit, SpriteBatch spriteBatch)
//    {
//        if (Unit.GatherTarget != null)
//        {
//            Texture2D reticleTexture = Art.Textures["Reticle"];
//            spriteBatch.Draw(reticleTexture, new Rectangle(Unit.GatherTarget.Rectangle.Center + Art.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
//        }
//        if (Unit.DepositTarget != null)
//        {
//            Texture2D reticleTexture = Art.Textures["GreenFlag"];
//            spriteBatch.Draw(reticleTexture, new Rectangle(Unit.DepositTarget.Rectangle.Center + Art.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
//        }
//    }
//}

//public abstract class UnitBase : EntityBase
//{
//    new static readonly float LayerDepth = 0.6f;
//    public int MaxHealth { get; }
//    public float Speed { get; }
//    public UnitBase(Type UnitType, string Name, Point Size, int MaxHealth, float Speed, bool Selectable) : base(UnitType, Name, Size, Selectable, LayerDepth)
//    {
//        this.MaxHealth = MaxHealth;
//        this.Speed = Speed;
//    }
//    public static void Initialize()
//    {
//        UnitBaseMelee.Dictionary.Add("Creep", new UnitBaseMelee("Creep", new Point(32), 50, 5, 1.0f, 2.0f, true));
//        UnitBaseGatherer.Dictionary.Add("Miner", new UnitBaseGatherer("Miner", new Point(32), 25, 1.0f, 1.0f, 10, true));
//        UnitBaseBuilder.Dictionary.Add("Builder", new UnitBaseBuilder("Builder", new Point(32), 25, 1.0f, 1.0f, true));
//    }
//}

//public class UnitBaseMelee : UnitBase
//{
//    public static Dictionary<string, UnitBaseMelee> Dictionary = new Dictionary<string, UnitBaseMelee>();
//    public int AttackDamage { get; }
//    public float AttackSpeed { get; }
//    public UnitBaseMelee(string Name, Point Size, int MaxHealth, int AttackDamage, float AttackSpeed, float Speed, bool Selectable) : base(typeof(UnitMelee), Name, Size, MaxHealth, Speed, Selectable)
//    {
//        this.AttackDamage = AttackDamage;
//        this.AttackSpeed = AttackSpeed;
//    }
//}

//public class UnitBaseGatherer : UnitBase
//{
//    public static Dictionary<string, UnitBaseGatherer> Dictionary = new Dictionary<string, UnitBaseGatherer>();
//    public float GatherSpeed { get; }
//    public int MaxCapacity { get; }
//    public UnitBaseGatherer(string Name, Point Size, int MaxHealth, float Speed, float GatherSpeed, int MaxCapacity, bool Selectable) : base(typeof(UnitGatherer), Name, Size, MaxHealth, Speed, Selectable)
//    {
//        this.GatherSpeed = GatherSpeed;
//        this.MaxCapacity = MaxCapacity;
//    }
//}

//public class UnitBaseBuilder : UnitBase, IHasRecipe
//{
//    static List<Recipe> recipes = new List<Recipe>();
//    public List<Recipe> Recipes { get { return Recipes; } }
//    public static Dictionary<string, UnitBaseBuilder> Dictionary = new Dictionary<string, UnitBaseBuilder>();
//    public float BuildSpeed { get; }
//    public UnitBaseBuilder(string Name, Point Size, int MaxHealth, float Speed, float BuildSpeed, bool Selectable) : base(typeof(UnitBuilder), Name, Size, MaxHealth, Speed, Selectable)
//    {
//        this.BuildSpeed = BuildSpeed;
//    }
//}

//public abstract class Unit : Entity, IHealth
//{
//    public Vector2 Destination { get; set; }
//    public bool HasDestination { get; set; } = false;
//    public int Health { get; set; }
//    public int MaxHealth { get { return Base.MaxHealth; } }
//    public IAttacker LastAttacker { get; set; }
//    new public UnitBase Base { get { return base.Base as UnitBase; } }

//    public Unit(UnitBase Base, Vector2 Position) : base(Base, Position)
//    {
//        Health = Base.MaxHealth;
//    }

//    public override void Update(GameTime gameTime)
//    {
//        if (HasDestination)
//        {
//            Vector2 pos = Position;
//            Vector2 move = Destination - pos;
//            float length = move.Length();
//            if (length > Base.Speed)
//            {
//                move.Normalize();
//                move *= Base.Speed;
//            }
//            pos += move;
//            Position = pos;
//            if (length <= 0.1f)
//                HasDestination = false;
//        }

//        base.Update(gameTime);
//    }
//    public void SetDestination(Vector2 Destination)
//    {
//        HasDestination = true;
//        this.Destination = Destination;
//    }

//    public void DrawDestinationFlag(SpriteBatch spriteBatch)
//    {
//        Texture2D flagTexture = Art.Textures["Flag"];
//        spriteBatch.Draw(flagTexture, new Rectangle(Destination.ToPoint() + Art.ReticleAndFlagOffset, flagTexture.Bounds.Size), Color.White);
//    }

//    void IHealth.Damage(IAttacker Attacker)
//    {
//        Health -= Attacker.Base.AttackDamage;
//        LastAttacker = Attacker;
//    }

//    //public override bool ToRemove()
//    //{
//    //    return Health <= 0;
//    //}
//}

//    public class UnitMelee : Unit, IAttacker
//    {
//        public float AttackTimer { get; set; } = 0.0f;
//        public IHealth AttackTarget { get; set; } = null;
//        public float AttackSpeed { get; }
//        public new UnitBaseMelee Base { get { return base.Base as UnitBaseMelee; } }

//        public UnitMelee(UnitBaseMelee Base, Vector2 Position) : base(Base, Position)
//        {
//        }

//        //////////////TODO moving with collision
//        public override void Update(GameTime gameTime)
//        {
//            if (AttackTarget != null)
//            {
//                if (AttackTarget.Health <= 0)
//                    AttackTarget = null;
//                else
//                {
//                    Vector2 pos = Position;
//                    Vector2 move = AttackTarget.Rectangle.Center.ToVector2() - pos;
//                    float length = move.Length();
//                    if (length > Base.Speed)
//                    {
//                        move.Normalize();
//                        move *= Base.Speed;
//                    }
//                    pos += move;
//                    Position = pos;
//                    if (length <= 0.1f)
//                    {
//                        if (AttackTimer <= 0.0f)
//                        {
//                            AttackTimer += Base.AttackSpeed;
//                            AttackTarget.Damage(this);
//                        }
//                    }
//                }
//            }

//            if (AttackTimer > 0.0f)
//                AttackTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

//            base.Update(gameTime);
//        }

//        public void DrawAttackReticle(SpriteBatch spriteBatch)
//        {
//            Texture2D reticleTexture = Art.Textures["Reticle"];
//            spriteBatch.Draw(reticleTexture, new Rectangle(AttackTarget.Rectangle.Center + Art.ReticleAndFlagOffset, reticleTexture.Bounds.Size), Color.White);
//        }
//    }

//    public class UnitGatherer : Unit, IGatherer
//    {
//        new public UnitBaseGatherer Base { get { return base.Base as UnitBaseGatherer; } }
//        public int CarriedResources { get; set; }
//        public float GatherSpeed { get; set; }
//        public ResourceNode GatherTarget { get; set; } = null;
//        public Building DepositTarget { get; set; } = null;
//        public Entity CurrentTarget { get; set; } = null;
//        public UnitGatherer(UnitBaseGatherer Base, Vector2 Position) : base(Base, Position) { }
//        public override void Update(GameTime gameTime)
//        {
//            if (Game.SelectedEntity == this)
//            {
//                if (Input.IsRightClicked(Input.MouseState))
//                {
//                    //HasDestination = true;
//                    //Destination = Input.MouseState.Position.ToVector2();
//                    if (CarriedResources < Base.MaxCapacity)
//                    {
//                        foreach (ResourceNode r in EntityManager.Entities.OfType<ResourceNode>())
//                            if (MouseExtension.Rectangle.Intersects(r.Rectangle))
//                            {
//                                GatherTarget = r;
//                                HasDestination = false;
//                            }
//                    }
//                    foreach (Building b in EntityManager.Entities.OfType<Building>().Where(n => (n.Base as BuildingBase).Depositable))
//                        if (MouseExtension.Rectangle.Intersects(b.Rectangle))
//                        {
//                            DepositTarget = b;
//                            HasDestination = false;
//                        }
//                    if (HasDestination == false && DepositTarget == null && GatherTarget == null && CurrentTarget == null)
//                    {
//                        HasDestination = true;
//                        Destination = Input.MouseState.Position.ToVector2();
//                    }
//                    if (HasDestination == true)
//                    {
//                        DepositTarget = null;
//                        GatherTarget = null;
//                        CurrentTarget = null;
//                    }
//                }
//            }

//            if (GatherTarget != null && !GatherTarget.Gatherable)
//            {
//                GatherTarget = null;
//                CurrentTarget = null;
//            }
//            if (CurrentTarget == null)
//            {
//                if (GatherTarget != null && CarriedResources < Base.MaxCapacity)
//                    CurrentTarget = GatherTarget;
//                else if (DepositTarget != null && CarriedResources > 0)
//                    CurrentTarget = DepositTarget;
//            }

//            if (CurrentTarget != null)
//            {
//                Vector2 pos = Position;
//                Vector2 move = CurrentTarget.Rectangle.Center.ToVector2() - pos;
//                float length = move.Length();
//                if (length > Base.Speed)
//                {
//                    move.Normalize();
//                    move *= Base.Speed;
//                }
//                pos += move;
//                Position = pos;
//                if (length <= 0.1f)
//                {
//                    if (CurrentTarget == DepositTarget)
//                    {
//                        DepositTarget.Deposit(this);
//                        if (GatherTarget != null)
//                            CurrentTarget = GatherTarget;
//                    }
//                    else
//                    {
//                        if (GatherSpeed <= 0.0f)
//                        {
//                            GatherSpeed += Base.GatherSpeed;
//                            if (!GatherTarget.Gatherable)
//                            {
//                                GatherTarget = null;
//                                CurrentTarget = DepositTarget;
//                            }
//                            GatherTarget.Gather(this);
//                            if (CarriedResources == Base.MaxCapacity)
//                                CurrentTarget = DepositTarget;
//                        }
//                    }
//                }
//            }


//            if (GatherSpeed > 0.0f)
//                GatherSpeed -= (float)gameTime.ElapsedGameTime.TotalSeconds;

//            base.Update(gameTime);
//        }
//    }

//    public class UnitBuilder : Unit, IHasRecipe
//    {
//        public static List<Recipe> recipes = new List<Recipe>();
//        public List<Recipe> Recipes { get { return recipes; } }
//        public UnitBuilder(UnitBaseBuilder Base, Vector2 Position) : base(Base, Position) { }
//    }