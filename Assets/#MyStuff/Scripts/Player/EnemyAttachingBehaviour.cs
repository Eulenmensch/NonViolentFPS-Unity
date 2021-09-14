using System;
using System.Collections.Generic;
using CMF;
using NonViolentFPS.Events;
using NonViolentFPS.NPCs;
using NonViolentFPS.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Task = System.Threading.Tasks.Task;

namespace NonViolentFPS.Player
{
	public class EnemyAttachingBehaviour : SerializedMonoBehaviour
	{
		[field: SerializeField] private List<Transform> attachmentPoints { get; set; }
		[SerializeField] private AnimationCurve slowDownCurve;
		[SerializeField] private int requiredShakeCount;
		[SerializeField] private float requiredMouseDistance;
		[SerializeField] private float shakeTimeWindow;
		[SerializeField] private float detachHeightOffset;
		[SerializeField] private float detachLaunchForce;

		private int attachedEnemyCount;
		private List<GameObject> enemyAttachments;
		private List<PrefabWrapper> attachedEnemyWrappers;
		private AdvancedWalkerController controller;
		private float defaultSpeed;
		private Vector2 lastMouseMove;
		private int shakeCounter;
		private bool isShakeCounterRunning;

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
			attachedEnemyWrappers = new List<PrefabWrapper>();
			enemyAttachments = new List<GameObject>();
			controller = GetComponent<AdvancedWalkerController>();
			defaultSpeed = controller.movementSpeed;
		}

		private void Update()
		{
			DetectShake();
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
				if (attachmentPoint.childCount > 0) continue; //skip the attachment point if it's filled
				enemyAttachments.Add(Instantiate(attachToPlayerComponent.PrefabToAttach, attachmentPoint));
				attachedEnemyCount++;
				attachedEnemyWrappers.Add(attachToPlayerComponent.SelfPrefab);
				SetPlayerSpeed();
				break;
			}
		}

		public void DetachEnemies()
		{
			print("shaka shaka");
			foreach (var enemyWrapper in attachedEnemyWrappers)
			{
				var enemy = enemyWrapper.Prefab;
				var playerPosition = transform.position;
				var index = attachedEnemyWrappers.IndexOf(enemyWrapper);

				var attachedPosition = attachmentPoints[index].transform.position;
				var spawnedEnemy = Instantiate(enemy, attachedPosition, Quaternion.identity);
				var launchDirection = (playerPosition - attachedPosition).normalized;
				var launchForce = launchDirection * detachLaunchForce;
				spawnedEnemy.GetComponent<Rigidbody>()?.AddForce(launchForce);
			}

			foreach (var enemyAttachment in enemyAttachments)
			{
				Destroy(enemyAttachment);
			}

			enemyAttachments.Clear();
			attachedEnemyWrappers.Clear();
			attachedEnemyCount = 0;

			SetPlayerSpeed();
		}

		private void SetPlayerSpeed()
		{
			switch (attachedEnemyCount)
			{
				case 0:
					controller.movementSpeed = defaultSpeed;
					break;
				case 1:
					controller.movementSpeed = defaultSpeed * slowDownCurve.Evaluate(0.333f);
					break;
				case 2:
					controller.movementSpeed = defaultSpeed * slowDownCurve.Evaluate(0.667f);
					break;
				case 3:
					controller.movementSpeed = 0;
					GameEvents.Instance.GameLost();
					break;
			}
		}

		private void DetectShake()
		{
			var mouseMove = Mouse.current.delta.ReadValue();
			if (Mathf.Sign(mouseMove.x) != Mathf.Sign(lastMouseMove.x) ||
			    Mathf.Sign(mouseMove.y) != Mathf.Sign(lastMouseMove.y))
			{
				if(Vector2.Distance(mouseMove, lastMouseMove) >= requiredMouseDistance)
				{
					shakeCounter++;
					if (!isShakeCounterRunning)
					{
						ShakeTimer();
					}
				}
			}

			lastMouseMove = mouseMove;

			if (shakeCounter >= requiredShakeCount)
			{
				DetachEnemies();
				shakeCounter = 0;
			}
		}

		private async void ShakeTimer()
		{
			isShakeCounterRunning = true;
			var time = 0f;
			while (time < shakeTimeWindow)
			{
				time += Time.deltaTime;
				await Task.Yield();
			}
			shakeCounter = 0;
			isShakeCounterRunning = false;
		}
	}
}