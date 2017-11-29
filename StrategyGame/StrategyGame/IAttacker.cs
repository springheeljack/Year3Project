using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public interface IAttacker: ISelectable
    {
        //int AttackDamage { get; }
        //float AttackSpeed { get; }
        MeleeUnitBase UnitBase { get; }
        float AttackTimer { get; }
        IHealth AttackTarget { get; set; }
        void DrawAttackReticle(SpriteBatch spriteBatch);
    }
}