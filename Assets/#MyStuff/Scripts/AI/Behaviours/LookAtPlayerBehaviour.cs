using DG.Tweening;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/LookAtPlayerBehaviour" )]
    public class LookAtPlayerBehaviour : AIBehaviour
    {
        [SerializeField] private float tweenDuration;

        public override UpdateType type => UpdateType.Regular;

        public override void DoBehaviour(NPC _npc)
        {
            var headComponent = _npc as IHeadComponent;
            if (headComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IHeadComponent));
                return;
            }

            headComponent.Head.DOLookAt( Camera.main.transform.position, tweenDuration, AxisConstraint.None, Vector3.up ).SetEase( Ease.InOutCirc );
        }
    }
}