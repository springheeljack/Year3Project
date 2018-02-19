using System.Collections.Generic;

namespace StrategyGame
{
    public class FSM
    {
        private Stack<FSMState> StateStack = new Stack<FSMState>();

        public delegate void FSMState(FSM fsm, GOAPAgent agent);

        public void Update(GOAPAgent agent)
        {
            if (StateStack.Peek() != null)
                StateStack.Peek().Invoke(this, agent);
        }

        public void PushState(FSMState state)
        {
            StateStack.Push(state);
        }

        public void PopState()
        {
            StateStack.Pop();
        }
    }
}
