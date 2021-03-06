using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using UnityEngine;

//TODO: this class currently sets the shooter's gun to the instance's assigned gun.
//		it should later add the gun to the shooter's gun list instead.

namespace NonViolentFPS.Shooting
{
	public class GunPickup : MonoBehaviour
	{
		[SerializeField] private Gun gun;
		[SerializeField] private GameObject interactionPromptText;

		private ShooterCopy playerShooter;
		private bool playerInTrigger;

		private void OnValidate()
		{
			Debug.Assert(gun is Gun, "The object you assigned does not implement IGun.");
		}

		private void OnEnable()
		{
			PlayerEvents.Instance.OnInteract += PickUpGun;
		}
		private void OnDisable()
		{
			PlayerEvents.Instance.OnInteract -= PickUpGun;
		}

		private void Start()
		{
			playerShooter = GameManager.Instance.Player.GetComponent<ShooterCopy>();
		}

		private void PickUpGun()
		{
			if (playerInTrigger)
			{
				playerShooter.ActivateGun(gun);
			}
		}

		private void OnTriggerEnter(Collider _other)
		{
			if (_other.gameObject == GameManager.Instance.Player)
			{
				playerInTrigger = true;
				interactionPromptText.SetActive(true);
			}
		}

		private void OnTriggerExit(Collider _other)
		{
			if (_other.gameObject == GameManager.Instance.Player)
			{
				playerInTrigger = false;
				interactionPromptText.SetActive(false);
			}
		}
	}
}
