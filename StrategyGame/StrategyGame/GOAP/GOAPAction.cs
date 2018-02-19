using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public abstract class GOAPAction
    {
        public Dictionary<string, object> Preconditions;
        public Dictionary<string, object> Effects;

        public bool InRange { get; set; } = false;
        public float Cost = 1f;
        public Entity Target;

        public GOAPAction()
        {
            Preconditions = new Dictionary<string, object>();
            Effects = new Dictionary<string, object>();
        }

        public void Reset()
        {
            InRange = false;
            Target = null;
            ResetExtra();
        }

        public abstract void ResetExtra();

        public abstract bool IsDone();

        public abstract bool CheckProceduralPrecondition(GOAPAgent agent);

        public abstract bool Run(Entity entity);

        public abstract bool RequiresInRange();

        public override abstract string ToString();
    }
}