using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/StartTimerEnterAction")]
	public class StartTimerEnterAction : EnterAction
	{
		[SerializeField] private float seconds;
		public override void Enter(NPC _npc)
		{
			// _npc.StartCoroutine(_npc.WaitForSeconds(seconds)); FIXME: probably redundant, see fixme in waitfortimercondition
		}
	}
}
