using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Actions
{
    public class StorePickaxe : GOAPAction
    {
        bool Stored = false;

        public StorePickaxe()
        {
            Cost = 1;
            Preconditions.Add("HasPickaxe", true);
            Effects.Add("HasPickaxe", false);
            Effects.Add("StorePickaxe", true);
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
            return Stored;
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
            (entity as Unit).Inventory.RemoveItem(ItemType.IronPickaxe);
            (Target as Building).Inventory.AddItem(ItemType.IronPickaxe);

            Stored = true;
            return true;
        }
    }
}