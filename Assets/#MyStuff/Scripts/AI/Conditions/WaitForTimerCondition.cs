using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/WaitForTimerCondition")]
	public class WaitForTimerCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;

		public override bool Evaluate(NPC _npc)
		{
			return true; //FIXME: not sure if this is still necessary now that I know how to use async methods
			// return !_npc.Waiting;
		}
	}
}
