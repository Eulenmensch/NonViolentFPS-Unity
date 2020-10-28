using System.Linq;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bubblegum.FauxPhysics.Damage
{

	/// <summary>
	/// A node from the softbody
	/// </summary>
	[System.Serializable]
	public class SoftbodyNode : MonoBehaviour
	{
		#region VARIABLES

		/// <summary>
		/// Get if this node is dirty
		/// </summary>
		public bool RenderDirty { get; private set; }

		/// <summary>
		/// Get if this node is dirty
		/// </summary>
		public bool MoveDirty { get; private set; }

		/// <summary>
		/// Check if this is a collider node
		/// </summary>
		public bool IsColliderNode
		{
			get
			{
				return !meshFilter && meshCollider;
			}
		}

		/// <summary>
		/// Get the damaged position of the node
		/// </summary>
		public Vector3 DamagedPosition { get; private set; }

		/// <summary>
		/// Get the damaged delta position
		/// </summary>
		public Vector3 DeltaDamagedPosition { get; private set; }

		/// <summary>
		/// Get the world damage position
		/// </summary>
		public Vector3 MeshDamagedPosition
		{
			get
			{
				return DamagedPosition + MeshTransform.InverseTransformPoint (transform.position);
			}
		}

		/// <summary>
		/// Get the mesh position
		/// </summary>
		public Vector3 MeshPosition
		{
			get
			{
				return MeshTransform.InverseTransformPoint (transform.position);
			}
		}

		/// <summary>
		/// Get the mesh transform
		/// </summary>
		public Transform MeshTransform
		{
			get
			{
				return meshFilter ? meshFilter.transform : meshCollider.transform;
			}
		}

		/// <summary>
		/// Get the filter we are interested in
		/// </summary>
		public Mesh Mesh
		{
			get
			{
				return meshFilter ? meshFilter.sharedMesh : meshCollider.sharedMesh;
			}

			set
			{
				if (meshFilter)
					meshFilter.mesh = value;
				else
					meshCollider.sharedMesh = value;
			}
		}

		/// <summary>
		/// Get the mesh collider
		/// </summary>
		public MeshCollider MeshCollider
		{
			get
			{
				return meshCollider;
			}
		}

		[Header ("Collection")]

		/// <summary>
		/// The mesh filter to edit
		/// </summary>
		[SerializeField, Tooltip ("The mesh filter to edit"), DisplayIf ("meshCollider", null, ComparisonMode.EQUALS)]
		private MeshFilter meshFilter;

		/// <summary>
		/// The mesh collider to edit
		/// </summary>
		[SerializeField, Tooltip ("The mesh collider to edit"), DisplayIf ("meshFilter", null, ComparisonMode.EQUALS)]
		private MeshCollider meshCollider;

		/// <summary>
		/// The shape to collect vertices with
		/// </summary>
		[SerializeField, Tooltip ("The shape to collect vertices with")]
		private Shape collectionShape = Shape.Sphere;

		/// <summary>
		/// Radius to find vertices for this node
		/// </summary>
		[SerializeField, Tooltip ("Radius to find vertices for this node"), DisplayIf ("collectionShape", Shape.Sphere)]
		private float collectionRadius = 1f;

		/// <summary>
		/// The ease type to use when collecting weights
		/// </summary>
		[SerializeField, Tooltip ("The ease type to use when collecting weights")]
		private AnimationCurve weightCollectionEasing = new AnimationCurve (new Keyframe (0f, 0f), new Keyframe (1f, 1f));

		[Header ("Force Response")]

		/// <summary>
		/// If this node is elastic or only moves in one direction
		/// </summary>
		[SerializeField, Tooltip ("If this node is elastic or only moves in one direction")]
		private bool elastic = true;

		/// <summary>
		/// The strength of this node
		/// </summary>
		[SerializeField, Tooltip ("The strength of this node"), Range (0f, 1f)]
		private float softness = 1f;

		/// <summary>
		/// The amount this node bounces back after collision
		/// </summary>
		[SerializeField, Tooltip ("The amount this node bounces back after collision"), Range (0f, 20f)]
		private float spring = 0f;

		/// <summary>
		/// The max distance we respond to impacts
		/// </summary>
		[SerializeField, Tooltip ("The max distance we respond to impacts")]
		private float responseDistance = 1f;

		/// <summary>
		/// The max amount that we can move this node
		/// </summary>
		[SerializeField, Tooltip ("The max amount that we can move this node")]
		private float maxMovement = 1f;

		/// <summary>
		/// Force that creates the maximum damage
		/// </summary>
		[SerializeField, Tooltip ("Force that creates the maximum damage")]
		private float maxResponseForce = 100f;

		/// <summary>
		/// The max angle that we will respond to when a force is applied
		/// </summary>
		[SerializeField, Tooltip ("The max angle that we will respond to when a force is applied"), Range (1, 100)]
		private int maxResponseAngle = 65;
		
		/// <summary>
		/// If we want to set information in the shader
		/// </summary>
		[SerializeField, Tooltip ("If we want to set information in the shader")]
		private bool updateMaterialDamage;

		/// <summary>
		/// The damage tex to edit
		/// </summary>
		[SerializeField, Tooltip ("The damage tex to edit"), DisplayIf ("updateMaterialDamage", true)]
		private Texture2D damageTex;

		[Header ("Connections")]

		/// <summary>
		/// Weight to apply when moving connections
		/// </summary>
		[SerializeField, Tooltip ("Weight to apply when moving connections"), Range (0f, 1f)]
		private float connectionWeight = 0.5f;

		/// <summary>
		/// Nodes that we are connected to and move relative to this node
		/// </summary>
		[SerializeField, Tooltip ("Nodes that we are connected to and move relative to this node")]
		private SoftbodyNode[] connections = new SoftbodyNode[0];

		/// <summary>
		/// Objects that need to move exactly with the node
		/// </summary>
		[SerializeField, Tooltip ("Objects that need to move exactly with the node")]
		private Transform[] fixedConnections = new Transform[0];

		/// <summary>
		/// All of the vertices that belong to this node
		/// </summary>
		[SerializeField, HideInInspector]
		private List<int> vertexIndexes = new List<int> ();

		/// <summary>
		/// The weights for each vertex
		/// </summary>
		[SerializeField, HideInInspector]
		private List<float> weights = new List<float> ();

		/// <summary>
		/// All of the connection directions
		/// </summary>
		private Vector3[] connectionDirections;

		/// <summary>
		/// The damaged vertex movements that we want to add
		/// </summary>
		private Vector3[] damagedMeshVertexMovements;

		/// <summary>
		/// All of the default fixed positions
		/// </summary>
		private Vector3[] fixedDefaultPositions;

		/// <summary>
		/// Damage uv indexes
		/// </summary>
		private Vector2Int[] damageUVs;

		/// <summary>
		/// All of the damage colors
		/// </summary>
		private Color[] damageColors;

		/// <summary>
		/// The squared collection radius
		/// </summary>
		private float sqrResponseDistance;

		/// <summary>
		/// The current compression of this node
		/// </summary>
		private float compression;

		/// <summary>
		/// The compression we need to add
		/// </summary>
		private float deltaCompression;

		/// <summary>
		/// Distance spring doesnt bother working
		/// </summary>
		private const float SPRING_KILL_DISTANCE = 0.001f;

		/// <summary>
		/// When overlapping we dont want to check the angle
		/// </summary>
		private const float ANGLE_IGNORE_DISTANCE = 0.01f;
			
		#endregion

		#region ENUMS

		/// <summary>
		/// The different shapes we can collect with
		/// </summary>
		public enum Shape { Sphere, Box }

		#endregion

		#region METHODS

		/// <summary>
		/// Awaken this object
		/// </summary>
		private void Awake ()
		{
			sqrResponseDistance = responseDistance * responseDistance;
			damagedMeshVertexMovements = new Vector3[vertexIndexes.Count];
			connectionDirections = connections.Select (connection => (connection.MeshPosition - MeshPosition).normalized).ToArray ();
			fixedDefaultPositions = fixedConnections.Select (connection => MeshTransform.InverseTransformPoint (connection.position)).ToArray ();

			InitializeDamage ();
		}

		/// <summary>
		/// Draw this node
		/// </summary>
		private void OnDrawGizmos ()
		{
#if UNITY_EDITOR
			if (!Selection.transforms.Contains (transform) && !Selection.transforms.Contains (transform.root))
				return;

			if (!meshCollider && !meshFilter)
				return;

			//Draw Node
			Gizmos.color = Color.blue;
			Gizmos.matrix = MeshTransform.localToWorldMatrix;
			Gizmos.DrawSphere (MeshDamagedPosition, 0.1f / MeshTransform.localScale.magnitude);
			Gizmos.DrawLine (MeshPosition, MeshPosition + MeshTransform.InverseTransformDirection (transform.forward) * maxMovement);

			//Draw more information
			if (UnityEditor.Selection.transforms.Contains (transform))
			{
				//Shape
				switch (collectionShape)
				{
					case Shape.Sphere:
						Gizmos.DrawWireSphere (MeshPosition, collectionRadius);
						break;

					case Shape.Box:
						Gizmos.DrawWireCube (MeshPosition, transform.localScale * 2f);
						break;
				}

				//Draw verts
				Vector3[] verts = Mesh.vertices;
				Vector3 size = Vector3.one * 0.05f / MeshTransform.localScale.magnitude;

				for (int i = 0; i < vertexIndexes.Count; i++)
				{
					Gizmos.color = Color.Lerp (Color.blue, Color.red, weights[i]);
					Gizmos.DrawCube (verts[vertexIndexes[i]], size);
				}

				//Draw connections
				Gizmos.color = Color.yellow;

				for (int i = 0; i < connections.Length; i++)
				{
					Gizmos.DrawLine (MeshDamagedPosition, connections[i].MeshDamagedPosition);
					Gizmos.DrawWireSphere (connections[i].MeshDamagedPosition, 0.11f / MeshTransform.localScale.magnitude);
				}
			}
#endif
		}

		/// <summary>
		/// Bake verticies into this node
		/// </summary>
		/// <param name="body"></param>
		public void Bake ()
		{
			//Init
			Vector3[] vertices = Mesh.vertices;
			int[] triangles = Mesh.triangles;
			vertexIndexes.Clear ();
			weights.Clear ();

			//Find vertices in range
			for (int i = 0; i < vertices.Length; i++)
			{
				if (InShape (vertices[i]))
				{
					float distance = Vector3.Distance (vertices[i], transform.localPosition);
					float radius = collectionShape == Shape.Box ? transform.localScale.magnitude : collectionRadius;
					float weight = weightCollectionEasing.Evaluate ((radius - distance) / radius);
					vertexIndexes.Add (i);
					weights.Add (weight);
				}
			}

#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty (this);
#endif
		}

		/// <summary>
		/// Initialize the damage texture methods
		/// </summary>
		/// <param name="tex"></param>
		public void InitializeDamage ()
		{
			if (updateMaterialDamage)
			{
				Vector2[] meshUVs = Mesh.uv;
				damageUVs = new Vector2Int[damagedMeshVertexMovements.Length];
				damageColors = new Color[damagedMeshVertexMovements.Length];

				for (int i = 0; i < vertexIndexes.Count; i++)
				{
					Vector2 uv = meshUVs[vertexIndexes[i]] * new Vector2 (damageTex.width, damageTex.height);
					Vector2Int damageUV = new Vector2Int ((int) uv.x, (int) uv.y);
					damageColors[i] = damageTex.GetPixel (damageUV.x, damageUV.y);
					damageUVs[i] = damageUV;
					damageTex.SetPixel (damageUV.x, damageUV.y, Color.clear);
				}
			}
		}

		/// <summary>
		/// Apply spring force
		/// </summary>
		public void UpdateNode ()
		{
			if (MoveDirty)
				ApplyDamagedMovement ();
			else
				Spring ();
		}

		/// <summary>
		/// Spring this node position
		/// </summary>
		public void Spring ()
		{
			if (spring != 0f && DamagedPosition.sqrMagnitude > SPRING_KILL_DISTANCE)
			{
				Vector3 oldPosition = DamagedPosition;
				DamagedPosition = Vector3.Lerp (DamagedPosition, Vector3.zero, Time.deltaTime * spring);
				DeltaDamagedPosition += DamagedPosition - oldPosition;
				MoveDirty = true;
			}
		}

		/// <summary>
		/// Move this node to the given position
		/// </summary>
		/// <param name="compressionPercent"></param>
		public void Move (float compressionPercent)
		{
			SetDamagePosition (compressionPercent * maxMovement * MeshTransform.InverseTransformDirection (-transform.forward), false);
			GetComponentInParent<Softbody> ().IncrementPriority (1);
		}

		/// <summary>
		/// Apply a soft movement force to the node
		/// </summary>
		/// <param name="distance"></param>
		/// <param name="direction"></param>
		public void ApplySoftForce (float distance, Collider contact, Vector3 direction)
		{
			float angle = Vector3.Angle (-transform.forward, direction);
			float sqrDistance = Vector3.SqrMagnitude (transform.position - contact.ClosestPoint (transform.position));

			if (angle < maxResponseAngle && sqrDistance < sqrResponseDistance)
				SetDamagePosition (MeshTransform.InverseTransformDirection (direction) * distance, false);
		}

		/// <summary>
		/// Apply the given force to this node
		/// </summary>
		public void ApplyForce (float force, Vector3 point, Vector3 forceDirection)
		{
			float angle = Vector3.Angle (-transform.forward, forceDirection);
			float sqrDistance = Vector3.SqrMagnitude (point - transform.position);

			if (angle < maxResponseAngle && sqrDistance < sqrResponseDistance)
			{
				float angleMultiplier = (maxResponseAngle - angle) / maxResponseAngle;
				float forceMultiplier = Mathf.Clamp01 (force / maxResponseForce) * softness;
				float distanceMultiplier = (sqrResponseDistance - sqrDistance) / sqrResponseDistance;

				ApplyForce (MeshTransform.InverseTransformDirection (forceDirection) * angleMultiplier * forceMultiplier * distanceMultiplier * maxMovement);
			}
		}

		/// <summary>
		/// Apply the node movement
		/// </summary>
		/// <param name="vertices"></param>
		public void SetVertices (Vector3[] vertices)
		{
			for (int i = 0; i < vertexIndexes.Count; i++)
			{
				vertices[vertexIndexes[i]] += damagedMeshVertexMovements[i];
				damagedMeshVertexMovements[i] = Vector3.zero;
			}

			RenderDirty = false;
		}

		/// <summary>
		/// Apply damaged texture information into the material
		/// </summary>
		public void SetDamagedTextureInfo (Vector2[] uvs)
		{
			for (int i = 0; i < vertexIndexes.Count; i++)
				uvs[vertexIndexes[i]].x += deltaCompression * weights[i];

			RenderDirty = false;
		}

		/// <summary>
		/// Set the damaged position
		/// </summary>
		/// <param name="movement"></param>
		public void SetDamagePosition (Vector3 movement, bool additive)
		{
			ApplyMovement (movement, additive);

			//Must be here to avoid infinite loop
			for (int i = 0; i < connections.Length; i++)
				if (connections[i].gameObject.activeSelf)
					connections[i].ApplyConnectionMovement (Vector3.Project (movement, connectionDirections[i]) * connectionWeight, additive);
		}

		/// <summary>
		/// Apply the given force to this node
		/// </summary>
		private void ApplyForce (Vector3 meshLocalForce)
		{
			//We need to clamp movement AND damaged position so connections respond correctly
			Vector3 movement = Mathf.Clamp (meshLocalForce.magnitude, 0f, maxMovement) * meshLocalForce.normalized;
			SetDamagePosition (movement, true);
		}

		/// <summary>
		/// Apply a movement to this node
		/// </summary>
		private void ApplyMovement (Vector3 movement, bool additive)
		{
			Vector3 oldPosition = DamagedPosition;
			float oldCompression = compression;

			DamagedPosition = additive ? DamagedPosition + movement : movement;
			compression = elastic ? Mathf.Clamp (DamagedPosition.magnitude, 0f, maxMovement) : Mathf.Clamp (DamagedPosition.magnitude, compression, maxMovement);
			deltaCompression = compression - oldCompression;
			DamagedPosition = compression * DamagedPosition.normalized;
			DeltaDamagedPosition += DamagedPosition - oldPosition;

			MoveDirty = true;
		}

		/// <summary>
		/// Apply movement to this node as a connection
		/// </summary>
		/// <param name="movement"></param>
		private void ApplyConnectionMovement (Vector3 movement, bool additive)
		{
			movement = movement.magnitude * MeshTransform.InverseTransformDirection (transform.forward);
			ApplyMovement (movement, additive);
		}

		/// <summary>
		/// Apply the damaged movement
		/// </summary>
		private void ApplyDamagedMovement ()
		{
			for (int i = 0; i < damagedMeshVertexMovements.Length; i++)
				damagedMeshVertexMovements[i] += DeltaDamagedPosition * weights[i];

			for (int i = 0; i < fixedConnections.Length; i++)
				fixedConnections[i].position = MeshTransform.TransformPoint (fixedDefaultPositions[i] + DamagedPosition);

			MoveDirty = false;
			RenderDirty = true;
			DeltaDamagedPosition = Vector3.zero;
		}

		/// <summary>
		/// Check the point is inside our shape
		/// </summary>
		/// <param name="point"></param>
		private bool InShape (Vector3 vertex)
		{
			switch (collectionShape)
			{
				case Shape.Sphere:
					return Vector3.Distance (vertex, transform.localPosition) < collectionRadius;

				case Shape.Box:
					Vector3 meshPos = MeshPosition;
					Vector3 collectionScale = transform.localScale;

					return vertex.x > meshPos.x - collectionScale.x && vertex.y > meshPos.y - collectionScale.y && vertex.z > meshPos.z - collectionScale.z &&
						vertex.x < meshPos.x + collectionScale.x && vertex.y < meshPos.y + collectionScale.y && vertex.z < meshPos.z + collectionScale.z;
			}

			return false;
		}

		#endregion

		#region SUB_CLASSES

		#if UNITY_EDITOR

		/// <summary>
		/// Editor script for the softbody node
		/// </summary>
		[CustomEditor (typeof (SoftbodyNode)), CanEditMultipleObjects]
		public class SoftbodyNodeEditor : EditorBase
		{
			#region METHODS

			private float scale = 0.5f;

			/// <summary>
			/// Draw scene objects
			/// </summary>
			protected virtual void OnSceneGUI ()
			{
				if (!Application.isPlaying)
					return;

				SoftbodyNode node = (SoftbodyNode) target;

				float size = HandleUtility.GetHandleSize (node.transform.position) * 2f;

				EditorGUI.BeginChangeCheck ();
				scale = Handles.ScaleSlider (scale, node.transform.position, node.transform.forward, node.transform.rotation, size, 0.1f);
				scale = Mathf.Clamp01 (scale);

				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Change Scale Value");
					node.Move (scale * 2f - 1f);
				}
			}

			#endregion
		}

#endif
		#endregion
	}
}