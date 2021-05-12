using NonViolentFPS.Events;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = nameof(AttachToPlayerEnterAction), menuName = "AI Kit/Enter Actions/AttachToPlayerEnterAction")]
	public class AttachToPlayerEnterAction : EnterAction
	{
		public override void Enter(NPC _npc)
		{
			NPCEvents.Instance.AttachToPlayer(_npc);
		}
	}
}