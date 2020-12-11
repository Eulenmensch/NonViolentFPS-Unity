using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class GunVisuals : MonoBehaviour
	{
		[field: SerializeField] public Transform ShootingOriginOverride { get; set; }
		[field: SerializeField] public MMFeedbacks FireFeedback { get; set; }
		[ShowInInspector] public MMFeedbacks SecondaryFireFeedback { get; set; }
		[field: SerializeField] public MMFeedbacks ChargeFeedback { get; set; }
 	}
}