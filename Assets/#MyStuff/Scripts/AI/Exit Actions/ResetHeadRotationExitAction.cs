using UnityEngine;
using DG.Tweening;

[CreateAssetMenu( menuName = "AI Kit/Exit Actions/ResetHeadRotationExitAction" )]
public class ResetHeadRotationExitAction : ExitAction
{
    public override void Exit(StateMachine _stateMachine)
    {
        _stateMachine.Head.DOKill();
        // _stateMachine.Head.localRotation = Quaternion.identity;
        _stateMachine.Head.DOLocalRotate( Vector3.zero, 0.05f, RotateMode.Fast );
    }
}