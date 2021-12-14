using UnityEngine;

namespace NonViolentFPS.Scripts.Quests
{
	[CreateAssetMenu(menuName = "Quest")]
	public class Quest : ScriptableObject
	{
		public bool Accepted { get; set; }
		public bool Completed { get; set; }
	}
}