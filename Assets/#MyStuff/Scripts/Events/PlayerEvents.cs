using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
}
