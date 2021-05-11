using System;
using UnityEngine;

namespace NonViolentFPS.Events
{
	public class UIEvents : MonoBehaviour
	{
		public static UIEvents Instance { get; private set; }

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

		public event Action<string> OnSetMaxScoreText;
		public void SetMaxScoreText(string _maxScoreText){OnSetMaxScoreText?.Invoke(_maxScoreText);}

		public event Action<string> OnTimerTextUpdate;
		public void UpdateTimerText(string _timerText){OnTimerTextUpdate?.Invoke(_timerText);}

		public event Action<int> OnAmmoTextUpdate;
		public void UpdateAmmoText(int _ammoCount){OnAmmoTextUpdate?.Invoke(_ammoCount);}
	}
}