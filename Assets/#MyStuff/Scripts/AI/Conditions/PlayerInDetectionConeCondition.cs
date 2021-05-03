using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/PlayerInDetectionConeCondition")]
	public class PlayerInDetectionConeCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;

		[SerializeField] private float coneAngle;

		public override bool Evaluate(NPC _npc)
		{
			var angle = GetAngleToPlayer(_npc);
			if (angle <= coneAngle)
			{
				return true;
			}

			return false;
		}

		private float CosToEuler(float _cosValue)
		{
			var radians = Mathf.Acos(_cosValue);
			return Mathf.Rad2Deg * radians;
		}

		private float GetAngleToPlayer(NPC _npc)
		{
			var playerPosition = _npc.Player.transform.position;
			var npcTransform = _npc.transform;
			var selfPosition = npcTransform.position;
			var playerDirection = (playerPosition - selfPosition).normalized;
			var selfForward = npcTransform.forward;
			var dot = Vector3.Dot(playerDirection, selfForward);
			var angleToPlayer = CosToEuler(dot);

			return angleToPlayer;
		}
	}
}