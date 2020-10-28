using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Bubblegum.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bubblegum.FauxPhysics.Damage
{
	/// <summary>
	/// Sets a mesh up to respond to collisions deforming visually and 
	/// </summary>
	public class Softbody : MonoBehaviour, IForceReceiver, ISaveable
	{
		#region PUBLIC_VARIABLES

		/// <summary>
		/// Get or set the priority of this softbody
		/// </summary>
		public int Priority { get; private set; }

		/// <summary>
		/// If the softbody should react to the physics system collisions
		/// </summary>
		public bool ReactToCollisions { get; set; }

		/// <summary>
		/// If we should debug this object
		/// </summary>
		[SerializeField, Tooltip ("If we should debug this object")]
		private bool debug;

		/// <summary>
		/// Min force to check nodes with
		/// </summary>
		[SerializeField, Tooltip ("Min force to check nodes with")]
		private float minCheckForce = 10f;

		/// <summary>
		/// The channel to apply to be read by shader
		/// </summary>
		[SerializeField, Tooltip ("The channel to apply to be read by shader")]
		private UVChannel damageChannel = UVChannel.UV2;

		/// <summary>
		/// All of the nodes in this body
		/// </summary>
		[SerializeField, ReadOnly, Tooltip ("All of the nodes in this body")]
		private SoftbodyNode[] nodes;

		#endregion

		#region PRIVATE_VARIABLES

		/// <summary>
		/// Key to save the softbody
		/// </summary>
		private const string PREFS_KEY = "SoftbodyState";

		/// <summary>
		/// The current bake message
		/// </summary>
		private string bakeMessage = "Ready";

		/// <summary>
		/// The current baking node
		/// </summary>
		private int bakeIteration;

		/// <summary>
		/// Damper we might have attached
		/// </summary>
		private SoftbodyDamper damper;

		/// <summary>
		/// All of the meshes we are interested in
		/// </summary>
		private List<Mesh> meshes = new List<Mesh> ();

		/// <summary>
		/// All of the nodes sorted by the filter they are targeting
		/// </summary>
		private Dictionary<Mesh, SoftbodyNode[]> sortedNodes = new Dictionary<Mesh, SoftbodyNode[]> ();

		/// <summary>
		/// All of the original vertices
		/// </summary>
		private Dictionary<Mesh, Vector3[]> originalVertices = new Dictionary<Mesh, Vector3[]> ();

		/// <summary>
		/// All of the vertices edited
		/// </summary>
		private Dictionary<Mesh, Vector3[]> editingVertices = new Dictionary<Mesh, Vector3[]> ();

		/// <summary>
		/// All of the damage uv channels
		/// </summary>
		private Dictionary<Mesh, Vector2[]> editingUVs = new Dictionary<Mesh, Vector2[]> ();

		#endregion

		#region METHODS

		/// <summary>
		/// Awaken this object
		/// </summary>
		private void Awake ()
		{
			ReactToCollisions = true;
			damper = GetComponent<SoftbodyDamper> ();

			//Find all meshes
			for (int i = 0; i < nodes.Length; i++)
				if (!meshes.Contains (nodes[i].Mesh))
					CreateMesh (nodes[i]);

			//Sort all nodes and verts
			for (int i = 0; i < meshes.Count; i++)
			{
				sortedNodes.Add (meshes[i], nodes.Where (node => node.Mesh == meshes[i]).ToArray ());
				originalVertices.Add (meshes[i], meshes[i].vertices);
				editingVertices.Add (meshes[i], meshes[i].vertices);

				//Special damage channel
				Vector2[] uv = new Vector2[meshes[i].vertexCount];
				ApplyUV (meshes[i], uv);
				editingUVs.Add (meshes[i], uv);
			}

			SoftbodyJobManager.RegisterJob (this);
		}

		/// <summary>
		/// Destroy this object
		/// </summary>
		private void OnDestroy ()
		{
			SoftbodyJobManager.DeregisterJob (this);
		}

		/// <summary>
		/// Update this object
		/// </summary>
		public void UpdateSoftbody ()
		{
			if (damper)
				damper.UpdateDamper ();

			for (int i = 0; i < nodes.Length; i++)
			{
				nodes[i].UpdateNode ();

				if (nodes[i].RenderDirty)
					ApplyNodeMovements (nodes[i].Mesh);
			}

			Priority = 0;
		}

		/// <summary>
		/// When we collide with another object
		/// </summary>
		/// <param name="collision"></param>
		public void OnCollisionEnter (Collision collision)
		{
			if (ReactToCollisions)
			{
				float force = collision.GetForce ();
				ApplyForce (force, collision.contacts.GetAveragePosition (), collision.impulse.normalized);
			}
		}

		/// <summary>
		/// Apply force to this softbody
		/// </summary>
		/// <param name="force"></param>
		/// <param name="direction"></param>
		public void ApplyForce (float force, Vector3 point, Vector3 direction, bool checkForce = true)
		{
			if (checkForce && force < minCheckForce)
				return;

			for (int i = 0; i < nodes.Length; i++)
				nodes[i].ApplyForce (force, point, direction);

			if (debug)
				print ("Collision event on softbody " + name + " with force " + force);

			IncrementPriority (1);
		}

		/// <summary>
		/// Apply the force to the object
		/// </summary>
		public void ApplyForce (Vector3 position, float force, float radius)
		{
			ApplyForce (force, position, (transform.position - position).normalized);
		}

		/// <summary>
		/// Apply a soft force to our nodes
		/// </summary>
		/// <param name="direction"></param>
		/// <param name="distance"></param>
		public void ApplySoftForce (Vector3 direction, Collider contact, float distance)
		{
			for (int i = 0; i < nodes.Length; i++)
				nodes[i].ApplySoftForce (distance, contact, direction);

			if (debug)
				print ("Collision event on softbody " + name + " with distance " + distance);

			IncrementPriority (1);
		}

		/// <summary>
		/// Apply all node movements
		/// </summary>
		public void ApplyNodeMovements (SoftbodyNode node)
		{
			ApplyNodeMovements (node.Mesh);
		}

		/// <summary>
		/// Bake all of the nodes we can find
		/// </summary>
		public void Bake ()
		{
#if UNITY_EDITOR
			EditorApplication.update += BakeNext;
			nodes = GetComponentsInChildren<SoftbodyNode> ();
			bakeIteration = 0;
			bakeMessage = "Baking...";
#endif
		}

		/// <summary>
		/// Save this softbody
		/// </summary>
		public void Save (Key id)
		{
			Float3[] offsets = nodes.Select (node => new Float3 (node.DamagedPosition)).ToArray ();
			string data = Serialize.SerializeObject (offsets);
			id.Save (PREFS_KEY, data);
		}
		/// <summary>
		/// Clear the state of the object
		/// </summary>
		/// <param name="id"></param>
		public void Clear (Key id)
		{
			id.Clear (PREFS_KEY);
		}

		/// <summary>
		/// Load this softbody
		/// </summary>
		/// <param name="id"></param>
		public void Load (Key id)
		{
			if (PlayerPrefs.HasKey (PREFS_KEY))
			{
				string data = id.Load (PREFS_KEY, "") as string;
				Float3[] offsets = Serialize.DeserializeObject (data, typeof (Float3[])) as Float3[];
				Vector3[] vectorOffsets = offsets.Select (offset => offset.ToVector3 ()).ToArray ();

				for (int i = 0; i < nodes.Length; i++)
					if (vectorOffsets[i] != Vector3.zero)
						nodes[i].SetDamagePosition (vectorOffsets[i], false);
			}
		}

		/// <summary>
		/// Increment the priority of this object
		/// </summary>
		/// <param name="amount"></param>
		public void IncrementPriority (int amount)
		{
			Priority += amount;
			SoftbodyJobManager.Instance.RegisterPriorityJob (this);
		}

		#endregion

		#region PRIVATE_METHODS

		/// <summary>
		/// Apply all node movements
		/// </summary>
		private void ApplyNodeMovements (Mesh mesh)
		{
			Vector3[] vertices = editingVertices[mesh];
			Vector2[] uv = editingUVs[mesh];
			SoftbodyNode[] nodes = sortedNodes[mesh];

			//Apply changes
			for (int i = 0; i < nodes.Length; i++)
			{
				if (nodes[i].RenderDirty)
				{
					nodes[i].SetVertices (vertices);
					nodes[i].SetDamagedTextureInfo (uv);
				}
			}

			mesh.vertices = vertices;
			ApplyUV (mesh, uv);
			mesh.RecalculateBounds ();

			//Hack to recalculate collider bounds
			SoftbodyNode node = nodes[0];

			if (node.IsColliderNode)
			{
				node.MeshCollider.enabled = false;
				node.MeshCollider.enabled = true;
			}
		}

		/// <summary>
		/// Bake the next object
		/// </summary>
		private void BakeNext ()
		{
#if UNITY_EDITOR

			//Stop
			if (Application.isPlaying || bakeIteration >= nodes.Length)
			{
				EditorApplication.update -= BakeNext;
				UnityEditor.EditorUtility.SetDirty (this);
				bakeMessage = "Baking Complete";
				return;
			}

			//Iterate
			try
			{
				nodes[bakeIteration].Bake ();
				bakeIteration++;
			}
			catch (System.Exception ex)
			{
				EditorApplication.update -= BakeNext;
				bakeMessage = "Error Baking - See log for details";
				throw ex;
			}
#endif
		}

		/// <summary>
		/// Create a collider mesh
		/// </summary>
		/// <param name="node"></param>
		private void CreateMesh (SoftbodyNode node)
		{
			Mesh mesh = Instantiate (node.Mesh);
			node.Mesh = mesh;
			mesh.uv2 = new Vector2[mesh.vertexCount];
			meshes.Add (mesh);
			mesh.MarkDynamic ();
		}

		/// <summary>
		/// Apply uv channel data
		/// </summary>
		/// <param name="mesh"></param>
		/// <param name="uv"></param>
		private void ApplyUV (Mesh mesh, Vector2[] uv)
		{
			switch (damageChannel)
			{
				case UVChannel.UV1:
					mesh.uv = uv;
					break;

				case UVChannel.UV2:
					mesh.uv2 = uv;
					break;

				case UVChannel.UV3:
					mesh.uv3 = uv;
					break;

				case UVChannel.UV4:
					mesh.uv4 = uv;
					break;
			}
		}

		#endregion

		#region SUB_CLASSES

#if UNITY_EDITOR

		/// <summary>
		/// Editor script for the softbody component
		/// </summary>
		[CustomEditor (typeof (Softbody))]
		public class SoftbodyEditor : EditorBase
		{
			#region METHODS

			/// <summary>
			/// Draw the unity inspector
			/// </summary>
			public override void OnInspectorGUI ()
			{
				base.OnInspectorGUI ();
				Softbody softbody = (Softbody) target;

				if (GUILayout.Button ("Bake"))
					softbody.Bake ();

				GUILayout.Space (10f);
				GUILayout.Label ("Bake Status:		" + softbody.bakeMessage);
			}

			#endregion
		}

#endif
		#endregion
	}
}