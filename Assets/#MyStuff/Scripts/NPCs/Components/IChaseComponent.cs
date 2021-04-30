using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IChaseComponent
	{
		Vector3 LastKnownPlayerLocation { get; set; }
	}
}