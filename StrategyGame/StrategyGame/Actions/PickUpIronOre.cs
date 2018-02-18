using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Actions
{
    public class PickUpIronOre : GOAPAction
    {
        private bool PickedUp = false;

        public PickUpIronOre()
        {
            Preconditions.Add("HasIronOre", false);
            Effects.Add("HasIronOre", true);
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
                if (b.Inventory.Items.Contains(ItemType.IronOre))
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
            (entity as Unit).Inventory.AddItem(ItemType.IronOre);
            (Target as Building).Inventory.RemoveItem(ItemType.IronOre);
            PickedUp = true;

            return true;
        }
    }
}