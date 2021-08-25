using NonViolentFPS.Utility;
using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IAttachToPlayerComponent
	{
		GameObject PrefabToAttach { get; set; }
		PrefabWrapper SelfPrefab { get; set; }
	}
}