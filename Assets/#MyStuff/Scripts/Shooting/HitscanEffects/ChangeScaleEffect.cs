using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class ChangeScaleEffect : MonoBehaviour, IHitscanEffect
	{
		[SerializeField] private float changeAmount;

		public void Initialize(RaycastHit _hit)
		{
			ChangeScale(changeAmount);
		}

		public void Destroy()
		{
			ChangeScale(-changeAmount);
		}

		private void ChangeScale(float _changeAmount)
		{
			var parentTransform = transform.parent;
			if(parentTransform.localScale.magnitude >= changeAmount)
			{
				parentTransform.localScale += Vector3.one * _changeAmount;
			}
			// else if(parentTransform.localScale.magnitude <)
		}
	}
}