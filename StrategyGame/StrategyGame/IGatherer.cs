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
        UnitBaseGatherer Base { get; }
        int CarriedResources { get; set; }
        float GatherSpeed { get; set; }
        ResourceNode GatherTarget { get; set; }
        IResourceDeposit DepositTarget { get; set; }
        IRectangleObject CurrentTarget { get; set; }
        void DrawGatherReticle(SpriteBatch spriteBatch);
        void DrawDepositReticle(SpriteBatch spriteBatch);
    }
}
