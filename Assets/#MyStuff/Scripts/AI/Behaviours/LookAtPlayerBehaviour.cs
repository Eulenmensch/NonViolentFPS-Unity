using DG.Tweening;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/LookAtPlayerBehaviour" )]
    public class LookAtPlayerBehaviour : AIBehaviour
    {
        public override void DoBehaviour(StateMachine _stateMachine)
        {
            _stateMachine.Head.DOLookAt( Camera.main.transform.position, 0.08f, AxisConstraint.None, Vector3.up ).SetEase( Ease.InOutCirc );
        }
    }
}