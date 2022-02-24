using System;
using NonViolentFPS.Events;
using UnityEngine;

namespace NonViolentFPS.Scripts.Game_Elements
{
	public class Pickup : MonoBehaviour
	{
		[SerializeField] private GameObject geo;
		[SerializeField] private float speed;
		[SerializeField] private GameObject playerParticles;

		private void Update()
		{
			transform.Rotate(transform.up, speed);
		}

		private void OnCollisionEnter(Collision collision)
		{
			PlayerEvents.Instance.PickUpCollected(playerParticles);
			Destroy(gameObject);
		}
	}
}