using UnityEngine;

[CreateAssetMenu( menuName = "AI Kit/Exit Actions/ResetHeadRotationExitAction" )]
public class ResetHeadRotationExitAction : ExitAction
{
    public override void Exit(StateMachine _stateMachine)
    {
        _stateMachine.Head.localRotation = Quaternion.identity;
    }
}