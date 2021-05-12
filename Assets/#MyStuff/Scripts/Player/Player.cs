using System;
using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using UnityEngine;

namespace NonViolentFPS.Player
{
	public class Player : MonoBehaviour
	{
		private void Awake()
		{
			GameManager.Instance.Player = gameObject;
		}

		private void Start()
		{
			GameEvents.Instance.PlayerLoaded(transform);
		}
	}
}
