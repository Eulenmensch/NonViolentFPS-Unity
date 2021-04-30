using DG.Tweening;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/LookAtPlayerBehaviour" )]
    public class LookAtPlayerBehaviour : AIBehaviour
    {
        public override UpdateType type => UpdateType.Regular;

        public override void DoBehaviour(NPC _npc)
        {
            var headComponent = _npc as IHeadComponent;
            if (headComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IHeadComponent));
                return;
            }

            headComponent.Head.DOLookAt( Camera.main.transform.position, 0.08f, AxisConstraint.None, Vector3.up ).SetEase( Ease.InOutCirc );
        }
    }
}