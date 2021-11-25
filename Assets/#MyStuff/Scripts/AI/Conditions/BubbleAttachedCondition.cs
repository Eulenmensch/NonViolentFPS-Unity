using NonViolentFPS.AI;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.Scripts.AI.Conditions
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/BubbleAttachedCondition")]
	public class BubbleAttachedCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;
		public override bool Evaluate(NPC _npc)
		{
			var bubbleComponent = _npc as IBubbleComponent;
			if (bubbleComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IBubbleComponent));
				return false;
			}

			if (bubbleComponent.AttachedBubble != null)
			{
				return true;
			}

			return false;
		}
	}
}