using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Exit Actions/SetAnimatorBoolExitAction")]
	public class SetAnimatorBoolExitAction : ExitAction
	{
		[SerializeField] private string parameterName;
		[SerializeField] private bool setValue;

		public override void Exit(NPC _npc)
		{
			var animatorComponent = _npc as IAnimatorComponent;
			if (animatorComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IAnimatorComponent));
				return;
			}

			animatorComponent.Animator.SetBool(parameterName, setValue);
		}
	}
}