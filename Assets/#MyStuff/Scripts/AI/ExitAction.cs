using UnityEngine;

public abstract class ExitAction : ScriptableObject
{
    public abstract void Exit(StateMachine _stateMachine);
}