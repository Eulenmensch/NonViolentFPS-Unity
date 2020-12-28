using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/PlayFeedbackEnterAction")]
	public class PlayFeedbackEnterAction : EnterAction
	{
		public override void Enter(NPC _npc)
		{
			var feedbacksComponent = _npc as IFeedbacksComponent;
			if (feedbacksComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IFeedbacksComponent));
				return;
			}

			feedbacksComponent.MMFeedbacks.PlayFeedbacks();
		}
	}
}
