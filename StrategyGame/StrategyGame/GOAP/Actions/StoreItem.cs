using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyGame.GOAP.Actions
{
    public class StoreItem : GOAPAction
    {
        public override string ToString()
        {
            return "Store " + Item.ToString();
        }

        private bool Stored = false;
        private ItemType Item;

        public StoreItem(ItemType item)
        {
            Item = item;
            Preconditions.Add(new Tuple<string, object>("HasItem", item), true);
            Effects.Add(new Tuple<string, object>("HasItem", item), false);
            Effects.Add(new Tuple<string, object>("StoreItem", item), true);
            Cost = 1;
        }

        public override void ResetExtra()
        {
            Stored = false;
        }

        public override bool IsDone()
        {
            return Stored;
        }

        public override bool RequiresInRange()
        {
            return true;
        }

        public override bool CheckProceduralPrecondition(GOAPAgent agent)
        {
            List<Building> buildings = EntityManager.GetBuildings();
            Building nearest = null;
            float distance = 0;
            foreach (Building b in buildings.Where(x => x.BuildingType == BuildingType.Stockpile))
            {
                if (nearest == null)
                {
                    nearest = b;
                    distance = ((agent as Unit).Position.Distance(b.Position));
                }
                else
                {
                    float tempDistance = ((agent as Unit).Position.Distance(b.Position));
                    if (tempDistance < distance)
                    {
                        nearest = b;
                        distance = tempDistance;
                    }
                }
            }
            Target = nearest;
            return nearest != null;
        }

        public override bool Run(Entity entity)
        {
            (Target as Building).Inventory.AddItem(Item);
            (entity as Unit).Inventory.RemoveItem(Item);
            Stored = true;
            return true;
        }
    }
}