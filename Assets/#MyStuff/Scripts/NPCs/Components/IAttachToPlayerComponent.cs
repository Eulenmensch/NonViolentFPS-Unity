using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IAttachToPlayerComponent
	{
		GameObject prefabToAttach { get; set; }
	}
}