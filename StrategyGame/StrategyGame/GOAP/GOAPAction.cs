using System;
using System.Collections.Generic;

namespace StrategyGame
{
    public abstract class GOAPAction
    {
        public Dictionary<Tuple<string, object>, object> Preconditions;
        public Dictionary<Tuple<string, object>, object> Effects;

        public bool InRange { get; set; } = false;
        public float Cost = 1f;
        public Entity Target;

        public GOAPAction()
        {
            Preconditions = new Dictionary<Tuple<string, object>, object>();
            Effects = new Dictionary<Tuple<string, object>, object>();
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