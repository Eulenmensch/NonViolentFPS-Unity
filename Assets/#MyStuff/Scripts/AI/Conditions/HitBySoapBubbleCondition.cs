﻿using NonViolentFPS.NPCs;
using NonViolentFPS.Shooting;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/HitBySoapBubbleCondition")]
	public class HitBySoapBubbleCondition : Condition
	{
		public override UpdateType type => UpdateType.Physics;

		public override bool Evaluate(NPC _npc)
		{
			foreach (var collision in _npc.ActiveCollisions)
			{
				var enclosingProjectile = collision.gameObject.GetComponent<EnclosingProjectile>();
				if (enclosingProjectile != null && enclosingProjectile.AttachedTarget == _npc.transform)
				{
					return true;
				}
			}

			if (_npc.GetComponentInChildren<EnclosingProjectile>())
			{
				return true;
			}
			
			return false;
		}
	}
}