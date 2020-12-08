using UnityEngine;
using UnityEngine.AI;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/ActivateRigidbodyEnterAction")]
	public class ActivateRigidbodyEnterAction : EnterAction
	{
		public override void Enter(StateMachine _stateMachine)
		{
			var machine = _stateMachine as RigidbodyStateMachine;
			if (EnableRigidbody(machine)) return;

			DisableNavMeshAgent(_stateMachine);
		}

		private static bool EnableRigidbody(RigidbodyStateMachine _machine)
		{
			if (_machine == null) return true;
			_machine.RigidbodyRef.isKinematic = false;
			return false;
		}

		private static void DisableNavMeshAgent(StateMachine _machine)
		{
			var navMeshAgent = _machine.GetComponent<NavMeshAgent>();
			if (navMeshAgent == null) return;
			navMeshAgent.enabled = false;
		}
	}
}
