using UnityEngine;
using DG.Tweening;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/LookAtPlayerBehaviour" )]
public class LookAtPlayerBehaviour : Behaviour
{
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        NavMeshAgentStateMachine machine = _stateMachine as NavMeshAgentStateMachine;

        // machine.Head.LookAt( machine.Player.transform, Vector3.up );
        machine.Head.DOLookAt( machine.Player.transform.position, 0.08f, AxisConstraint.None, Vector3.up ).SetEase( Ease.InOutCirc );
    }
}