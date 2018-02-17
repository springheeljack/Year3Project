using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class Inventory
    {
        public List<ItemType> Items { get; private set; }
        public int Capacity { get; private set; }
        public int ItemCount { get; private set; }
        public Inventory(int capacity)
        {
            Capacity = capacity;
            ItemCount = 0;
            Items = new List<ItemType>();
        }
        public void AddItem(ItemType itemType)
        {
            Items.Add(itemType);
            ItemCount++;
        }
        public void RemoveItem(ItemType itemType)
        {
            Items.Remove(itemType);
            ItemCount--;
        }
    }

    public enum ItemType
    {
        Log,
        IronOre,
        Iron,
        IronAxe,
        IronPickaxe
    }
}