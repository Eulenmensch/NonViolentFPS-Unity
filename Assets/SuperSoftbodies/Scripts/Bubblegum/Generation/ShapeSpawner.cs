using UnityEngine;

namespace Bubblegum.Generation
{

	/// <summary>
	/// Spawner that creates objects at random points within a shape
	/// </summary>
	public class ShapeSpawner : SpawnerStripped
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// The shape that we want to spawn in
		/// </summary>
		[SerializeField, Tooltip ("The shape that we want to spawn in")]
		private Shape spawnShape;

		#endregion // PUBLIC_VARIABLES

		#region ENUMERATORS

		/// <summary>
		/// The shape of the spawner
		/// </summary>
		private enum Shape { CUBE, SPHERE }

		#endregion // ENUMERATORS

		#region METHODS

		/// <summary>
		/// Draw all gizmo objects
		/// </summary>
		void OnDrawGizmos ()
		{
			Gizmos.color = Color.blue;

			switch (spawnShape)
			{
				case Shape.CUBE:
					Gizmos.DrawWireCube (SpawnPoint.Position, transform.localScale);
					break;

				case Shape.SPHERE:
					Gizmos.DrawWireSphere (SpawnPoint.Position, transform.localScale.x);
					break;
			}
		}

		/// <summary>
		/// Spawn within the selected shape
		/// </summary>
		public override void Spawn ()
		{
			Vector3 defaultPosition = SpawnPoint.Position;

			switch (spawnShape)
			{
				case Shape.CUBE:
					SetSpawnPoint (GetPointInCube ());
					break;

				case Shape.SPHERE:
					SetSpawnPoint (GetPointInSphere ());
					break;
			}

			base.Spawn ();
			SetSpawnPoint (defaultPosition);
		}

		/// <summary>
		/// Get a random point in a cube around the spawn point
		/// </summary>
		/// <returns></returns>
		private Vector3 GetPointInCube()
		{
			Vector3 scale = transform.localScale;
			return SpawnPoint.Position + new Vector3 (Random.Range (-scale.x, scale.x), Random.Range (-scale.y, scale.y), Random.Range (-scale.z, scale.z));
		}

		/// <summary>
		/// Get a random point in sphere around the spawn point
		/// </summary>
		/// <returns></returns>
		private Vector3 GetPointInSphere ()
		{
			return SpawnPoint.Position + Random.insideUnitSphere * transform.localScale.x;
		}

		#endregion // PRIVATE_METHODS
	}
}