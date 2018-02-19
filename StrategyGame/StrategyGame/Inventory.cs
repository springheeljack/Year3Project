using System.Collections.Generic;

namespace StrategyGame
{
    public class Inventory
    {
        public Dictionary<ItemType,int> Items { get; private set; }
        //public List<ItemType> Items { get; private set; }
        public int Capacity { get; private set; }
        public int ItemCount { get; private set; }
        public Inventory(int capacity)
        {
            Capacity = capacity;
            ItemCount = 0;
            Items = new Dictionary<ItemType, int>();
        }
        public void AddItem(ItemType itemType)
        {
            if (Items.ContainsKey(itemType))
                Items[itemType]++;
            else
                Items.Add(itemType, 1);
            ItemCount++;
        }
        public void RemoveItem(ItemType itemType)
        {
            if (Items.ContainsKey(itemType))
            {
                Items[itemType]--;
                if (Items[itemType] == 0)
                    Items.Remove(itemType);
            }
            ItemCount--;
        }
        //public static string GetItemName(ItemType item)
        //{
        //    switch (item)
        //    {
        //        case ItemType.Log:
        //            return "Log";
        //        case ItemType.IronOre:
        //            return "Iron Ore";
        //        case ItemType.Iron:
        //            return "Iron";
        //        case ItemType.IronAxe:
        //            return "Iron Axe";
        //        case ItemType.IronPickaxe:
        //            return "Iron Pickaxe";
        //    }
        //    return "Error";
        //}
    }
}