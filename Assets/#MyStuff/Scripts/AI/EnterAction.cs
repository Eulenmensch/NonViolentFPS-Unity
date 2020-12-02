using UnityEngine;

namespace NonViolentFPS.AI
{
    public abstract class EnterAction : ScriptableObject
    {
        public abstract void Enter(StateMachine _stateMachine);
    }
}