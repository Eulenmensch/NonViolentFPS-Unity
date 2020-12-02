using NonViolentFPS.Events;
using UnityEngine;
using UnityEngine.UI;

namespace NonViolentFPS.UI
{
	public class AmmoSlider : MonoBehaviour
	{
		[SerializeField] private Slider ammoSlider;

		private int currentGunAmmoCount;

		private void OnEnable()
		{
			PlayerEvents.Instance.OnAmmoChanged += UpdateAmmoSliderPosition;
			PlayerEvents.Instance.OnGunChanged += UpdateCurrentGunAmmoCount;
		}

		private void OnDisable()
		{
			PlayerEvents.Instance.OnAmmoChanged -= UpdateAmmoSliderPosition;
			PlayerEvents.Instance.OnGunChanged -= UpdateCurrentGunAmmoCount;
		}

		private void UpdateAmmoSliderPosition(int _currentAmmo)
		{
			ammoSlider.value = (float)_currentAmmo / currentGunAmmoCount;
		}

		private void UpdateCurrentGunAmmoCount(int _newGunAmmoCount)
		{
			currentGunAmmoCount = _newGunAmmoCount - 1;
		}
	}
}
