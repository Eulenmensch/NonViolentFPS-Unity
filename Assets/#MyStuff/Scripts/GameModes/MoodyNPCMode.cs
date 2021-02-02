using System;
using NonViolentFPS.Manager;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace NonViolentFPS.GameModes
{
	public class MoodyNPCMode : SerializedMonoBehaviour
	{
		#region Singleton
		public static MoodyNPCMode Instance { get; private set; }

		private void Awake()
		{
			if ( Instance != null && Instance != this )
			{
				Destroy( this );
			}
			else
			{
				Instance = this;
			}
		}
		#endregion

		[SerializeField] private int maxUnitsInFights;
		[SerializeField] private float gameDuration;
		[SerializeField] private TMP_Text maxUnitsInFightsText;
		[SerializeField] private TMP_Text activeUnitsInFightsText;
		[SerializeField] private TMP_Text timeText;

		private float time;
		private int activeUnitsInFights;
		private bool won;

		private void Start()
		{
			maxUnitsInFightsText.text = maxUnitsInFights.ToString();
			time = gameDuration;
			timeText.text = Mathf.Round(time).ToString();
		}

		private void Update()
		{
			time -= Time.deltaTime;
			if (time >= gameDuration && !won)
			{
				GameManager.Instance.SetGameWon();
				won = true;
			}
			timeText.text = Mathf.Round(time).ToString();
		}

		public void AddUnitInFight()
		{
			activeUnitsInFights++;
			print(activeUnitsInFights);
			if (activeUnitsInFights >= maxUnitsInFights)
			{
				GameManager.Instance.SetGameLost();
				won = false;
			}
			activeUnitsInFightsText.text = activeUnitsInFights.ToString();
		}

		public void RemoveUnitInFight()
		{
			activeUnitsInFights--;
			activeUnitsInFightsText.text = activeUnitsInFights.ToString();
		}
	}
}