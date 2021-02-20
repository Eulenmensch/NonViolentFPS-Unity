using System.Globalization;
using NonViolentFPS.Events;
using UnityEngine;

namespace NonViolentFPS.GameModes
{
	[CreateAssetMenu(fileName = "RescueGameMode", menuName = "GameModes/Rescue")]
	public class RescueMode : GameMode
	{
		private float time;
		public override void Load()
		{
			base.Load();
			time = 0;
			UIEvents.Instance.UpdateTimerText(Mathf.Round(time).ToString(CultureInfo.CurrentCulture));
		}

		public override void Evaluate()
		{
			time += Time.deltaTime;
			UIEvents.Instance.UpdateTimerText(time.ToString("F2").Replace(",", "."));
		}
	}
}