using System;
using NonViolentFPS.Events;
using TMPro;
using UnityEngine;

namespace NonViolentFPS.UI
{
	public class UpdateTimer : MonoBehaviour
	{
		[SerializeField] private TMP_Text timerText;

		private void OnEnable()
		{
			UIEvents.Instance.OnTimerTextUpdate += UpdateTimerText;
		}

		private void OnDisable()
		{
			UIEvents.Instance.OnTimerTextUpdate -= UpdateTimerText;
		}

		private void UpdateTimerText(string _timerText)
		{
			timerText.text = _timerText;
		}
	}
}