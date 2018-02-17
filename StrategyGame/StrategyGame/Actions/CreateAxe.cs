using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Actions
{
    public class CreateAxe : GOAPAction
    {
        bool Created = false;

        public CreateAxe()
        {
            Preconditions.Add("HasLog", true);
            Effects.Add("HasAxe", true);
            Effects.Add("HasLog", false);
            Cost = 5;
        }

        public override bool CheckProceduralPrecondition(GOAPAgent agent)
        {
            List<Building> buildings = EntityManager.GetBuildings().Where(x => x.BuildingType == BuildingType.Forge).ToList();
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
            return Created;
        }

        public override bool RequiresInRange()
        {
            return true;
        }

        public override void ResetExtra()
        {
            Created = false;
        }

        public override bool Run(Entity entity)
        {
            (entity as Unit).Inventory.AddItem(ItemType.IronAxe);
            (entity as Unit).Inventory.RemoveItem(ItemType.Log);

            Created = true;
            return true;
        }
    }
}