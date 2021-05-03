using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Behaviours/CachePlayerLocationBehaviour")]
	public class CachePlayerLocationBehaviour : AIBehaviour
	{
		public override UpdateType type => UpdateType.Regular;

		public override void DoBehaviour(NPC _npc)
		{
			var chaseComponent = _npc as IChaseComponent;
			if (chaseComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IChaseComponent));
				return;
			}

			chaseComponent.LastKnownPlayerLocation = _npc.Player.transform.position;
		}
	}
}