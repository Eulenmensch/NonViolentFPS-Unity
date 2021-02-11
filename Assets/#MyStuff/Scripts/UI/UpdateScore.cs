using System;
using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using TMPro;
using UnityEngine;

namespace NonViolentFPS.UI
{
	public class UpdateScore : MonoBehaviour
	{
		[SerializeField] private TMP_Text scoreText;
		[SerializeField] private TMP_Text maxScoreText;

		private void OnEnable()
		{
			UIEvents.Instance.OnSetMaxScoreText += SetMaxScoreText;
			GameEvents.Instance.OnScoreChanged += UpdateScoreText;
		}

		private void OnDisable()
		{
			UIEvents.Instance.OnSetMaxScoreText -= SetMaxScoreText;
			GameEvents.Instance.OnScoreChanged -= UpdateScoreText;
		}

		private void UpdateScoreText(int _scoreChange)
		{
			scoreText.text = GameManager.Instance.CurrentGameMode.Score.ToString();
		}

		private void SetMaxScoreText(string _maxScoreText)
		{
			maxScoreText.text = _maxScoreText;
		}
	}
}