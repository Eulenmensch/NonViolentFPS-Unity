using NonViolentFPS.NPCs;
using Sirenix.Serialization;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu( menuName = "AI Kit/Conditions/RayToPlayerHitCondition" )]
	public class RayToPlayerHitCondition : Condition
	{
		public override UpdateType type => UpdateType.Physics;

		[SerializeField] private float raycastArraySpacing;

		public override bool Evaluate(NPC _npc)
		{
			var rangeComponent = _npc as IRangeComponent;
			if (rangeComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IRangeComponent));
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


			if (UnityEngine.Physics.Raycast(rayToPlayer, out var hit ,rangeComponent.Range))
			{
				Debug.Log(hit.collider.name);
				if (hit.collider.gameObject == _npc.Player)
				{
					return true;
				}
			}

			return false;
		}
	}
}