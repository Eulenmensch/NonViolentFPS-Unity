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
			var playerPosition = _npc.Player.transform.position;
			var rayToPlayer = new Ray(selfPosition, playerPosition);

			// var rayArray = new Ray[9];
			// foreach (var ray in rayArray)
			// {
			//
			// }


			if (UnityEngine.Physics.Raycast(rayToPlayer, out var hit ,rangeComponent.Range))
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