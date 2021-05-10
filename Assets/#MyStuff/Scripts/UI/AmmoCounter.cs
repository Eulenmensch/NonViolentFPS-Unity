using System;
using NonViolentFPS.Events;
using TMPro;
using UnityEngine;

namespace NonViolentFPS.UI
{
	public class AmmoCounter : MonoBehaviour
	{
		[SerializeField] private TMP_Text ammoCounterText;


		private void OnEnable()
		{
			UIEvents.Instance.OnAmmoTextUpdate += UpdateAmmoCounter;
		}

		private void OnDisable()
		{
			UIEvents.Instance.OnAmmoTextUpdate -= UpdateAmmoCounter;
		}

		private void UpdateAmmoCounter(string _ammoCount)
		{
			ammoCounterText.text = _ammoCount;
		}
	}
}