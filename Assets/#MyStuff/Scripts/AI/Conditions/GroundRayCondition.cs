using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/GroundRayCondition")]
	public class GroundRayCondition : Condition
	{
		[SerializeField] private LayerMask groundLayerMask;

		public override UpdateType type => UpdateType.Physics;

		public override bool Evaluate(NPC _npc)
		{
			var groundRayComponent = _npc as IGroundRayComponent;
			if (groundRayComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IGroundRayComponent));
				return false;
			}

			var grounded = UnityEngine.Physics.Raycast(_npc.transform.position, Vector3.down, groundRayComponent.GroundRayLength, groundLayerMask);
			return grounded;
		}
	}
}