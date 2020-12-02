using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/GetKnockedBackEnterAction")]
	public class GetKnockedBackEnterAction : EnterAction
	{
		[SerializeField] private float knockBackForce;
		[SerializeField] private float upwardsModifier;
		[SerializeField] private float knockBackOffset;

		public override void Enter(StateMachine _stateMachine)
		{
			var machine = _stateMachine as RigidbodyStateMachine;

			var playerPosition = machine.Player.transform.position;
			var playerDirection = playerPosition - machine.transform.position;
			var nearbyPositionInPlayerDirection = machine.transform.position + playerDirection.normalized * knockBackOffset;
			machine.RigidbodyRef.AddExplosionForce(knockBackForce, nearbyPositionInPlayerDirection, knockBackOffset+1f, upwardsModifier);
		}
	}
}
