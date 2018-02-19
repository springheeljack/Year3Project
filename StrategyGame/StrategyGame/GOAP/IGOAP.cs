using System;
using System.Collections.Generic;

namespace StrategyGame
{
    public interface IGOAP
    {
        Dictionary<Tuple<string, object>, object> GetWorldState();

        Dictionary<Tuple<string, object>, object> CreateGoalState();

        void PlanFailed(Dictionary<Tuple<string, object>, object> failedGoal);

        void PlanFound(Dictionary<Tuple<string, object>, object> goal, Queue<GOAPAction> actions);

        void ActionsFinished();

        void PlanAborted(GOAPAction aborter);

        bool MoveAgent(GOAPAction nextAction);
    }
}
