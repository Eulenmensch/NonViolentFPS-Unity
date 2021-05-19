using NonViolentFPS.NPCs;
using UnityEngine;
using UnityEngine.AI;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/OnNavMeshCondition")]
	public class OnNavMeshCondition : Condition
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

			var maxSearchRadius = navMeshComponent.Agent.height *2;
			return NavMesh.SamplePosition(_npc.transform.position, out var hit, maxSearchRadius, NavMesh.AllAreas);
		}
	}
}