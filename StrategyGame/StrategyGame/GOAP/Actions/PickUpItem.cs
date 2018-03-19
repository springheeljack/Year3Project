using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyGame.GOAP.Actions
{
    public class PickUpItem : GOAPAction
    {
        public override string ToString()
        {
            return "Pick up " + Item.ToString();
        }

        private bool PickedUp = false;
        private ItemType Item;

        public PickUpItem(ItemType item)
        {
            Item = item;
            Preconditions.Add(new Tuple<string, object>("HasItem", item), false);
            Effects.Add(new Tuple<string, object>("HasItem", item), true);
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
                if (b.Inventory.Items.ContainsKey(Item))
                    if (nearest == null)
                    {
                        nearest = b;
                        distance = (agent.PlannedLocation.Distance(b.Position));
                    }
                    else
                    {
                        float tempDistance = agent.PlannedLocation.Distance(b.Position);
                        if (tempDistance < distance)
                        {
                            nearest = b;
                            distance = tempDistance;
                        }
                    }
            }
            Target = nearest;
            if (Target !=null)
                agent.PlannedLocation = Target.Position;
            return nearest != null;
        }

        public override bool Run(Entity entity)
        {
            if ((Target as Building).Inventory.Items.ContainsKey(Item))
            {
                (entity as Unit).Inventory.AddItem(Item);
                (Target as Building).Inventory.RemoveItem(Item);
                PickedUp = true;
                return true;
            }
            else
                return false;
        }
    }
}