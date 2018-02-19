using System.Collections.Generic;

namespace StrategyGame
{
    public class Gather
    {
        public static Dictionary<string, Gather> Dictionary = new Dictionary<string, Gather>();

        public static void Initialize()
        {
            Dictionary = new Dictionary<string, Gather>
            {
                {"IronRock",new Gather(ResourceNodeType.IronRock,ItemType.IronOre,new List<ItemType>{ItemType.IronPickaxe},2) },
                {"IronRockNoPickaxe",new Gather(ResourceNodeType.IronRock,ItemType.IronOre,new List<ItemType>(),5) },
                {"CoalRock",new Gather(ResourceNodeType.CoalRock,ItemType.Coal,new List<ItemType>{ItemType.IronPickaxe},2) },
                {"CoalRockNoPickaxe",new Gather(ResourceNodeType.CoalRock,ItemType.Coal,new List<ItemType>(),5) },
                {"Sticks",new Gather(ResourceNodeType.Sticks,ItemType.Log,new List<ItemType>(),5) },
                {"Tree",new Gather(ResourceNodeType.Tree,ItemType.Log,new List<ItemType>{ItemType.IronAxe},2) }
            };
        }

        public ResourceNodeType Type { get; private set; }
        public ItemType Output { get; private set; }
        public List<ItemType> Input { get; private set; }
        public float Duration { get; private set; }

        public Gather(ResourceNodeType type, ItemType output, List<ItemType> input, float duration)
        {
            Type = type;
            Output = output;
            Input = input;
            Duration = duration;
        }
    }
}
