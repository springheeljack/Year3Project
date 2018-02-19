using System;
using System.Collections.Generic;

namespace StrategyGame
{
    public class GOAPPlanner
    {
        private class Node
        {
            public Node Parent;
            public float TotalCost;
            public Dictionary<Tuple<string, object>, object> State;
            public GOAPAction Action;

            public Node(Node parent, float totalCost, Dictionary<Tuple<string, object>, object> state, GOAPAction action)
            {
                Parent = parent;
                TotalCost = totalCost;
                State = state;
                Action = action;
            }
        }

        public Queue<GOAPAction> Plan(GOAPAgent agent, List<GOAPAction> availableActions, Dictionary<Tuple<string, object>, object> worldState, Dictionary<Tuple<string, object>, object> goal)
        {
            foreach (GOAPAction a in availableActions)
                a.Reset();

            List<GOAPAction> UseableActions = new List<GOAPAction>();
            foreach (GOAPAction a in availableActions)
                if (a.CheckProceduralPrecondition(agent))
                    UseableActions.Add(a);

            List<Node> Tree = new List<Node>();

            Node Start = new Node(null, 0, worldState, null);
            bool Success = BuildGraph(Start, Tree, UseableActions, goal);

            if (!Success)
                return null;

            Node Cheapest = null;
            foreach (Node node in Tree)
                if (Cheapest == null)
                    Cheapest = node;
                else
                {
                    if (node.TotalCost < Cheapest.TotalCost)
                        Cheapest = node;
                }

            List<GOAPAction> Result = new List<GOAPAction>();
            Node n = Cheapest;
            while(n!=null)
            {
                if (n.Action != null)
                    Result.Insert(0, n.Action);
                n = n.Parent;
            }

            Queue<GOAPAction> queue = new Queue<GOAPAction>();
            foreach (GOAPAction a in Result)
                queue.Enqueue(a);

            return queue;
        }

        private bool BuildGraph(Node parent, List<Node> tree, List<GOAPAction> usableActions, Dictionary<Tuple<string, object>, object> goal)
        {
            bool HasSolution = false;

            foreach (GOAPAction a in usableActions)
                if (InState(a.Preconditions, parent.State))
                {
                    Dictionary<Tuple<string, object>, object> CurrentState = PopulateState(parent.State, a.Effects);

                    Node Node = new Node(parent, parent.TotalCost + a.Cost, CurrentState, a);

                    if (InState(goal, CurrentState))
                    {
                        tree.Add(Node);
                        HasSolution = true;
                    }
                    else
                    {
                        List<GOAPAction> Subset = ActionSubset(usableActions, a);
                        bool Found = BuildGraph(Node, tree, Subset, goal);
                        if (Found)
                            HasSolution = true;
                    }
                }
            return HasSolution;
        }

        private bool InState(Dictionary<Tuple<string, object>, object> test, Dictionary<Tuple<string, object>, object> state)
        {
            bool AllMatch = true;
            foreach (KeyValuePair<Tuple<string, object>, object> t in test)
            {
                bool Match = false;
                foreach (KeyValuePair<Tuple<string, object>, object> s in state)
                    if (s.Equals(t))
                    {
                        Match = true;
                        break;
                    }
                if (!Match)
                    AllMatch = false; ////////////////////this bit is a bit jank
            }
            return AllMatch;
        }

        private Dictionary<Tuple<string, object>, object> PopulateState(Dictionary<Tuple<string, object>, object> currentState, Dictionary<Tuple<string, object>, object> stateChange)
        {
            Dictionary<Tuple<string, object>, object> State = new Dictionary<Tuple<string, object>, object>();
            foreach (KeyValuePair<Tuple<string, object>, object> kvp in currentState)
                State.Add(kvp.Key, kvp.Value);

            foreach (KeyValuePair<Tuple<string, object>, object> change in stateChange)
            {
                bool Exists = false;

                foreach (KeyValuePair<Tuple<string, object>, object> s in State)
                    if (s.Key.Equals(change.Key))
                    {
                        Exists = true;
                        break;
                    }

                if (Exists)
                {
                    State.Remove(change.Key);
                    State.Add(change.Key, change.Value);
                }
                else
                    State.Add(change.Key, change.Value);
                
            }
            return State;
        }

        private List<GOAPAction> ActionSubset(List<GOAPAction> actions, GOAPAction remove)
        {
            List<GOAPAction> Subset = new List<GOAPAction>();
            foreach (GOAPAction a in actions)
                if (!a.Equals(remove))
                    Subset.Add(a);
            return Subset;
        }
    }
}