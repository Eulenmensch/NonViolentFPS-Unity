using NonViolentFPS.Events;
using UnityEngine;

namespace NonViolentFPS.Level
{
	public class HitBoxSetWon : MonoBehaviour
	{
		public void SetWon()
		{
			GameEvents.Instance.GameWon();
		}
	}
}