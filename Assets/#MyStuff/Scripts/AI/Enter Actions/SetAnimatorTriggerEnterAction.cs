using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/SetAnimatorTriggerEnterAction")]
	public class SetAnimatorTriggerEnterAction : EnterAction
	{
		[SerializeField] private string parameterName;

		public override void Enter(NPC _npc)
		{
			var animatorComponent = _npc as IAnimatorComponent;
			if (animatorComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IAnimatorComponent));
				return;
			}

			animatorComponent.Animator.SetTrigger(parameterName);
		}
	}
}