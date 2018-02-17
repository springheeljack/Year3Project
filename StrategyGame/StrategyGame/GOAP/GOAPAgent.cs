using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class GOAPAgent : Entity
    {
        private GOAPPlanner Planner;
        private List<GOAPAction> AvailableActions;
        private Queue<GOAPAction> CurrentActions;
        private FSM fsm;
        private IGOAP DataProvider;
        private FSM.FSMState IdleState;
        private FSM.FSMState MoveToState;
        private FSM.FSMState PerformActionState;

        public GOAPAgent(Vector2 position, string name, Point size, Texture2D texture) : base(position, name, size, texture) { }

        public void Start()
        {
            fsm = new FSM();
            AvailableActions = new List<GOAPAction>();
            CurrentActions = new Queue<GOAPAction>();
            Planner = new GOAPPlanner();
            FindDataProvider();
            CreateIdleState();
            CreateMoveToState();
            CreatePerformActionState();
            fsm.PushState(IdleState);
            LoadActions();
        }

        public void Update()
        {
            fsm.Update(this);
        }

        public void AddAction(GOAPAction action)
        {
            AvailableActions.Add(action);
        }

        public GOAPAction GetAction(Type action)
        {
            foreach (GOAPAction a in AvailableActions)
                if (a.GetType().Equals(a))
                    return a;
            return null;
        }

        public void RemoveAction(GOAPAction action)
        {
            AvailableActions.Remove(action);
        }

        private bool HasActionPlan()
        {
            return CurrentActions.Count > 0;
        }

        private void CreateIdleState()
        {
            IdleState = (fsm, agent) =>
            {
                Dictionary<string, object> WorldState = DataProvider.GetWorldState();
                Dictionary<string, object> Goal = DataProvider.CreateGoalState();

                Queue<GOAPAction> Plan = Planner.Plan(agent, AvailableActions, WorldState, Goal);
                if (Plan != null)
                {
                    CurrentActions = Plan;
                    DataProvider.PlanFound(Goal, Plan);

                    fsm.PopState();
                    fsm.PushState(PerformActionState);
                }
                else
                {
                    DataProvider.PlanFailed(Goal);
                    fsm.PopState();
                    fsm.PushState(IdleState);
                }
            };
        }

        private void CreateMoveToState()
        {
            MoveToState = (fsm, agent) =>
            {
                GOAPAction Action = CurrentActions.Peek();
                if (Action.RequiresInRange() && Action.Target == null)
                {
                    fsm.PopState();
                    fsm.PopState();
                    fsm.PushState(IdleState);
                    return;
                }

                if (DataProvider.MoveAgent(Action))
                    fsm.PopState();
            };
        }

        private void CreatePerformActionState()
        {
            PerformActionState = (fsm, agent) =>
            {
                if (!HasActionPlan())
                {
                    fsm.PopState();
                    fsm.PushState(IdleState);
                    DataProvider.ActionsFinished();
                }

                GOAPAction action = CurrentActions.Peek();
                if (action.IsDone())
                    CurrentActions.Dequeue();

                if (HasActionPlan())
                {
                    action = CurrentActions.Peek();
                    bool InRange = action.RequiresInRange() ? action.InRange : true;

                    if (InRange)
                    {
                        bool Success = action.Run(agent);

                        if (!Success)
                        {
                            fsm.PopState();
                            fsm.PushState(IdleState);
                            DataProvider.PlanAborted(action);
                        }
                    }
                    else
                    {
                        fsm.PushState(MoveToState);
                    }
                }
                else
                {
                    fsm.PopState();
                    fsm.PushState(IdleState);
                    DataProvider.ActionsFinished();
                }
            };
        }

        private void FindDataProvider()
        {
            DataProvider = this as IGOAP;
        }

        private void LoadActions()
        {
            AvailableActions = (this as Unit).Actions;
        }

    }
}