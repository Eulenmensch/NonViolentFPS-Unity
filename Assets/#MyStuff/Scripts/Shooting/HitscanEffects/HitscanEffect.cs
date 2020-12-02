using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public interface IHitscanEffect
	{
		void Initialize(RaycastHit _hit);
	}
}

