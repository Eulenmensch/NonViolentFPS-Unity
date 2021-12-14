using NonViolentFPS.Events;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = "SetEnemyDefeatedEnterAction", menuName = "AI Kit/Enter Actions/SetEnemyDefeatedEnterAction")]
	public class SetEnemyDefeatedEnterAction : EnterAction
	{
		public override void Enter(NPC _npc)
		{
			var defeatableComponent = _npc as IDefeatableComponent;
			if (defeatableComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IDefeatableComponent));
				return;
			}

			defeatableComponent.Defeated = true;
			NPCEvents.Instance.Defeated(_npc);
		}
	}
}