using DG.Tweening;
using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Exit Actions/ResetHeadRotationExitAction" )]
    public class ResetHeadRotationExitAction : ExitAction
    {
        public override void Exit(NPC _npc)
        {
            var headComponent = _npc as IHeadComponent;
            if (headComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IHeadComponent));
                return;
            }

            headComponent.Head.DOKill();
            headComponent.Head.DOLocalRotate( Vector3.zero, 0.05f, RotateMode.Fast );
        }
    }
}