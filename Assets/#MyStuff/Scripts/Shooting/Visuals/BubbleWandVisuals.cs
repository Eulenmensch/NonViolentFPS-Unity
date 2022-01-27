using MoreMountains.Feedbacks;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class BubbleWandVisuals : GunVisuals
	{
		[field: SerializeField] public Transform GunTarget { get; set; }
	}
}