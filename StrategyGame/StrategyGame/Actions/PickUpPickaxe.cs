using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Actions
{
    public class PickUpPickaxe : GOAPAction
    {
        public override string ToString()
        {
            return "Pick up pickaxe";
        }

        private bool PickedUp = false;

        public PickUpPickaxe()
        {
            Preconditions.Add("HasPickaxe", false);
            Effects.Add("HasPickaxe", true);
            Cost = 1;
        }

        public override void ResetExtra()
        {
            PickedUp = false;
        }

        public override bool IsDone()
        {
            return PickedUp;
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
                if (b.Inventory.Items.Contains(ItemType.IronPickaxe))
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
            (entity as Unit).Inventory.AddItem(ItemType.IronPickaxe);
            (Target as Building).Inventory.RemoveItem(ItemType.IronPickaxe);
            PickedUp = true;

            return true;
        }
    }
}