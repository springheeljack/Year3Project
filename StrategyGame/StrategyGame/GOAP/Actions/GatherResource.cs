using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyGame.GOAP.Actions
{
    public class GatherResource : GOAPAction
    {
        public override string ToString()
        {
            return "Gather " + Gather.Type.ToString();
        }

        private bool Gathered = false;

        private double StartTime = 0;
        private float Duration;
        private Gather Gather;

        public GatherResource(Gather gather)
        {
            Gather = gather;
            foreach (ItemType i in Gather.Input)
                Preconditions.Add(new Tuple<string, object>("HasItem", i), true);
            Effects.Add(new Tuple<string, object>("HasItem", Gather.Output), true);
            Duration = Gather.Duration;
            Cost = Gather.Duration;
        }

        public override void ResetExtra()
        {
            Gathered = false;
            StartTime = 0;
        }

        public override bool IsDone()
        {
            return Gathered;
        }

        public override bool RequiresInRange()
        {
            return true;
        }

        public override bool CheckProceduralPrecondition(GOAPAgent agent)
        {
            List<ResourceNode> resourceNodes = EntityManager.GetResourceNodes();
            ResourceNode nearest = null;
            float distance = 0;
            foreach (ResourceNode rn in resourceNodes.Where(x => x.ResourceNodeType == Gather.Type))
            {
                if (nearest == null)
                {
                    nearest = rn;
                    distance = ((agent as Unit).Position.Distance(rn.Position));
                }
                else
                {
                    float tempDistance = ((agent as Unit).Position.Distance(rn.Position));
                    if (tempDistance < distance)
                    {
                        nearest = rn;
                        distance = tempDistance;
                    }
                }
            }
            Target = nearest;
            return nearest != null;
        }

        public override bool Run(Entity entity)
        {
            if (StartTime == 0)
                StartTime = Global.gameTime.TotalGameTime.TotalSeconds;

            if (Global.gameTime.TotalGameTime.TotalSeconds - StartTime > Duration)
            {
                (entity as Unit).Inventory.AddItem(Gather.Output);
                Gathered = true;
            }
            return true;
        }
    }
}