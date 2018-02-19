using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Actions
{
    public class GatherIronOreWithHands : GOAPAction
    {
        public override string ToString()
        {
            return "Gather iron ore with hands";
        }

        private bool Gathered = false;

        private double StartTime = 0;
        public float GatherDuration = 5;

        public GatherIronOreWithHands()
        {
            Preconditions.Add("HasPickaxe", false);
            Preconditions.Add("HasIronOre", false);
            Effects.Add("HasIronOre", true);
            Cost = 5;
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
            foreach (ResourceNode rn in resourceNodes.Where(x => x.ResourceNodeType == ResourceNodeType.IronRock))
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
                (entity as Unit).Inventory.AddItem(ItemType.IronOre);
                Gathered = true;
            }
            return true;
        }
    }
}