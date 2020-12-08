using MoreMountains.Feedbacks;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class GunVisuals : MonoBehaviour
	{
		[field: SerializeField] public Transform ShootingOrigin { get; set; }
		[field: SerializeField] public MMFeedbacks FireFeedback { get; set; }
		[field: SerializeField] public MMFeedbacks ChargeFeedback { get; set; }
 	}
}