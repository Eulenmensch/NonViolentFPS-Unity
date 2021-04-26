using System;
using DG.Tweening;
using NonViolentFPS.Events;
using UnityEngine;
using UnityEngine.UI;

namespace NonViolentFPS.UI
{
	public class CrosshairToggle : MonoBehaviour
	{
		[SerializeField] private Image crosshairImage;
		[SerializeField] private float fadeDuration;

		private Color defaultColor;

		private void OnEnable()
		{
			PlayerEvents.Instance.OnAimDownSights += ToggleCrosshair;
		}

		private void OnDisable()
		{
			PlayerEvents.Instance.OnAimDownSights -= ToggleCrosshair;
		}

		private void Start()
		{
			defaultColor = crosshairImage.color;
		}

		private void ToggleCrosshair(bool _isAimingDownSights)
		{
			switch (_isAimingDownSights)
			{
				case true:
					crosshairImage.DOColor(Color.clear, fadeDuration);
					break;
				case false:
					crosshairImage.DOColor(defaultColor, fadeDuration);
					break;
			}
		}
	}
}