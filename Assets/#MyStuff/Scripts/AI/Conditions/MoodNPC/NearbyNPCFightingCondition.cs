using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = "NearbyNPCFightingCondition", menuName = "AI Kit/Conditions/Mood/NearbyNPCFightingCondition")]
	public class NearbyNPCFightingCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;

		[SerializeField] private State fightingState;

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
				if (npc.StateMachine.CurrentState == fightingState)
				{
					return true;
				}
			}
			return false;
		}
	}
}