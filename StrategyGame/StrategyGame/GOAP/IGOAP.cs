using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public interface IGOAP
    {
        Dictionary<string, object> GetWorldState();

        Dictionary<string, object> CreateGoalState();

        void PlanFailed(Dictionary<string, object> failedGoal);

        void PlanFound(Dictionary<string, object> goal, Queue<GOAPAction> actions);

        void ActionsFinished();

        void PlanAborted(GOAPAction aborter);

        bool MoveAgent(GOAPAction nextAction);
    }
}
