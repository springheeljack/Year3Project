using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Actions
{
    public class GatherTree : GOAPAction
    {
        private bool Gathered = false;
        //private ResourceNode TargetNode;

        private double StartTime = 0;
        public float GatherDuration = 2;

        public GatherTree()
        {
            Preconditions.Add("HasLog", false);
            Effects.Add("HasLog", true);
            Cost = 2;
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
            foreach (ResourceNode rn in resourceNodes.Where(x => x.ResourceNodeType == ResourceNodeType.Tree))
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

            if (Global.gameTime.TotalGameTime.TotalSeconds - StartTime > GatherDuration)
            {
                //Finished Gathering
                (entity as Unit).Inventory.AddItem(ItemType.Log);
                Gathered = true;
            }
            return true;
        }
    }
}