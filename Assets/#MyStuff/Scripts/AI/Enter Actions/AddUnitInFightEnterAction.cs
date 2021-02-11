using NonViolentFPS.Events;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = "AddUnitInFightEnterAction", menuName = "AI Kit/Enter Actions/AddUnitInFightEnterAction")]
	public class AddUnitInFightEnterAction : EnterAction
	{
		public override void Enter(NPC _npc)
		{
			GameEvents.Instance.ChangeScore(1);
		}
	}
}