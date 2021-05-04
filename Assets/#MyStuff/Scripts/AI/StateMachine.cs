using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    public class StateMachine
    {
        public bool hit { get; set; }

        public State CurrentState { get; set; }
        public readonly NPC npc;

        public StateMachine(NPC _npc)
        {
            npc = _npc;
        }

        public void Update()
        {
            CurrentState.UpdateState( this );
        }

        public void UpdatePhysics()
        {
            CurrentState.UpdatePhysicsState(this);
        }

        public void TransitionToState(State _newState)
        {
            if ( _newState.GetType() != typeof( RemainInState ))
            {
                CurrentState.Exit( npc );
                CurrentState = _newState;
                Debug.Log(_newState.name);
                CurrentState.Enter( npc );
            }
        }
    }
}
