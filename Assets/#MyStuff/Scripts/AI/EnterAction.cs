using UnityEngine;

public abstract class EnterAction : ScriptableObject
{
    public abstract void Enter(StateMachine _stateMachine);
}