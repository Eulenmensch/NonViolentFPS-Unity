using UnityEngine;

namespace NonViolentFPS.AI
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool Evaluate(StateMachine _stateMachine);
    }
}