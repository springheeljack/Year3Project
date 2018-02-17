using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class GOAPPlanner
    {
        private class Node
        {
            public Node Parent;
            public float TotalCost;
            public Dictionary<string, object> State;
            public GOAPAction Action;

            public Node(Node parent, float totalCost, Dictionary<string, object> state, GOAPAction action)
            {
                Parent = parent;
                TotalCost = totalCost;
                State = state;
                Action = action;
            }
        }

        public Queue<GOAPAction> Plan(GOAPAgent agent, List<GOAPAction> availableActions, Dictionary<string, object> worldState, Dictionary<string, object> goal)
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

        private bool BuildGraph(Node parent, List<Node> tree, List<GOAPAction> usableActions, Dictionary<string, object> goal)
        {
            bool HasSolution = false;

            foreach (GOAPAction a in usableActions)
                if (InState(a.Preconditions, parent.State))
                {
                    Dictionary<string, object> CurrentState = PopulateState(parent.State, a.Effects);

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

        private bool InState(Dictionary<string, object> test, Dictionary<string, object> state)
        {
            bool AllMatch = true;
            foreach (KeyValuePair<string, object> t in test)
            {
                bool Match = false;
                foreach (KeyValuePair<string, object> s in state)
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

        private Dictionary<string,object> PopulateState(Dictionary<string,object> currentState, Dictionary<string,object> stateChange)
        {
            Dictionary<string, object> State = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> kvp in currentState)
                State.Add(kvp.Key, kvp.Value);

            foreach (KeyValuePair<string, object> change in stateChange)
            {
                bool Exists = false;

                foreach (KeyValuePair<string, object> s in State)
                    if (s.Key == change.Key)
                    {
                        Exists = true;
                        break;
                    }

                if (Exists)
                {
                    State.Remove(change.Key);
                    State.Add(change.Key, change.Value);

                    //Dictionary<string, object> temp = new Dictionary<string, object>();
                    //foreach (KeyValuePair<string, object> kvp in State.Where(x => !x.Equals(change.Key)))
                    //    temp.Add(kvp.Key, kvp.Value);
                    //State.Clear();
                    //State = temp;
                    //State.Add(change.Key, change.Value);
                    /////// this is jank
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