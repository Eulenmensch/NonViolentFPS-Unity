using UnityEngine;
using DG.Tweening;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/LookAtPlayerBehaviour" )]
public class LookAtPlayerBehaviour : Behaviour
{
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        _stateMachine.Head.DOLookAt( Camera.main.transform.position, 0.08f, AxisConstraint.None, Vector3.up ).SetEase( Ease.InOutCirc );
    }
}