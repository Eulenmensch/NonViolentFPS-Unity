using System;
using System.Collections;
using Cinemachine;
using NonViolentFPS.Events;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class AnimateOnDollyTrack : MonoBehaviour
	{
		[SerializeField] private AnimationCurve positionCurve;
		[SerializeField] private float xRotationMax;
		[SerializeField] private float yRotationMax;
		[SerializeField] private float zRotationMax;
		[SerializeField] private AnimationCurve xRotation;
		[SerializeField] private AnimationCurve yRotation;
		[SerializeField] private AnimationCurve zRotation;
		[SerializeField] private CinemachineDollyCart dollyCart;
		[SerializeField] private float animationTime;

		private Quaternion defaultRotation;

		private void OnEnable()
		{
			PlayerEvents.Instance.OnInteract += StartAnimation;
		}

		private void Start()
		{
			defaultRotation = transform.localRotation;
		}

		private void StartAnimation()
		{
			StartCoroutine(AnimatePosition());
			StartCoroutine(AnimateRotation());
		}

		private IEnumerator AnimatePosition()
		{
			var startTime = Time.time;
			while (Time.time - startTime <= animationTime)
			{
				dollyCart.m_Position = positionCurve.Evaluate((Time.time - startTime) / animationTime);
				yield return null;
			}

			dollyCart.m_Position = 0;
		}

		private IEnumerator AnimateRotation()
		{
			var startTime = Time.time;
			while (Time.time - startTime <= animationTime)
			{
				var defaultEulerRotation = defaultRotation.eulerAngles;

				var eulerRotation = new Vector3();
				var timeProgress = (Time.time - startTime) / animationTime;
				eulerRotation.x = defaultEulerRotation.x + xRotationMax * xRotation.Evaluate(timeProgress);
				eulerRotation.y = defaultEulerRotation.y + yRotationMax * yRotation.Evaluate(timeProgress);
				eulerRotation.z = defaultEulerRotation.z + zRotationMax * zRotation.Evaluate(timeProgress);

				transform.localRotation = Quaternion.Euler(eulerRotation);
				yield return null;
			}

			transform.localRotation = defaultRotation;
		}
	}
}