using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Actions
{
    public class StoreLog : GOAPAction
    {
        public override string ToString()
        {
            return "Store log";
        }

        private bool Stored = false;

        public StoreLog()
        {
            Preconditions.Add("HasLog", true);
            Effects.Add("HasLog", false);
            Effects.Add("StoreLog", true);
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
            (Target as Building).Inventory.AddItem(ItemType.Log);
            (entity as Unit).Inventory.RemoveItem(ItemType.Log);
            Stored = true;
            return true;
        }
    }
}