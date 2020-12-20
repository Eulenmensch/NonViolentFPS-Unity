using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu(menuName = "AI Kit/Conditions/PlayerInTriggerCondition")]
    public class PlayerInTriggerCondition : Condition
    {
        public override bool Evaluate(NPC _npc)
        {
            var triggerComponent = _npc as ITriggerComponent;
            if (triggerComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(ITriggerComponent));
                return false;
            }

            return triggerComponent.Triggered;
        }
    }
}