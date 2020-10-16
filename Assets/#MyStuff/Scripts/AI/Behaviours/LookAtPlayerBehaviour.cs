using UnityEngine;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/LookAtPlayerBehaviour" )]
public class LookAtPlayerBehaviour : Behaviour
{
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        NavMeshAgentStateMachine machine = _stateMachine as NavMeshAgentStateMachine;

        machine.Head.LookAt( machine.Player.transform, Vector3.up );
    }
}