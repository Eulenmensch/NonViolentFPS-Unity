using NonViolentFPS.Events;
using UnityEngine;
using UnityEngine.UI;

namespace NonViolentFPS.UI
{
	public class AmmoSlider : MonoBehaviour
	{
		[SerializeField] private Slider ammoSlider;

		private int currentGunAmmoTypeCount;

		private void OnEnable()
		{
			PlayerEvents.Instance.OnAmmoChanged += UpdateAmmoSliderPosition;
			PlayerEvents.Instance.OnGunChanged += UpdateCurrentGunAmmoTypeCount;
		}

		private void OnDisable()
		{
			PlayerEvents.Instance.OnAmmoChanged -= UpdateAmmoSliderPosition;
			PlayerEvents.Instance.OnGunChanged -= UpdateCurrentGunAmmoTypeCount;
		}

		private void UpdateAmmoSliderPosition(int _currentAmmo)
		{
			ammoSlider.value = (float)_currentAmmo / currentGunAmmoTypeCount;
		}

		private void UpdateCurrentGunAmmoTypeCount(int _newGunAmmoCount)
		{
			currentGunAmmoTypeCount = _newGunAmmoCount - 1;
		}
	}
}
