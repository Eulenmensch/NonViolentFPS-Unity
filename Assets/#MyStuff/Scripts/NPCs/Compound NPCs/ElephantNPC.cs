using System.Collections.Generic;
using System.Linq;
using NonViolentFPS.AI;
using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using NonViolentFPS.Scripts.Quests;
using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public class ElephantNPC : NPC,
		IDialogueComponent,
		IHeadComponent,
		ILookAtComponent,
		IInteractionComponent,
		ITriggerComponent
	{
		[field: SerializeField] public Transform CanvasAttachmentPoint { get; set; }
		[field: SerializeField] public YarnProgram YarnDialogue { get; set; }
		[field: SerializeField] public string StartNode { get; set; }
		[field: SerializeField] public Transform Head { get; set; }
		[field: SerializeField] public Transform LookAtTarget { get; set; }
		[field: SerializeField] public GameObject InteractionPrompt { get; set; }
		[field: SerializeField] public bool Triggered { get; set; }

		[SerializeField] private Quest questToGive;
		[SerializeField] private List<NPC> grapplers;

		private bool encounterClear;

		//FIXME: This is a hardcoded system for the specific, elephant related quest. This would be handled by a more robust system
		//FIXME: and outside of this class if there are ever more quests

		protected override void Start()
		{
			base.Start();
			questToGive.Completed = false;
			questToGive.Accepted = false;
			DialogueManager.Instance.YarnRunner.AddCommandHandler("accept_quest", AcceptQuest);
			DialogueManager.Instance.YarnRunner.AddFunction("quest_complete",1, parameters => QuestComplete());
			DialogueManager.Instance.YarnRunner.AddFunction("quest_accepted",1, parameters => QuestAccepted());
			DialogueManager.Instance.YarnRunner.AddFunction("encounter_clear", 1, _parameters => EncounterClear());
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			NPCEvents.Instance.OnDefeated += CheckEncounterClear;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			NPCEvents.Instance.OnDefeated -= CheckEncounterClear;
		}

		private void CheckEncounterClear(NPC _grappler)
		{
			if (grapplers.Contains(_grappler))
			{
				grapplers.Remove(_grappler);
			}

			if (grapplers.Count == 0)
			{
				encounterClear = true;
			}
		}

		private bool EncounterClear()
		{
			return encounterClear;
		}

		private bool QuestComplete()
		{
			return questToGive.Completed;
		}

		private bool QuestAccepted()
		{
			return questToGive.Accepted;
		}

		private void AcceptQuest(string[] parameters)
		{
			questToGive.Accepted = true;
		}

		private void CompleteQuest()
		{
			questToGive.Completed = true;
		}
	}
}