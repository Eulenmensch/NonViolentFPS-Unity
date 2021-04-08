using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using UnityEngine;

namespace NonViolentFPS.Player
{
	public class Player : MonoBehaviour
	{
		private void Start()
		{
			GameManager.Instance.Player = gameObject;
			GameEvents.Instance.PlayerLoaded(transform);
		}
	}
}
