using UnityEngine;
using DG.Tweening;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/LookAtTargetBehaviour" )]
public class LookAtTargetBehaviour : Behaviour
{
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        NavMeshAgentStateMachine machine = _stateMachine as NavMeshAgentStateMachine;
        // machine.Head.LookAt( machine.LookAtTarget, Vector3.up );
        machine.Head.DOLookAt( machine.LookAtTarget.position, 0.08f, AxisConstraint.None, Vector3.up ).SetEase( Ease.InOutCirc );
    }
}