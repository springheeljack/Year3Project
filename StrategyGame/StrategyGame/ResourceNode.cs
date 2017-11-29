using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class ResourceNodeBase
    {
        public static Dictionary<string, ResourceNodeBase> BaseDict = new Dictionary<string, ResourceNodeBase>();
        public string Name { get; }
        public Point Size { get; }
        public Texture2D Texture { get; }
        public int ResourceMin { get; }
        public int ResourceMax { get; }
        public List<GathererUnitBase> AcceptedGatherers { get; } = new List<GathererUnitBase>();

        public ResourceNodeBase(string Name, Point Size, int ResourceMin, int ResourceMax, List<GathererUnitBase> AcceptedGatherers)
        {
            this.Name = Name;
            this.Size = Size;
            this.ResourceMin = ResourceMin;
            this.ResourceMax = ResourceMax;
            this.AcceptedGatherers = AcceptedGatherers;
            Texture = TextureManager.ResourceNodeTextures[Name];
        }

        public static void Initialize()
        {
            BaseDict.Add("Iron Rock", new ResourceNodeBase("Iron Rock", new Point(16), 50, 250, new List<GathererUnitBase>() { GathererUnitBase.BaseDict["Miner"] }));
            BaseDict.Add("Tree", new ResourceNodeBase("Tree", new Point(16), 50, 250, new List<GathererUnitBase>() { GathererUnitBase.BaseDict["Miner"] }));
        }
    }

    //public class IronRockBase : ResourceNodeBase
    //{
    //    new public static List<GathererUnitBase> AcceptedGatherers { get; } = new List<GathererUnitBase>();
    //    public string 
    //    public static void Initialize()
    //    {
    //        AcceptedGatherers.Add(GathererUnitBase.BaseDict["Miner"]);
    //    }
    //}

    //public class TreeBase : ResourceNodeBase
    //{
    //    new public static List<GathererUnitBase> AcceptedGatherers { get; } = new List<GathererUnitBase>();
    //    public static void Initialize()
    //    {
    //        AcceptedGatherers.Add(GathererUnitBase.BaseDict["Woodcutter"]);
    //    }
    //}

    public interface IResourceNode : ISelectable
    {
        int Resources { get; set; }
        bool Gatherable { get; set; }
        ResourceNodeBase Base { get; }
    }

    public class ResourceNode : IResourceNode
    {
        public ResourceNodeBase Base { get; }
        public int Resources { get; set; }
        public bool Gatherable { get; set; } = true;
        public string Name { get { return Base.Name; } }
        public Rectangle Rectangle { get; }
        public Texture2D Texture { get { return Base.Texture; } }

        public ResourceNode(Point Position, ResourceNodeBase Base)
        {
            this.Base = Base;
            Rectangle = new Rectangle(Position, Base.Size);
            Random random = new Random();
            Resources = random.Next(Base.ResourceMin, Base.ResourceMax);
        }

        public static void Initialize()
        {
            ResourceNodeBase.Initialize();
        }

        public void Gather(IGatherer Gatherer)
        {
            Resources -= 1;
            Gatherer.CarriedResources += 1;
            if (Resources <= 0)
                Gatherable = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}