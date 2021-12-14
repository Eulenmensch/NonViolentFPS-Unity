using System.Collections.Generic;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.Manager
{
	public class NpcManager : MonoBehaviour
	{
		#region Singleton
		public static NpcManager Instance { get; private set; }

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

		// ReSharper disable once InconsistentNaming : Abbreviation
		public HashSet<NPC> NPCs { get; set; } = new HashSet<NPC>();
	}
}