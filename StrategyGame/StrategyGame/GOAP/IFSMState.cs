namespace StrategyGame
{
    public interface IFSMState
    {
        void Update(FSM fsm, GOAPAgent agent);
    }
}