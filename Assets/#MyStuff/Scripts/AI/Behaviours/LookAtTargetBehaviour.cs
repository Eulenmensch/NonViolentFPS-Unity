using UnityEngine;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/LookAtTargetBehaviour" )]
public class LookAtTargetBehaviour : Behaviour
{
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        NavMeshAgentStateMachine machine = _stateMachine as NavMeshAgentStateMachine;

        machine.Head.LookAt( machine.LookAtTarget, Vector3.up );
    }
}