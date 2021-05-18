using System.Threading.Tasks;
using UnityEngine;

namespace NonViolentFPS.Utility
{
	[RequireComponent(typeof(ParticleSystem))]
	public class DestroyParticles : MonoBehaviour
	{
		private ParticleSystem particles;
		private void Start()
		{
			particles = GetComponent<ParticleSystem>();
			DestroyAfterSeconds();
		}

		private async void DestroyAfterSeconds()
		{
			var time = Time.time;
			while (Time.time - time <= 5)
			{
				await Task.Yield();
			}
			Destroy(gameObject);
		}
	}
}