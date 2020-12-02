using Sirenix.OdinInspector;

namespace NonViolentFPS.AI
{
    public abstract class AIBehaviour : SerializedScriptableObject
    {
        public abstract void DoBehaviour(StateMachine _stateMachine);
    }
}
