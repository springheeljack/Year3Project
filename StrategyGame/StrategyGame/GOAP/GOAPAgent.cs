using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private LinkedList<Text> ThoughtLog = new LinkedList<Text>();
        public Vector2 PlannedLocation;

        public LinkedList<Text> GetThoughts()
        {
            return ThoughtLog;
        }

        public GOAPAgent(Vector2 position, string name, Point size, Texture2D texture) : base(position, name, size, texture) { PlannedLocation = position; }

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

        public void UpdateAgent()
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
                Dictionary<Tuple<string, object>, object> WorldState = DataProvider.GetWorldState();
                Dictionary<Tuple<string, object>, object> Goal = DataProvider.CreateGoalState();

                Queue<GOAPAction> Plan = Planner.Plan(agent, AvailableActions, WorldState, Goal);
                if (Plan != null)
                {
                    AddThought("Plan created for goal " + Goal.Keys.First(), Color.DarkBlue);
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
                {
                    fsm.PopState();
                    AddThought("Performing action " + Action.ToString(), Color.Blue);
                }
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
                {
                    AddThought("Completed action " + action.ToString(),Color.DarkOrange);
                    CurrentActions.Dequeue();
                }

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
                        AddThought("Moving to " + action.Target.Name,Color.DarkOliveGreen);
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

        public void AddThought(string thoughtString, Color thoughtColor)
        {
            AddThought(new Text(thoughtString, thoughtColor));
        }
        public void AddThought(Text thought)
        {
            ThoughtLog.AddFirst(thought);
        }
    }
}