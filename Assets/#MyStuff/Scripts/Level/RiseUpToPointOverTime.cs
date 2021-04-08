using UnityEngine;

namespace NonViolentFPS.Level
{
	public class RiseUpToPointOverTime : MonoBehaviour
	{
		[SerializeField] private float endHeight;
		[SerializeField] private float risingSpeed;


		private void Update()
		{
			if (transform.position.y < endHeight)
			{
				transform.position += Time.deltaTime * risingSpeed * Vector3.up;
			}
		}
	}
}