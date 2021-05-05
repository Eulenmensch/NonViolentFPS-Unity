using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/PlayerInAlertConeCondition")]
	public class PlayerInDetectionConeCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;

		public override bool Evaluate(NPC _npc)
		{
			var alertConeComponent = _npc as IAlertConeComponent;
			if (alertConeComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IAlertConeComponent));
				return false;
			}

			var angle = GetAngleToPlayer(_npc);
			var distanceToPlayer = Vector3.Distance(_npc.Player.transform.position, _npc.transform.position);
			if (angle <= alertConeComponent.AlertAngle && distanceToPlayer <= alertConeComponent.AlertRange)
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