using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    public class StateMachine
    {
        public bool hit { get; set; }
        public State CurrentState { get; set; }

        public readonly NPC OwnerNPC;
        private readonly State anyState;

        public StateMachine(NPC _ownerNPC, State _anyState)
        {
            OwnerNPC = _ownerNPC;
            anyState = _anyState;
        }

        public void Update()
        {
            CurrentState.UpdateState( this );
            anyState.UpdateState(this);
        }

        public void UpdatePhysics()
        {
            CurrentState.UpdatePhysicsState(this);
            anyState.UpdatePhysicsState(this);
        }

        public void TransitionToState(State _newState)
        {
            if ( _newState.GetType() != typeof( RemainInState ))
            {
                CurrentState.Exit( OwnerNPC );
                CurrentState = _newState;
                Debug.Log(_newState.name);
                CurrentState.Enter( OwnerNPC );
            }
        }
    }
}
