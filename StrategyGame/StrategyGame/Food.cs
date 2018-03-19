using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class Food
    {
        public static Dictionary<string, Food> Dictionary { get; private set; } = new Dictionary<string, Food>();
        public static void Initialize()
        {
            Dictionary.Add("Bread", new Food(ItemType.Bread, 30));
        }

        public ItemType ItemType { get; private set; }
        public int RestoreAmount { get; private set; }

        public Food(ItemType itemType, int restoreAmount)
        {
            ItemType = itemType;
            RestoreAmount = restoreAmount;
        }
    }
}
