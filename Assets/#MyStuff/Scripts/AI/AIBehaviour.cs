using Sirenix.OdinInspector;

public abstract class AIBehaviour : SerializedScriptableObject
{
    public abstract void DoBehaviour(StateMachine _stateMachine);
}
