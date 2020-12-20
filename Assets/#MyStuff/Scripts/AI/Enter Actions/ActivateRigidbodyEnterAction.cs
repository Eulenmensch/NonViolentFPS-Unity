using NonViolentFPS.Scripts.NPCs;
using UnityEngine;
using UnityEngine.AI;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/ActivateRigidbodyEnterAction")]
	public class ActivateRigidbodyEnterAction : EnterAction
	{
		public override void Enter(NPC _npc)
		{
			var rigidbodyComponent = _npc as IRigidbodyComponent;
			if (rigidbodyComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IRigidbodyComponent));
				return;
			}

			var navmeshMoveComponent = _npc as INavMeshMoveComponent;

			if (EnableRigidbody(rigidbodyComponent)) return;

			DisableNavMeshAgent(navmeshMoveComponent);
		}

		private static bool EnableRigidbody(IRigidbodyComponent _rigidbodyComponent)
		{
			if (_rigidbodyComponent == null) return true;
			_rigidbodyComponent.RigidbodyRef.isKinematic = false;
			return false;
		}

		private static void DisableNavMeshAgent(INavMeshMoveComponent _navMeshMoveComponent)
		{
			var navMeshAgent = _navMeshMoveComponent.Agent;
			if (navMeshAgent == null) return;
			navMeshAgent.enabled = false;
		}
	}
}
