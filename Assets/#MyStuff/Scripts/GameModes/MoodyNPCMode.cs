using NonViolentFPS.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

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

		private float time;
		private int activeUnitsInFights;
		private bool won;

		private void Update()
		{
			time += Time.deltaTime;
			if (time >= gameDuration && !won)
			{
				GameManager.Instance.SetGameWon();
				won = true;
			}
		}

		public void AddUnitInFight()
		{
			activeUnitsInFights++;
			if (activeUnitsInFights >= maxUnitsInFights)
			{
				GameManager.Instance.SetGameLost();
			}
		}

		public void RemoveUnitInFight()
		{
			activeUnitsInFights--;
		}
	}
}