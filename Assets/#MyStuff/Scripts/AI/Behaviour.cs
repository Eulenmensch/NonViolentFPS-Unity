using UnityEngine;

public abstract class Behaviour : ScriptableObject
{
    public abstract void DoBehaviour(StateMachine _stateMachine);
}