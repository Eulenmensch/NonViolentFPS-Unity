using System;
using System.Collections.Generic;
using CMF;
using NonViolentFPS.Events;
using NonViolentFPS.NPCs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NonViolentFPS.Player
{
	public class EnemyAttachingBehaviour : SerializedMonoBehaviour
	{
		[field: SerializeField] private List<Transform> attachmentPoints { get; set; }
		[SerializeField] private AnimationCurve slowDownCurve;

		private int attachedEnemyCount;
		private AdvancedWalkerController controller;
		private float defaultSpeed;

		private void OnEnable()
		{
			NPCEvents.Instance.OnAttachToPlayer += AttachEnemy;
		}

		private void OnDisable()
		{
			NPCEvents.Instance.OnAttachToPlayer -= AttachEnemy;
		}

		private void Start()
		{
			controller = GetComponent<AdvancedWalkerController>();
			defaultSpeed = controller.movementSpeed;
		}

		private void AttachEnemy(NPC _npc)
		{
			var attachToPlayerComponent = _npc as IAttachToPlayerComponent;
			if (attachToPlayerComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IAttachToPlayerComponent));
				return;
			}

			foreach (var attachmentPoint in attachmentPoints)
			{
				if (attachmentPoint.childCount > 0) continue;
				Instantiate(attachToPlayerComponent.prefabToAttach, attachmentPoint);
				Destroy(_npc.gameObject);
				attachedEnemyCount++;
				SetPlayerSpeed();
				break;
			}
		}

		private void SetPlayerSpeed()
		{
			switch (attachedEnemyCount)
			{
				case 0:
					controller.movementSpeed = defaultSpeed;
					break;
				case 1:
					controller.movementSpeed = defaultSpeed * slowDownCurve.Evaluate(0.667f);
					break;
				case 2:
					controller.movementSpeed = defaultSpeed * slowDownCurve.Evaluate(0.333f);
					break;
				case 3:
					controller.movementSpeed = 0;
					GameEvents.Instance.GameLost();
					break;
			}
		}
	}
}