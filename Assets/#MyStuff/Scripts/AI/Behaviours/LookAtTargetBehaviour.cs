using DG.Tweening;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/LookAtTargetBehaviour" )]
    public class LookAtTargetBehaviour : AIBehaviour
    {
        public override UpdateType type => UpdateType.Regular;

        public override void DoBehaviour(NPC _npc)
        {
            var lookAtComponent = _npc as ILookAtComponent;
            if (lookAtComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(ILookAtComponent));
                return;
            }
            var headComponent = _npc as IHeadComponent;
            if (headComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IHeadComponent));
                return;
            }

            headComponent.Head.DOLookAt( lookAtComponent.LookAtTarget.position, 0.08f, AxisConstraint.None, Vector3.up ).SetEase( Ease.InOutCirc );
        }
    }
}