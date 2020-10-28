using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bubblegum
{

	/// <summary>
	/// Spawns objects automatically using pooling
	/// </summary>
	public class SpawnerStripped : MonoBehaviour
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// The prefab that you want to set
		/// </summary>
		public GameObject Prefab { get { return prefabToSpawn; } set { prefabToSpawn = value; } }

		/// <summary>
		/// Gets or sets the spawn position.
		/// </summary>
		/// <value>The spawn position.</value>
		public Target SpawnPoint { get; set; }

		/// <summary>
		/// Gets the spawn rotation.
		/// </summary>
		/// <value>The spawn rotation.</value>
		public Quaternion SpawnRotation
		{
			get
			{
				return spawnPoint.rotation;
			}
		}

		/// <summary>
		/// If we should loop our spawning
		/// </summary>
		public bool LoopSpawning { get; set; }

		/// <summary>
		/// The delay between each spawn
		/// </summary>
		public float SpawnRate { get; set; }

		/// <summary>
		/// The event for when we spawn an object
		/// </summary>
		public Action<GameObject> onSpawn;

		/// <summary>
		/// If we should spawn on awake
		/// </summary>
		public bool spawnOnAwake;

		/// <summary>
		/// The prefab to spawn.
		/// </summary>
		[Tooltip ("The prefab to spawn.")]
		[SerializeField]
		protected GameObject prefabToSpawn;

		/// <summary>
		/// The spawn point to create objects, uses this objects transform as default
		/// </summary>
		[Tooltip ("The spawn point to create objects, uses this objects transform as default")]
		[SerializeField]
		protected Transform spawnPoint;

		/// <summary>
		/// If we should be responsible for destroying objects we created
		/// </summary>
		[SerializeField, Tooltip ("If we should be responsible for destroying objects we created")]
		private bool selfCleanup;

		/// <summary>
		/// If the prefab is poolable then we will try pooling it
		/// </summary>
		[SerializeField, Tooltip ("If the prefab is poolable then we will try pooling it")]
		private bool shouldPool;

		/// <summary>
		/// The parent to the spawned objects
		/// </summary>
		[Tooltip ("The parent to the spawned objects")]
		[SerializeField]
		private Transform spawnParent;

		/// <summary>
		/// If the new object should use its own rotation or the spawn points
		/// </summary>
		[Tooltip ("If the new object should use its own rotation or the spawn points")]
		[SerializeField]
		private bool useSpawnPointRotation;

		/// <summary>
		/// If we should spawn objects at the given spawn point or their default position
		/// </summary>
		[Tooltip ("If we should spawn objects at the given spawn point or their default position")]
		[SerializeField]
		private bool useSpawnPointPosition = true;

		/// <summary>
		/// If we should scale objects
		/// </summary>
		[Tooltip ("If we should scale objects")]
		[SerializeField]
		private bool useSpawnPointScale;

		#endregion // PUBLIC_VARIABLES

		#region PRIVATE_VARIABLES

		/// <summary>
		/// The spawn delay offset.
		/// </summary>
		private float spawnDelayOffset;

		/// <summary>
		/// Objects we are responsible for cleaning up
		/// </summary>
		private List<Component> cleanupObjects = new List<Component> ();

		#endregion // PRIVATE_VARIABLES

		#region MONOBEHAVIOUR_METHODS

		/// <summary>
		/// Start this behaviour
		/// </summary>
		protected virtual void Awake ()
		{
			if (!spawnPoint)
				spawnPoint = transform;

			SpawnPoint = new Target (spawnPoint);
			SetSpawnObject (prefabToSpawn);

			if (spawnOnAwake)
				Spawn ();
		}

		/// <summary>
		/// Validate inspector input
		/// </summary>
		private void OnValidate ()
		{
			if (!spawnPoint)
				spawnPoint = transform;

			SpawnPoint = new Target (spawnPoint);
		}

		/// <summary>
		/// Destroy this object
		/// </summary>
		private void OnDestroy ()
		{
			if (selfCleanup)
				for (int i = 0; i < cleanupObjects.Count; i++)
					if (cleanupObjects[i])
					{
						Destroy (cleanupObjects[i].gameObject);
					}
		}

		#endregion // MONOBEHAVIOUR_METHODS

		#region PUBLIC_METHODS

		/// <summary>
		/// Sets the spawn point.
		/// </summary>
		/// <param name="transform">Transform.</param>
		public void SetSpawnPoint (Transform transform)
		{
			SpawnPoint = new Target (transform);
		}

		/// <summary>
		/// Sets the spawn position.
		/// </summary>
		/// <param name="position">Position.</param>
		public void SetSpawnPoint (Vector3 position)
		{
			SpawnPoint = new Target (position);
		}

		/// <summary>
		/// Spawn an object at the given position
		/// </summary>
		/// <param name="position"></param>
		public void SpawnAtPoint (Transform position)
		{
			SetSpawnPoint (position);
			Spawn ();
		}

		/// <summary>
		/// Spawn an object at the given position
		/// </summary>
		/// <param name="position"></param>
		public void SpawnAtPoint (Vector3 position)
		{
			SetSpawnPoint (position);
			Spawn ();
		}

		/// <summary>
		/// Spawns at positions.
		/// </summary>
		/// <param name="positions">Positions.</param>
		/// <param name="delay">Delay.</param>
		public void SpawnAtPositions (Vector3[] positions, float delay = 0f)
		{
			if (delay != 0f)
				StartCoroutine (SpawnAtPostionsWithDelay (positions, delay));
			else
			{
				for (int i = 0; i < positions.Length; i++)
				{
					SpawnPoint.Set (positions[i]);
					Spawn ();
				}
			}
		}

		/// <summary>
		/// Invoke the spawn method after a few seconds
		/// </summary>
		/// <param name="seconds">Seconds.</param>
		public void SpawnAfterSeconds (float seconds)
		{
			Invoke ("Spawn", seconds);
		}

		/// <summary>
		/// Spawns the object, returns true if somehting was actually spawned (obstacles could prevent spawn happening)
		/// </summary>
		public virtual void Spawn ()
		{
			Quaternion rotation = useSpawnPointRotation ? SpawnRotation : prefabToSpawn.transform.rotation;
			Vector3 position = useSpawnPointPosition ? SpawnPoint.Position : prefabToSpawn.transform.position;
			Vector3 scale = useSpawnPointScale ? SpawnPoint.Transform.localScale : prefabToSpawn.transform.localScale;

			//If use spawn position was from UI then correct to world space
			if (useSpawnPointPosition && transform is RectTransform && !(prefabToSpawn.transform is RectTransform) && !IsRenderMode (RenderMode.ScreenSpaceCamera))
			{
				Camera camera = Camera.main;
				position = camera.ScreenToWorldPoint (position - new Vector3 (0, 0, camera.transform.position.z));
			}

			GameObject obj = Instantiate (prefabToSpawn, position, rotation, spawnParent) as GameObject;
			obj.transform.localScale = scale;

			if (selfCleanup)
				cleanupObjects.Add (obj.transform);

			if (onSpawn != null)
				onSpawn (obj);

			if (LoopSpawning)
				Invoke ("Spawn", SpawnRate);
		}

		/// <summary>
		/// Set the new spawn object then spawn
		/// </summary>
		/// <param name="component"></param>
		public void Spawn (GameObject obj)
		{
			SetSpawnObject (obj);
			Spawn ();
		}

		/// <summary>
		/// Spawn the given amount of times
		/// </summary>
		/// <param name="amount"></param>
		public void Spawn (int amount)
		{
			for (int i = 0; i < amount; i++)
				Spawn ();
		}

		/// <summary>
		/// Maybe spawn depending on random chance
		/// </summary>
		public void RandomSpawn (int spawnChance)
		{
			if (UnityEngine.Random.Range (0, spawnChance) == 0)
				Spawn ();
		}

		/// <summary>
		/// Maybe spawn depending on random chance
		/// </summary>
		public void RandomSpawn (int spawnChance, System.Random random)
		{
			if (random.Next (0, spawnChance) == 0)
				Spawn ();
		}

		/// <summary>
		/// Set the prefab to spawn
		/// </summary>
		/// <param name="prefab"></param>
		private void SetSpawnObject (GameObject prefab)
		{
			prefabToSpawn = prefab;
		}

		/// <summary>
		/// When the context is changed we update the object to spawn
		/// </summary>
		/// <param name="obj"></param>
		public void ContextChanged (object obj)
		{
			if (obj is GameObject)
				prefabToSpawn = (GameObject)obj;
		}

		#endregion // PUBLIC_METHODS

		#region PRIVATE_METHODS

		/// <summary>
		/// Spawns at postions with delay.
		/// </summary>
		/// <returns>The at postions with delay.</returns>
		/// <param name="positions">Positions.</param>
		/// <param name="delay">Delay.</param>
		private IEnumerator SpawnAtPostionsWithDelay (Vector3[] positions, float delay)
		{
			for (int i = 0; i < positions.Length; i++)
			{
				SpawnPoint.Set (positions[i]);
				Spawn ();

				yield return new WaitForSeconds (delay);
			}
		}

		/// <summary>
		/// Check whether Spawner is using particular render mode
		/// </summary>
		/// <param name="renderMode"></param>
		/// <returns></returns>
		private bool IsRenderMode (RenderMode renderMode)
		{
			if (transform is RectTransform)
				return GetComponentInParent<Canvas> ().renderMode == renderMode;
			else
				return false;
		}

		#endregion // PRIVATE_METHODS
	}
}