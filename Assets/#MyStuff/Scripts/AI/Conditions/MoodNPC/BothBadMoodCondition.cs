using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/Mood/BothBadMoodCondition")]
	public class BothBadMoodCondition : Condition
	{
		public override bool Evaluate(NPC _npc)
		{
			var otherNPCsComponent = _npc as IOtherNPCsComponent;
			if (otherNPCsComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IOtherNPCsComponent));
				return false;
			}

			foreach (var npc in otherNPCsComponent.OtherNPCs)
			{
				if (npc.StateMachine.CurrentState == _npc.StateMachine.CurrentState)
				{
					return true;
				}
			}
			return false;
		}
	}
}