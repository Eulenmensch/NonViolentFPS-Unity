using NonViolentFPS.Physics;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	[CreateAssetMenu(menuName = "Guns/BalloonGun")]
	public class BalloonGun : Gun, IPrimaryFireable
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

		public void PrimaryFireEnter()
		{
			throw new System.NotImplementedException();
		}

		public void PrimaryFireAction()
		{
			throw new System.NotImplementedException();
		}

		public void PrimaryFireExit()
		{
			throw new System.NotImplementedException();
		}
	}
}