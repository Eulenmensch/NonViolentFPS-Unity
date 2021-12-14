using System;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.Events
{
	public class NPCEvents : MonoBehaviour
	{
		public static NPCEvents Instance { get; private set; }

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

		public event Action<NPC> OnAttachToPlayer;
		public void AttachToPlayer(NPC _npc){OnAttachToPlayer?.Invoke(_npc);}

		public event Action<NPC> OnDefeated;
		public void Defeated(NPC _npc){OnDefeated?.Invoke(_npc);}
	}
}