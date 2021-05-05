using NonViolentFPS.AI;
using UnityEngine;

namespace NonViolentFPS.NPCs
{
	[CreateAssetMenu( menuName = "AI Kit/Conditions/PlayerInConeUnobstructedCondition" )]
	public class PlayerInConeUnobstructedCondition : Condition
	{
		[SerializeField] private float coneAngle;
		[SerializeField] private float coneLength;

		public override UpdateType type => UpdateType.Physics;
		public override bool Evaluate(NPC _npc)
		{
			var selfPosition = _npc.transform.position;
			var playerHalfHeight = _npc.Player.GetComponent<CapsuleCollider>().height / 2;
			var playerPosition = _npc.Player.transform.position + Vector3.up * playerHalfHeight;
			var playerDirection = (playerPosition - selfPosition).normalized;
			var playerDistance = Vector3.Distance(playerPosition, selfPosition);

			var playerWithinAngle = GetPlayerWithinAngle(_npc);
			var playerUnobstructedAndInRange = GetPlayerUnobstructedAndInRange(_npc, selfPosition, playerDirection);

			return playerWithinAngle && playerUnobstructedAndInRange;
		}

		private bool GetPlayerUnobstructedAndInRange(NPC _npc, Vector3 _selfPosition, Vector3 _playerDirection)
		{
			var rayToPlayer = new Ray(_selfPosition, _playerDirection);
			if (!UnityEngine.Physics.Raycast(rayToPlayer, out var hit, coneLength)) return false;
			return hit.collider.gameObject == _npc.Player;
		}

		private bool GetPlayerWithinAngle(NPC _npc)
		{
			var angle = GetAngleToPlayer(_npc);
			return angle <= coneAngle;
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