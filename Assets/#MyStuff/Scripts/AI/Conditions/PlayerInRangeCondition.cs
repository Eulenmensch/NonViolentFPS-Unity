using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Conditions/PlayerInRangeCondition" )]
    public class PlayerInRangeCondition : Condition
    {
        public override bool Evaluate(NPC _npc)
        {
            var rangeComponent = _npc as IRangeComponent;
            if (rangeComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IRangeComponent));
                return false;
            }

            var playerPosition = _npc.Player.transform.position;
            var selfPosition = _npc.transform.position;

            return Vector3.Distance(playerPosition, selfPosition) <= rangeComponent.Range;
        }
    }
}