using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Actions
{
    public class StoreAxe : GOAPAction
    {
        bool Stored = false;

        public StoreAxe()
        {
            Cost = 1;
            Preconditions.Add("HasAxe", true);
            Effects.Add("HasAxe", false);
        }

        public override bool CheckProceduralPrecondition(GOAPAgent agent)
        {
            List<Building> buildings = EntityManager.GetBuildings().Where(x => x.BuildingType == BuildingType.Stockpile).ToList();
            Building nearest = null;
            float distance = 0;
            foreach (Building b in buildings)
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
            Target = nearest;
            return nearest != null;
        }

        public override bool IsDone()
        {
            return Stored = true;
        }

        public override bool RequiresInRange()
        {
            return true;
        }

        public override void ResetExtra()
        {
            Stored = false;
        }

        public override bool Run(Entity entity)
        {
            (entity as Unit).Inventory.RemoveItem(ItemType.IronAxe);
            (Target as Building).Inventory.AddItem(ItemType.IronAxe);

            Stored = true;
            return true;
        }
    }
}