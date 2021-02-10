using System;
using System.Globalization;
using NonViolentFPS.Events;
using UnityEngine;
using TMPro;

namespace NonViolentFPS.GameModes
{
	[CreateAssetMenu(fileName = "MoodGameMode", menuName = "GameModes/Mood")]
	public class MoodyNPCMode : GameMode
	{
		[SerializeField] private int maxUnitsInFights;
		[SerializeField] private float gameDuration;
		[SerializeField] private TMP_Text maxUnitsInFightsText;
		[SerializeField] private TMP_Text timeText;

		private float time;
		// private int activeUnitsInFights;
		private bool won;

		public override void Load()
		{
			base.Load();
			maxUnitsInFightsText.text = maxUnitsInFights.ToString();
			time = gameDuration;
			timeText.text = Mathf.Round(time).ToString(CultureInfo.CurrentCulture);
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
			if (time >= gameDuration && !won)
			{
				GameEvents.Instance.GameWon();
				won = true;
			}

			timeText.text = Mathf.Round(time).ToString(CultureInfo.CurrentCulture);
		}

		private void EvaluateScore()
		{
			if (Score >= maxUnitsInFights)
			{
				GameEvents.Instance.GameLost();
			}
		}
	}
}