using UnityEngine;

namespace NonViolentFPS.Utility
{
	[CreateAssetMenu(fileName = "PrefabWrapper", menuName = "Utility/Prefab Wrapper", order = 0)]
	public class PrefabWrapper : ScriptableObject
	{
		[field: SerializeField] public GameObject Prefab { get; set; }
	}
}