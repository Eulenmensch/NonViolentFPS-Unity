using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace NonViolentFPS.Scripts.Level
{
	public class HitBoxMMFeedbacksTrigger : MonoBehaviour
	{
		[SerializeField] private MMFeedbacks winFeedbacks;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag.Equals("Player"))
			{
				winFeedbacks.PlayFeedbacks();
			}
		}
	}
}