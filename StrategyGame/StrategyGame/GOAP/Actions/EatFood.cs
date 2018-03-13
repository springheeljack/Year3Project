using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.GOAP.Actions
{
    public class EatFood : GOAPAction
    {
        public override string ToString()
        {
            return "Eat " + Food.ItemType.ToString();
        }

        private bool Eaten = false;
        private Food Food;

        public EatFood(Food food)
        {
            Food = food;
            Preconditions.Add(new Tuple<string, object>("HasItem", Food.ItemType), true);
            Effects.Add(new Tuple<string, object>("HasItem", Food.ItemType), false);
            Effects.Add(new Tuple<string, object>("EatItem", Food.ItemType), true);
            Cost = 5;
        }

        public override void ResetExtra()
        {
            Eaten = false;
        }

        public override bool IsDone()
        {
            return Eaten;
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
                if (b.Inventory.Items.ContainsKey(Food.ItemType))
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
            if (Target != null)
                agent.PlannedLocation = Target.Position;
            return nearest != null;
        }

        public override bool Run(Entity entity)
        {
            (entity as Unit).EatFood(Food);
            
            Eaten = true;

            return true;
        }
    }
}