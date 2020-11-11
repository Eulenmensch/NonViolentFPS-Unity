using UnityEngine;

public class ShootingRangeTestEffect : MonoBehaviour, IHitscanEffect
{
	public void Initialize(RaycastHit _hit) { }

	private void Start()
	{
		var machine = GetComponentInParent<StateMachine>();
		machine.hit = true;
	}
}
