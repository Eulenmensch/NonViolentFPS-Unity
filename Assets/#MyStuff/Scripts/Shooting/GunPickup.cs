using System;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
	[SerializeField] private MonoBehaviour gun;

	private Shooter playerShooter;
	private bool playerInTrigger;

	private void OnValidate()
	{
		Debug.Assert(gun is IGun, "The component you assigned does not implement IGun.");
	}

	private void OnEnable()
	{
		PlayerEvents.Instance.OnInteract += PickUpGun;
	}
	private void OnDisable()
	{
		PlayerEvents.Instance.OnInteract -= PickUpGun;
	}

	private void Awake()
	{
		playerShooter = GameManager.Instance.Player.GetComponent<Shooter>();
	}

	private void PickUpGun()
	{
		if (playerInTrigger)
		{
			playerShooter.ActiveGun = gun as IGun;
		}
	}

	private void OnTriggerEnter(Collider _other)
	{
		if (_other.gameObject == GameManager.Instance.Player)
		{
			playerInTrigger = true;
		}
	}

	private void OnTriggerExit(Collider _other)
	{
		if (_other.gameObject == GameManager.Instance.Player)
		{
			playerInTrigger = false;
		}
	}
}
