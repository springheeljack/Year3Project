using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyGame.GOAP.Actions
{
    public class CreateItem : GOAPAction
    {
        public override string ToString()
        {
            return "Create " + Recipe.Output.ToString();
        }

        bool Created = false;
        private double StartTime = 0;
        public float Duration;
        private Recipe Recipe;

        public CreateItem(Recipe recipe)
        {
            Recipe = recipe;
            foreach (ItemType i in Recipe.Input)
            {
                Preconditions.Add(new Tuple<string, object>("HasItem", i), true);
                Effects.Add(new Tuple<string, object>("HasItem", i), false);
            }
            Effects.Add(new Tuple<string, object>("HasItem", Recipe.Output), true);
            Cost = Recipe.Duration;
            Duration = Recipe.Duration;
        }

        public override bool CheckProceduralPrecondition(GOAPAgent agent)
        {
            List<Building> buildings = EntityManager.GetBuildings().Where(x => x.BuildingType == Recipe.Type).ToList();
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
            StartTime = 0;
        }

        public override bool Run(Entity entity)
        {
            if (StartTime == 0)
                StartTime = Global.gameTime.TotalGameTime.TotalSeconds;

            if (Global.gameTime.TotalGameTime.TotalSeconds - StartTime > Duration)
            {
                (entity as Unit).Inventory.AddItem(Recipe.Output);
                foreach (ItemType i in Recipe.Input)
                    (entity as Unit).Inventory.RemoveItem(i);
                Created = true;
            }
            return true;
        }
    }
}