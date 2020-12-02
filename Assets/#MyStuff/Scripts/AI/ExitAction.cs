using UnityEngine;

namespace NonViolentFPS.AI
{
    public abstract class ExitAction : ScriptableObject
    {
        public abstract void Exit(StateMachine _stateMachine);
    }
}