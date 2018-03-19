using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class Enemy : Entity
    {
        Unit Target = null;
        public Enemy(Vector2 position) : base(position, "Enemy", new Point(32), Art.Textures["Creep"])
        {

        }

        public override void Update()
        {
            if (Target == null)
            {
                List<Unit> units = EntityManager.GetUnits();
                float dist = 99999999;
                foreach (Unit u in units)
                {
                    if (!(u is Fighter))
                        if (u.Position.Distance(Position) < dist)
                        {
                            dist = u.Position.Distance(Position);
                            Target = u;
                        }
                }
            }

            if (Target != null)
            {
                Vector2 movement = Target.Position - Position;
                movement.Normalize();
                movement *= (float)Global.gameTime.ElapsedGameTime.TotalSeconds;
                movement *= 50.0f;
                Position += movement;
                if (Target.Position.Distance(Position) < 10.0f)
                {
                    EntityManager.ToRemove.Add(Target);
                    Target = null;
                }
            }
            base.Update();
        }
    }
}
