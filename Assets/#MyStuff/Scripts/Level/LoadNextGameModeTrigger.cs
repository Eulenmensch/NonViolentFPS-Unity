using NonViolentFPS.GameModes;
using NonViolentFPS.Manager;
using UnityEngine;

namespace NonViolentFPS.Level
{
	public class LoadNextGameModeTrigger : MonoBehaviour
	{
		[SerializeField] private GameMode nextGameMode;
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject == GameManager.Instance.Player)
			{
				GameManager.Instance.LoadNewGameMode(nextGameMode);
			}
		}
	}
}