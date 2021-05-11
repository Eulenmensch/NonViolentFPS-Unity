using System;
using System.Collections.Generic;
using NonViolentFPS.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
	public class AmmoCountSoapFilmFeedback : SerializedMonoBehaviour
	{
		[SerializeField] private Material soapFilmMaterial;
		[DictionaryDrawerSettings(KeyLabel = "ammoCount", ValueLabel = "Posterize Power")]
		public Dictionary<int, float> ammoCountPosterizationDictionary;

		private float defaultOpacity;

		private void OnEnable()
		{
			UIEvents.Instance.OnAmmoTextUpdate += UpdateSoapFilmFeedback;
		}

		private void OnDisable()
		{
			UIEvents.Instance.OnAmmoTextUpdate -= UpdateSoapFilmFeedback;
		}

		private void Start()
		{
			defaultOpacity = soapFilmMaterial.GetFloat("_Opacity");
		}

		private void UpdateSoapFilmFeedback(int _ammoCount)
		{
			var posterizePower = ammoCountPosterizationDictionary[_ammoCount];
			soapFilmMaterial.SetFloat("_PosterizePower", posterizePower);
			if (_ammoCount == 0)
			{
				soapFilmMaterial.SetFloat("_Opacity", 0);
			}
			else
			{
				soapFilmMaterial.SetFloat("_Opacity", defaultOpacity);
			}
		}
	}
}