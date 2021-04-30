using NonViolentFPS.NPCs;
using UnityEngine.AI;

namespace NonViolentFPS.AI
{
	public class LocationReachedCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;

		public override bool Evaluate(NPC _npc)
		{
			var navMeshComponent = _npc as INavMeshMoveComponent;
			if (navMeshComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(INavMeshMoveComponent));
				return false;
			}

			if (navMeshComponent.Agent.pathStatus == NavMeshPathStatus.PathComplete)
			{
				return true;
			}

			return false;
		}
	}
}