using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI Kit/Enter Actions/ActivateRigidbodyEnterAction")]
public class ActivateRigidbodyEnterAction : EnterAction
{
	public override void Enter(StateMachine _stateMachine)
	{
		var machine = _stateMachine as RigidbodyStateMachine;
		machine.RigidbodyRef.isKinematic = false;
		machine.GetComponent<NavMeshAgent>().enabled = false;
	}
}
