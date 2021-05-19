using System;
using NonViolentFPS.Manager;
using NonViolentFPS.Physics;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class EnemyTurret : MonoBehaviour
	{
		[SerializeField] private GameObject projectilePrefab;
		[SerializeField] private Transform shootingOrigin;
		[SerializeField] private Transform projectileContainer;

		[SerializeField] private float arcHeight;
		[SerializeField, Min(0.01f)] private float fireRate;

		private Transform target;
		private float maxArcHeight;
		private float gravity;
		private float timer;

		private void Update()
		{
			if (timer <= 1/fireRate)
			{
				timer += Time.deltaTime;
			}
			else
			{
				FireProjectile();
				timer = 0;
			}
			#if UNITY_EDITOR
			DrawPath();
			#endif
		}

		private void Start()
		{
			target = GameManager.Instance.Player.transform;
			var customGravity = projectilePrefab.GetComponent<CustomGravity>();
			if (customGravity == null)
			{
				gravity = -UnityEngine.Physics.gravity.magnitude;
			}
			else
			{
				gravity = -customGravity.GroundGravity;
			}
		}

		private void FireProjectile()
		{
			var projectile = Instantiate(projectilePrefab, shootingOrigin.position, Quaternion.identity);
			var projectileRigidbody = projectile.GetComponent<Rigidbody>();
			projectile.transform.SetParent(projectileContainer);
			projectileRigidbody.velocity = CalculateLaunchVelocity().launchVelocity;
		}

		private LaunchData CalculateLaunchVelocity()
		{
			var distanceToTarget = target.position - shootingOrigin.position;
			var distanceToTargetXZ = new Vector3(distanceToTarget.x, 0, distanceToTarget.z);
			maxArcHeight = Mathf.Max(target.position.y, shootingOrigin.position.y) + arcHeight;

			var time = Mathf.Sqrt(-2 * maxArcHeight / gravity) + Mathf.Sqrt(2 * (distanceToTarget.y - maxArcHeight) / gravity);
			var launchVelocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * maxArcHeight);
			var launchVelocityXZ = distanceToTargetXZ / time;
			var launchVelocity = launchVelocityXZ + launchVelocityY;

			return new LaunchData(launchVelocity, time);
		}

		private void DrawPath()
		{
			var launchData = CalculateLaunchVelocity();
			var previousDrawPoint = shootingOrigin.position;

			const int resolution = 30;
			for (int i = 0; i < resolution; i++)
			{
				var simulationTime = i / (float) resolution * launchData.timeToTarget;
				var displacemnt = launchData.launchVelocity * simulationTime + Vector3.up * (gravity * simulationTime * simulationTime) / 2f;
				var drawPoint = shootingOrigin.position + displacemnt;
				Debug.DrawLine(previousDrawPoint, drawPoint, Color.white);
				previousDrawPoint = drawPoint;
			}
		}

		private readonly struct LaunchData
		{
			public readonly Vector3 launchVelocity;
			public readonly float timeToTarget;

			public LaunchData(Vector3 _launchVelocity, float _timeToTarget)
			{
				launchVelocity = _launchVelocity;
				timeToTarget = _timeToTarget;
			}
		}
	}
}