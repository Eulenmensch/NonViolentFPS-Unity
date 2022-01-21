using NonViolentFPS.Physics;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	[CreateAssetMenu(menuName = "Guns/BalloonGun")]
	public class BalloonGun : ProjectileGun
	{
		private Balloon balloon;
		public override void SetUpGun(ShooterCopy _shooter)
		{
			base.SetUpGun(_shooter);
			balloon = GunInstance.GetComponentInChildren<Balloon>();
			balloon.transform.parent = null;
		}

		public override void CleanUpGun()
		{
			base.CleanUpGun();
			Destroy(balloon.gameObject);
		}
	}
}