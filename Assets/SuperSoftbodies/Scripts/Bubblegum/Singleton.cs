using UnityEngine;

namespace Bubblegum
{

	/// <summary>
	/// Denotes a model type object that will only have one instance exist an once but still 
	/// requires the Unity monobehaviour methods to function
	/// </summary>
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// Gets the single instance of the class and creates a new one if none exists
		/// </summary>
		/// <value>The instance.</value>
		public static T Instance
		{
			get
			{
				//If the application is ending then return null
				if (applicationQuiting)
				{
					print ("Trying to access instance of type " + typeof (T).ToString () + " while the application is exiting," +
						" this is not allowed and will return null");
					return null;
				}

				//If we don't have an instace hen find it
				if (localInstance == null)
				{
					localInstance = FindObjectOfType (typeof (T)) as T;

					//If we couldn't find an instance then create one
					if (localInstance == null)
					{
						localInstance = new GameObject ().AddComponent<T> ();
						localInstance.gameObject.name = typeof (T).ToString ();
						DontDestroyOnLoad (localInstance.gameObject);

						print ("No instance of type " + typeof (T).ToString () + " exists in the scene, adding a new one now with the" +
							   " DontDestroyOnLoad functionality");
					}
				}

				return localInstance;
			}
		}

		/// <summary>
		/// If we are exiting the application
		/// </summary>
		public static bool Quiting
		{
			get
			{
				return applicationQuiting;
			}
		}

		#endregion // PUBLIC_VARIABLES

		#region PRIVATE_VARIABLES

		/// <summary>
		/// If the app is currently quiting, we should not allow an instance to be aquired
		/// </summary>
		private static bool applicationQuiting;

		/// <summary>
		/// The local instance of the singleton
		/// </summary>
		protected static T localInstance;

		#endregion // PRIVATE_VARIABLES

		#region MONOBEHAVIOUR_METHODS

		/// <summary>
		/// Called when all objects have been initialized regardless of whether the script is enabled
		/// </summary>
		protected virtual void Awake ()
		{
			if (localInstance != null && localInstance != this)
			{
				Destroy (this);
				throw new DuplicateSingletonException (gameObject);
			}
			else
				localInstance = this as T;
		}

		/// <summary>
		/// When the application quits
		/// </summary>
		void OnApplicationQuit ()
		{
			applicationQuiting = true;
		}

		#endregion // MONOBEHAVIOUR_METHODS

		#region SUB_CLASSES

		//Thrown when a duplicate singleton is attempted to be added to the scene
		public class DuplicateSingletonException : UnityException
		{

			/// <summary>
			/// Initializes a new instance of the singleton exception class
			/// </summary>
			public DuplicateSingletonException (GameObject go)
			{
				Debug.LogError ("Only one instance of singleton type " + typeof (T).ToString () + " can exist per scene, removing component from " + go.name);
			}

		}

		#endregion // SUB_CLASSES
	}
}