using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/GetKnockedBackEnterAction")]
	public class GetKnockedBackEnterAction : EnterAction
	{
		[SerializeField] private float knockBackForce;
		[SerializeField] private float upwardsModifier;
		[SerializeField] private float knockBackOffset;

		public override void Enter(NPC _npc)
		{
			var rigidbodyComponent = _npc as IRigidbodyComponent;
			if (rigidbodyComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IRigidbodyComponent));
				return;
			}

			var playerPosition = _npc.Player.transform.position;
			var position = _npc.transform.position;
			var playerDirection = playerPosition - position;
			var nearbyPositionInPlayerDirection = position + playerDirection.normalized * knockBackOffset;
			rigidbodyComponent.RigidbodyRef.AddExplosionForce(knockBackForce, nearbyPositionInPlayerDirection, knockBackOffset+1f, upwardsModifier);
		}
	}
}
