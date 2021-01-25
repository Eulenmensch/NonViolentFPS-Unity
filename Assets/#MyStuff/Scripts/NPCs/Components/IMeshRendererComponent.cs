using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IMeshRendererComponent
	{
		MeshRenderer[] MeshRenderers { get; set; }
	}
}