using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Bubblegum
{
    /// <summary>
    /// A system for dispatching code to execute on the main thread.
	/// Can't inherit singleton as there as Instance is called from other threads
    /// </summary>
	[ExecuteInEditMode]
    public class Dispatcher : MonoBehaviour
    {
		#region PUBLIC_VARIABLES

		/// <summary>
		/// Gets a value indicating whether or not the current thread is the game's main thread.
		/// </summary>
		public bool IsMainThread
		{
			get
			{
				return Thread.CurrentThread == mainThread;
			}
		}

		/// <summary>
		/// Gets the single instance of the class and creates a new one if none exists
		/// </summary>
		/// <value>The instance.</value>
		public static Dispatcher Instance
		{
			get
			{
				if (localInstance == null)
					Debug.LogError ("No instance of type " + typeof (Dispatcher).ToString () + " exists in the scene, you must add one manually.");

				return localInstance;
			}
		}

		#endregion

		#region PRIVATE_VARIABLES
		
		/// <summary>
		/// The local instance of the singleton
		/// </summary>
		protected static Dispatcher localInstance;

		/// <summary>
		/// The main thread to execute on
		/// </summary>
		private Thread mainThread;

		/// <summary>
		/// Thread safe locker
		/// </summary>
        private object lockObject = new object();

		/// <summary>
		/// All of the actions to invoke
		/// </summary>
        private readonly Queue<Action> actions = new Queue<Action>();

		#endregion

		#region MONOBEHAVIOUR_METHODS

		/// <summary>
		/// Awaken this instance
		/// </summary>
		void OnEnable ()
		{
			localInstance = this;
			mainThread = Thread.CurrentThread;

#if UNITY_EDITOR
			if (!Application.isPlaying)
				UnityEditor.EditorApplication.update += Update;
#endif
		}

		/// <summary>
		/// Update this object
		/// </summary>
		void Update ()
		{
			lock (lockObject)
				while (actions.Count > 0)
					actions.Dequeue () ();
		}

		#endregion

		#region PUBLIC_METHODS

		/// <summary>
		/// Queues an action to be invoked on the main game thread.
		/// </summary>
		/// <param name="action">The action to be queued.</param>
		public void InvokeAsync(Action action)
        {
			// Don't bother queuing work on the main thread; just execute it.
			if (IsMainThread)                
                action();
            else
                lock (lockObject)
                    actions.Enqueue(action);
        }

        /// <summary>
        /// Queues an action to be invoked on the main game thread and blocks the
        /// current thread until the action has been executed.
        /// </summary>
        /// <param name="action">The action to be queued.</param>
        public void Invoke(Action action)
        {
            bool hasRun = false;

            InvokeAsync(() =>
            {
                action();
                hasRun = true;
            });

            // Lock until the action has run
            while (!hasRun)
                Thread.Sleep(5);
        }

		#endregion
	}
}
