using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = "ChangeMaterialEnterAction", menuName = "AI Kit/Enter Actions/ChangeMaterialEnterAction")]
	public class ChangeMaterialEnterAction : EnterAction
	{
		[SerializeField] private Material material;

		public override void Enter(NPC _npc)
		{
			var meshRendererComponent = _npc as IMeshRendererComponent;
			if (meshRendererComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IMeshRendererComponent));
				return;
			}

			foreach (var renderer in meshRendererComponent.MeshRenderers)
			{
				renderer.material = material;
			}
		}
	}
}