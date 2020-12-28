using NonViolentFPS.NPCs;

namespace NonViolentFPS.AI
{
    public class StateMachine
    {
        public bool hit { get; set; }

        private State currentState;
        public readonly NPC npc;

        public StateMachine(NPC _npc)
        {
            npc = _npc;
        }

        public void Update()
        {
            currentState.UpdateState( this );
        }

        public void TransitionToState(State _newState)
        {
            if ( _newState.GetType() != typeof( RemainInState ) )
            {
                currentState.Exit( npc );
                currentState = _newState;
                currentState.Enter( npc );
            }
        }
    }
}
