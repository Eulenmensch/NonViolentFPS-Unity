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
			PlayerEvents.Instance.OnReload += StartAnimation;
		}

		private void OnDisable()
		{
			PlayerEvents.Instance.OnReload -= StartAnimation;
		}

		private void Start()
		{
			defaultRotation = transform.localRotation;
		}

		private void StartAnimation(float _animationTime)
		{
			StartCoroutine(Animate(_animationTime));
		}

		private IEnumerator Animate(float _animationTime)
		{
			var startTime = Time.time;
			while (Time.time - startTime <= _animationTime)
			{
				var timerProgress = (Time.time - startTime) / _animationTime;
				AnimateRotation(timerProgress);
				AnimatePosition(timerProgress);
				yield return null;
			}
			ResetAnimatedProperties();
		}

		private void AnimateRotation(float _timerProgress)
		{
			var defaultEulerRotation = defaultRotation.eulerAngles;
			var eulerRotation = new Vector3();

			eulerRotation.x = defaultEulerRotation.x + xRotationMax * xRotation.Evaluate(_timerProgress);
			eulerRotation.y = defaultEulerRotation.y + yRotationMax * yRotation.Evaluate(_timerProgress);
			eulerRotation.z = defaultEulerRotation.z + zRotationMax * zRotation.Evaluate(_timerProgress);

			transform.localRotation = Quaternion.Euler(eulerRotation);
		}

		private void AnimatePosition(float _timerProgress)
		{
			dollyCart.m_Position = positionCurve.Evaluate(_timerProgress);
		}

		private void ResetAnimatedProperties()
		{
			dollyCart.m_Position = 0;
			transform.localRotation = defaultRotation;
		}
	}
}