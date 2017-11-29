using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public interface IGatherer : ISelectable
    {
        //float MiningSpeed { get; }
        //int MaxCapacity { get; }
        GathererUnitBase UnitBase { get; }
        int CarriedResources { get; set; }
        float GatherTimer { get; set; }
        ResourceNode GatherTarget { get; set; }
        void DrawGatherReticle(SpriteBatch spriteBatch);
    }
}
