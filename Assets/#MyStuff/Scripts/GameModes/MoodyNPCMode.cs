using System.Globalization;
using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using UnityEngine;

namespace NonViolentFPS.GameModes
{
	[CreateAssetMenu(fileName = "MoodGameMode", menuName = "GameModes/Mood")]
	public class MoodyNPCMode : GameMode
	{
		[SerializeField] private int maxUnitsInFights;
		[SerializeField] private float gameDuration;

		private float time;

		public override void Load()
		{
			base.Load();
			UIEvents.Instance.SetMaxScoreText(maxUnitsInFights.ToString());
			time = gameDuration;
			UIEvents.Instance.UpdateTimerText(Mathf.Round(time).ToString(CultureInfo.CurrentCulture));
		}

		public override void Evaluate()
		{
			//This order is important, so that the player doesn't lose at 0.001 seconds remaining
			EvaluateTimer();
			EvaluateScore();
		}

		private void EvaluateTimer()
		{
			time -= Time.deltaTime;
			if (time >= gameDuration && GameManager.Instance.MetaState != GameMetaState.Won)
			{
				GameEvents.Instance.GameWon();
			}

			UIEvents.Instance.UpdateTimerText(Mathf.Round(time).ToString(CultureInfo.CurrentCulture));
		}

		private void EvaluateScore()
		{
			if (Score >= maxUnitsInFights && GameManager.Instance.MetaState != GameMetaState.Lost)
			{
				GameEvents.Instance.GameLost();
			}
		}
	}
}