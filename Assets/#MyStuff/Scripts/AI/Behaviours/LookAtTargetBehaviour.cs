using UnityEngine;
using DG.Tweening;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/LookAtTargetBehaviour" )]
public class LookAtTargetBehaviour : AIBehaviour
{
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        // machine.Head.LookAt( machine.LookAtTarget, Vector3.up );
        _stateMachine.Head.DOLookAt( _stateMachine.LookAtTarget.position, 0.08f, AxisConstraint.None, Vector3.up ).SetEase( Ease.InOutCirc );
    }
}