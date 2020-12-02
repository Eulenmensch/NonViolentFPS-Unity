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

		public void Interact(InputAction.CallbackContext _context)
		{
			if (_context.started)
			{
				OnInteract?.Invoke();
			}
		}

		#region Shooting

		public event Action<int> OnAmmoChanged;
		public void ChangeAmmo(int _currentAmmo){ OnAmmoChanged?.Invoke(_currentAmmo);}

		public event Action<int> OnGunChanged;
		public void UpdateGunStats(int _newGunAmmoCount){OnGunChanged?.Invoke(_newGunAmmoCount);}


		#endregion
	}
}
