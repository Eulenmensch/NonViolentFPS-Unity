using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bubblegum.FauxPhysics.Damage
{
	/// <summary>
	/// Schedules softbodies to run tasks
	/// </summary>
	public class SoftbodyJobManager : Singleton<SoftbodyJobManager>
	{
		#region VARIABLES

		/// <summary>
		/// Get the execution priority
		/// </summary>
		public int ExecutePriority { get { return priorityExecute; } }

		/// <summary>
		/// If we should debug the job manager
		/// </summary>
		[SerializeField, Tooltip ("If we should debug the job manager")]
		private bool debug;

		/// <summary>
		/// The max jobs to run each frame
		/// </summary>
		[SerializeField, Tooltip ("The max jobs to run each frame"), Range (1, 10)]
		private int jobsPerFrame = 1;

		/// <summary>
		/// Iterate jobs and execute any with high priority
		/// </summary>
		[SerializeField, Tooltip ("Iterate jobs and execute any with high priority")]
		private bool priorityLoop = true;

		/// <summary>
		/// Execute jobs at priortiy level
		/// </summary>
		[SerializeField, Tooltip ("Execute jobs at priortiy level"), DisplayIf ("priorityLoop", true)]
		private int priorityExecute = 10;

		/// <summary>
		/// All of the softbodies alive right now
		/// </summary>
		private static List<Softbody> softbodyJobs = new List<Softbody> ();

		/// <summary>
		/// All of the priority jobs
		/// </summary>
		private static List<Softbody> priorityJobs = new List<Softbody> ();

		/// <summary>
		/// The current iteration index
		/// </summary>
		private static int index;

		#endregion

		#region METHODS

		/// <summary>
		/// Update this object
		/// </summary>
		private void Update ()
		{
			if (softbodyJobs.Count == 0)
				return;

			int targetIndex = (index + jobsPerFrame) % softbodyJobs.Count;

			//Priority loop
			if (priorityLoop)
				for (int i = 0; i < priorityJobs.Count; i++)
					priorityJobs[i].UpdateSoftbody ();

			//Standard loop
			do
			{
				if (!priorityJobs.Contains (softbodyJobs[index]))
					softbodyJobs[index].UpdateSoftbody ();

				index = (index + 1) % softbodyJobs.Count;
			}
			while (index != targetIndex);

			priorityJobs.Clear ();
		}

		/// <summary>
		/// Register the softbody
		/// </summary>
		/// <param name="softbody"></param>
		public static void RegisterJob (Softbody softbody)
		{
			softbodyJobs.Add (softbody);
		}
		
		/// <summary>
		/// Deregister the softbody
		/// </summary>
		/// <param name="softbody"></param>
		public static void DeregisterJob (Softbody softbody)
		{
			softbodyJobs.Remove (softbody);

			if (index >= softbodyJobs.Count)
				index = Mathf.Clamp (index - 1, 0, softbodyJobs.Count - 1);

			if (priorityJobs.Contains (softbody))
				priorityJobs.Remove (softbody);
		}

		/// <summary>
		/// Register a job to execute immediately
		/// </summary>
		/// <param name="softbody"></param>
		public void RegisterPriorityJob (Softbody softbody)
		{
			if (!priorityLoop)
				return;

			if (softbody.Priority >= priorityExecute && !priorityJobs.Contains (softbody))
			{
				priorityJobs.Add (softbody);

				if (debug)
					print ("Softbody priority job count: " + priorityJobs.Count);
			}
		}

		#endregion

		#region SUB_CLASSES

#if UNITY_EDITOR

		/// <summary>
		/// Editor script for softbody
		/// </summary>
		[CustomEditor (typeof (SoftbodyJobManager))]
		public class SoftbodyJobManagerEditor : EditorBase
		{
			#region METHODS

			/// <summary>
			/// Draw the inspector
			/// </summary>
			public override void OnInspectorGUI ()
			{
				base.OnInspectorGUI ();

				EditorGUILayout.LabelField ("Softbody Jobs: " + softbodyJobs.Count);
				EditorGUILayout.LabelField ("Priority Jobs: " + priorityJobs.Count);
			}

			#endregion
		}

#endif

		#endregion
	}
}