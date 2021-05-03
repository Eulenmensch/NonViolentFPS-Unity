using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	public class AtDefaultLocationCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;

		public override bool Evaluate(NPC _npc)
		{
			var defaultLocationComponent = _npc as IDefaultLocationComponent;
			if (defaultLocationComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IDefaultLocationComponent));
				return false;
			}

			var selfPosition = _npc.transform.position;
			var distanceToDefaultLocation = Vector3.Distance(selfPosition, defaultLocationComponent.DefaultLocation);
			var buffer = Random.Range(defaultLocationComponent.BufferRadiusMin,
				defaultLocationComponent.BufferRadiusMax);

			if (distanceToDefaultLocation <= buffer)
			{
				return true;
			}

			return false;
		}
	}
}