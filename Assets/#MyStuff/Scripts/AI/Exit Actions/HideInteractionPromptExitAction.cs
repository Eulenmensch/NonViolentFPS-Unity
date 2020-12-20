using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Exit Actions/HideInteractionPromptExitAction")]
	public class HideInteractionPromptExitAction : ExitAction
	{
		public override void Exit(NPC _npc)
		{
			var interactionComponent = _npc as IInteractionComponent;
			if (interactionComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IInteractionComponent));
				return;
			}

			interactionComponent.InteractionPrompt.SetActive(false);
		}
	}
}