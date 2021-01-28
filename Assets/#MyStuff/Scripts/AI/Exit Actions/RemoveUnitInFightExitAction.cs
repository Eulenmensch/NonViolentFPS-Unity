using NonViolentFPS.GameModes;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = "RemoveUnitInFightExitAction", menuName = "AI Kit/Exit Actions/RemoveUnitInFightExitAction")]
	public class RemoveUnitInFightExitAction : ExitAction
	{
		public override void Exit(NPC _npc)
		{
			MoodyNPCMode.Instance.RemoveUnitInFight();
		}
	}
}