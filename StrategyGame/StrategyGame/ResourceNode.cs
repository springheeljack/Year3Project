using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class ResourceNodeBase : EntityBase
    {
        new static readonly float LayerDepth = 0.5f;
        public static Dictionary<string, ResourceNodeBase> Dictionary = new Dictionary<string, ResourceNodeBase>();
        public int ResourceMin { get; }
        public int ResourceMax { get; }
        public List<UnitBaseGatherer> AcceptedGatherers { get; } = new List<UnitBaseGatherer>();

        public ResourceNodeBase(Type ResourceType, string Name, Point Size, int ResourceMin, int ResourceMax, List<UnitBaseGatherer> AcceptedGatherers,bool Selectable) : base(ResourceType,Name,Size,Selectable,LayerDepth)
        {
            this.ResourceMin = ResourceMin;
            this.ResourceMax = ResourceMax;
            this.AcceptedGatherers = AcceptedGatherers;
        }

        public static void Initialize()
        {
            Dictionary.Add("Iron Rock", new ResourceNodeBase(typeof(ResourceNode), "Iron Rock", new Point(32), 50, 250, new List<UnitBaseGatherer>() { UnitBaseGatherer.Dictionary["Miner"] },true));
            Dictionary.Add("Tree", new ResourceNodeBase(typeof(ResourceNode), "Tree", new Point(32), 50, 250, new List<UnitBaseGatherer>() { UnitBaseGatherer.Dictionary["Miner"] },true));
        }
    }
    public interface IResourceNode
    {
        int Resources { get; set; }
        bool Gatherable { get; set; }
        ResourceNodeBase Base { get; }
    }

    public class ResourceNode : Entity, IResourceNode
    {
        new public ResourceNodeBase Base { get { return base.Base as ResourceNodeBase; } }
        public int Resources { get; set; }
        public bool Gatherable { get; set; } = true;

        public ResourceNode(ResourceNodeBase Base, Vector2 Position) : base(Base, Position)
        {
            Random random = new Random();
            Resources = random.Next(Base.ResourceMin, Base.ResourceMax);
        }

        public void Gather(IGatherer Gatherer)
        {
            Resources -= 1;
            Gatherer.CarriedResources += 1;
            if (Resources <= 0)
                Gatherable = false;
        }
    }
}