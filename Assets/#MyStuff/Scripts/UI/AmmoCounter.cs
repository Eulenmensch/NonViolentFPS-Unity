using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using NonViolentFPS.Shooting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NonViolentFPS.UI
{
	public class AmmoCounter : MonoBehaviour
	{
		[SerializeField] private TMP_Text ammoCounterText;
		[SerializeField] private Image outlineImage;
		[SerializeField] private Sprite[] outlines;

		private int clipSize;

		private void OnEnable()
		{
			UIEvents.Instance.OnAmmoTextUpdate += UpdateAmmoCounter;
			UIEvents.Instance.OnAmmoTextUpdate += UpdateAmmoCounterOutline;
		}

		private void OnDisable()
		{
			UIEvents.Instance.OnAmmoTextUpdate -= UpdateAmmoCounter;
			UIEvents.Instance.OnAmmoTextUpdate -= UpdateAmmoCounterOutline;
		}

		private void Start()
		{
			var ammoClipComponent = GameManager.Instance.Player.GetComponent<ShooterCopy>().ActiveGun as IAmmoClipComponent;
			if (ammoClipComponent == null)
			{
				Debug.LogError("the active gun does not have an ammo clip");
				return;
			}
			clipSize = ammoClipComponent.ClipSize;
			UIEvents.Instance.UpdateAmmoText(clipSize);
		}

		private void UpdateAmmoCounter(int _ammoCount)
		{
			ammoCounterText.text = _ammoCount.ToString();
		}

		private void UpdateAmmoCounterOutline(int _ammoCount)
		{
			var ammoPercent = (float)_ammoCount / clipSize;
			if (ammoPercent >= 0.34f)
			{
				outlineImage.sprite = outlines[0];
			}
			else if (ammoPercent > 0)
			{
				outlineImage.sprite = outlines[1];
			}
			else if (ammoPercent == 0)
			{
				outlineImage.sprite = outlines[2];
			}
		}
	}
}