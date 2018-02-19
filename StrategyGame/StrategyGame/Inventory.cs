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
        public static string GetItemName(ItemType item)
        {
            switch (item)
            {
                case ItemType.Log:
                    return "Log";
                case ItemType.IronOre:
                    return "Iron Ore";
                case ItemType.Iron:
                    return "Iron";
                case ItemType.IronAxe:
                    return "Iron Axe";
                case ItemType.IronPickaxe:
                    return "Iron Pickaxe";
            }
            return "Error";
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