using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Events
{
	public class PlayerEvents : MonoBehaviour
	{
		public static PlayerEvents Instance { get; private set; }

		private void Awake()
		{
			if ( Instance != null && Instance != this )
			{
				Destroy( this );
			}
			else
			{
				Instance = this;
			}
		}

		public event Action OnInteract;

		//TODO: This should honestly be a regular event call with input logic being
		//TODO: handled in the controller
		public void Interact(InputAction.CallbackContext _context)
		{
			if (_context.started)
			{
				OnInteract?.Invoke();
			}
		}

		public event Action<int> OnAmmoChanged;
		public void ChangeAmmo(int _currentAmmo){ OnAmmoChanged?.Invoke(_currentAmmo);}

		public event Action<int> OnGunChanged;
		public void UpdateGunStats(int _newGunAmmoCount){OnGunChanged?.Invoke(_newGunAmmoCount);}

		public event Action<bool> OnAimDownSights;
		public void AimDownSights(bool _isAimingDownSights){OnAimDownSights?.Invoke(_isAimingDownSights);}

		public event Action<float> OnReload;
		public void Reload(float _reloadTime){OnReload?.Invoke(_reloadTime);}

		public event Action OnReloadCompleted;
		public void ReloadCompleted(){OnReloadCompleted?.Invoke();}

		public event Action<GameObject> OnPickUpCollected;
		public void PickUpCollected(GameObject _particles){OnPickUpCollected?.Invoke(_particles);}
	}
}
