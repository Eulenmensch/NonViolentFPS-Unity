using NonViolentFPS.Shooting;
using UnityEngine;

public class BubbleFreezer : MonoBehaviour
{
	[SerializeField] private float unfreezeTime;

	private void OnTriggerEnter(Collider other)
	{
		var enclosingProjectile = other.GetComponent<EnclosingProjectile>();
		if (enclosingProjectile == null) {return;}
		enclosingProjectile.Freeze();
	}

	private void OnTriggerExit(Collider other)
	{
		var enclosingProjectile = other.GetComponent<EnclosingProjectile>();
		if (enclosingProjectile == null) {return;}
		enclosingProjectile.Unfreeze(unfreezeTime);
	}
}
