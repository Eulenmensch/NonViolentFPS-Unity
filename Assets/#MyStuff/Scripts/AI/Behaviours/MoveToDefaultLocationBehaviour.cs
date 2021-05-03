using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Behaviours/MoveToDefaultLocationBehaviour")]
	public class MoveToDefaultLocationBehaviour : AIBehaviour
	{
		public override UpdateType type => UpdateType.Regular;

		public override void DoBehaviour(NPC _npc)
		{
			var defaultLocationComponent = _npc as IDefaultLocationComponent;
			if (defaultLocationComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IDefaultLocationComponent));
				return;
			}

			var navMeshComponent = _npc as INavMeshMoveComponent;
			if (navMeshComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(INavMeshMoveComponent));
				return;
			}

			var distance = Vector3.Distance(navMeshComponent.Agent.destination, defaultLocationComponent.DefaultLocation);

			if (distance >= defaultLocationComponent.BufferRadiusMax)
			{
				navMeshComponent.Agent.SetDestination(defaultLocationComponent.DefaultLocation);
			}
		}
	}
}