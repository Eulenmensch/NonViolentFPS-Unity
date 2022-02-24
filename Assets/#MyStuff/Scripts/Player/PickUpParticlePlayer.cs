using System;
using NonViolentFPS.Events;
using UnityEngine;

namespace NonViolentFPS.Player
{
	public class PickUpParticlePlayer : MonoBehaviour
	{
		private void OnEnable()
		{
			PlayerEvents.Instance.OnPickUpCollected += PlayParticles;
		}

		private void OnDisable()
		{
			PlayerEvents.Instance.OnPickUpCollected -= PlayParticles;
		}

		private void PlayParticles(GameObject _particles)
		{
			print("pickup");
			var particles = Instantiate(_particles, transform);
			// particles.GetComponent<ParticleSystem>().Play();
		}

	}
}