using DG.Tweening;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Exit Actions/ResetHeadRotationExitAction" )]
    public class ResetHeadRotationExitAction : ExitAction
    {
        [SerializeField] private float rotationTime;

        public override void Exit(NPC _npc)
        {
            var headComponent = _npc as IHeadComponent;
            if (headComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IHeadComponent));
                return;
            }

            headComponent.Head.DOKill();
            headComponent.Head.DOLocalRotate( Vector3.zero, rotationTime, RotateMode.Fast );
        }
    }
}