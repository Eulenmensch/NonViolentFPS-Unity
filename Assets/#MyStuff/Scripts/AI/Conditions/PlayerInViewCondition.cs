using NonViolentFPS.NPCs;
using Sirenix.Serialization;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu( menuName = "AI Kit/Conditions/PlayerInViewCondition" )]
	public class PlayerInViewCondition : Condition
	{
		public override UpdateType type => UpdateType.Physics;

		[SerializeField] private float raycastArraySpacing;

		public override bool Evaluate(NPC _npc)
		{
			var viewComponent = _npc as IViewConeComponent;
			if (viewComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IViewConeComponent));
				return false;
			}

			var selfPosition = _npc.transform.position;

			var playerHalfHeight = _npc.Player.GetComponent<CapsuleCollider>().height / 2;
			var playerPosition = _npc.Player.transform.position + Vector3.up * playerHalfHeight;
			var playerDirection = (playerPosition - selfPosition).normalized;
			var rayToPlayer = new Ray(selfPosition, playerDirection);
			Debug.DrawRay(selfPosition, playerDirection, Color.green);
			// var rayArray = new Ray[9];
			// foreach (var ray in rayArray)
			// {
			//
			// }


			if (UnityEngine.Physics.Raycast(rayToPlayer, out var hit ,viewComponent.ViewRange))
			{
				if (hit.collider.gameObject == _npc.Player)
				{
					return true;
				}
			}

			return false;
		}
	}
}