using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StrategyGame
{
    public enum ResourceNodeType
    {
        Tree,
        IronRock,
        Sticks,
        CoalRock,
        Farm
    }

    public class ResourceNodeBase
    {
        public static void Initialize()
        {
            Bases.Add("Tree", new ResourceNodeBase("Tree", new Point(32), Art.Textures["Tree"], ResourceNodeType.Tree));
            Bases.Add("Iron Rock", new ResourceNodeBase("Iron Rock", new Point(32), Art.Textures["Iron Rock"], ResourceNodeType.IronRock));
            Bases.Add("Sticks", new ResourceNodeBase("Sticks", new Point(32), Art.Textures["Sticks"], ResourceNodeType.Sticks));
            Bases.Add("Coal Rock", new ResourceNodeBase("Coal Rock", new Point(32), Art.Textures["Coal Rock"], ResourceNodeType.CoalRock));
            Bases.Add("Farm", new ResourceNodeBase("Farm", new Point(64), Art.Textures["Farm"], ResourceNodeType.Farm));
        }

        public static Dictionary<string, ResourceNodeBase> Bases = new Dictionary<string, ResourceNodeBase>();

        public string Name { get; private set; }
        public Point Size { get; private set; }
        public Texture2D Texture {get; private set;}
        public ResourceNodeType ResourceNodeType { get; private set; }

        public ResourceNodeBase(string name, Point size, Texture2D texture,ResourceNodeType resourceNodeType)
        {
            Name = name;
            Size = size;
            Texture = texture;
            ResourceNodeType = resourceNodeType;
        }
    }

    public class ResourceNode :Entity
    {
        public ResourceNodeType ResourceNodeType { get; private set; }

        public ResourceNode(ResourceNodeBase resourceNodeBase, Vector2 position):base(position,resourceNodeBase.Name, resourceNodeBase.Size, resourceNodeBase.Texture)
        {
            ResourceNodeType = resourceNodeBase.ResourceNodeType;
        }
    }
}