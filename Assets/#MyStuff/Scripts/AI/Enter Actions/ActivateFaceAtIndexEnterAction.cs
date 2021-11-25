using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = "ActivateFaceAtIndexEnterAction", menuName = "AI Kit/Enter Actions/ActivateFaceAtIndexEnterAction")]
	public class ActivateFaceAtIndexEnterAction : EnterAction
	{
		[SerializeField] private int faceIndex;

		public override void Enter(NPC _npc)
		{
			var facesComponent = _npc as IFacesComponent;
			if (facesComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IFacesComponent));
				return;
			}

			var faces = facesComponent.Faces;

			foreach (var face in faces)
			{
				if (faces.IndexOf(face) == faceIndex)
				{
					face.SetActive(true);
				}
				else
				{
					face.SetActive(false);
				}
			}
		}
	}
}