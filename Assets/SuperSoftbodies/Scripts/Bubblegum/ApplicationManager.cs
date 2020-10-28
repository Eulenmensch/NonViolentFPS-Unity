using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bubblegum
{

	/// <summary>
	/// Top level model class that overlooks the entire application
	/// </summary>
	public class ApplicationManager : Singleton<ApplicationManager>
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// If the application is paused
		/// </summary>
		public static bool paused { get; private set; }

		/// <summary>
		/// Scene managers check this bool to see if they need to load the main scene
		/// </summary>
		public static bool initialized { get; private set; }

		/// <summary>
		/// The last screen orientation that the user had
		/// We read DeviceOrientation incase the screen orientation is set to autorotate
		/// </summary>
		public static DeviceOrientation LastScreenOrientation { get; private set; }

		/// <summary>
		/// The paused time scale.
		/// </summary>
		[Tooltip ("The paused time scale.")]
		[SerializeField]
		private float pausedTimeScale = 0f;

		/// <summary>
		/// The application target frame rate
		/// </summary>
		[Tooltip ("The application target frame rate"), Range (15, 120)]
		[SerializeField]
		private int targetFrameRate = 30;

		/// <summary>
		/// If we should debug this component
		/// </summary>
		[SerializeField, Tooltip ("If we should debug this component")]
		private bool debug;

		#endregion // PUBLIC_VARIABLES

		#region EVENTS/DELEGATES

		/// <summary>
		/// Occurs when the app is paused
		/// </summary>
		public static event StateChanged onPause;

		/// <summary>
		/// Occurs when this app loses focus
		/// </summary>
		public static event StateChanged onLostFocus;

		/// <summary>
		/// Occurs when this app gains focus
		/// </summary>
		public static event StateChanged onGainedFocus;

		/// <summary>
		/// Called when the application ends
		/// </summary>
		public static event StateChanged onQuit;

		#endregion // EVENTS/DELEGATES

		#region PRIVATE_VARIABLES

		/// <summary>
		/// Timescale for when we resume
		/// </summary>
		private float resumeTimeScale;

		/// <summary>
		/// History we can use to reverse behaviour
		/// </summary>
		private Stack<System.Action> history = new Stack<System.Action> ();

		/// <summary>
		/// Prefs key for the last used orientation
		/// </summary>
		private const string LAST_SCREEN_ORIENTATION_KEY = "LastScreenOrientation";

		#endregion

		#region MONOBEHAVIOUR_METHODS

		/// <summary>
		/// Awake this instance.
		/// </summary>
		protected override void Awake ()
		{
			base.Awake ();

			if (debug)
				Debug.Log (System.Environment.Version);

			//Special cases
#if UNITY_ANDROID || UNITY_IOS
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif

			Application.targetFrameRate = targetFrameRate;
			LastScreenOrientation = (DeviceOrientation) PlayerPrefs.GetInt (LAST_SCREEN_ORIENTATION_KEY, (int) DeviceOrientation.LandscapeLeft);
		}

		/// <summary>
		/// Update this object
		/// </summary>
		private void Update ()
		{
			if (Input.deviceOrientation != DeviceOrientation.FaceDown && 
				Input.deviceOrientation != DeviceOrientation.FaceUp &&
				Input.deviceOrientation != DeviceOrientation.Unknown)
			{
				LastScreenOrientation = Input.deviceOrientation;
				PlayerPrefs.SetInt (LAST_SCREEN_ORIENTATION_KEY, (int) LastScreenOrientation);
			}
		}

		/// <summary>
		/// Raises the application focus event.
		/// </summary>
		/// <param name="focusStatus">If set to <c>true</c> focus status.</param>
		private void OnApplicationFocus (bool focusStatus)
		{
			if (focusStatus)
			{
				if (onGainedFocus != null)
					onGainedFocus ();

				if (debug)
					print ("Application has gained focus");
			}
			else
			{
				if (onLostFocus != null)
					onLostFocus ();

				if (debug)
					print ("Application has lost focus");
			}
		}

		/// <summary>
		/// When the app is exited
		/// </summary>
		private void OnApplicationQuit ()
		{
			if (onQuit != null)
				onQuit ();
		}

		#endregion // MONOBEHAVIOUR_METHODS

		#region PUBLIC_METHODS

#if UNITY_EDITOR

		/// <summary>
		/// Delete all player prefs
		/// </summary>
		[MenuItem ("Tools/Delete Player Prefs")]
		public static void DeleteMyPlayerPrefs ()
		{
			PlayerPrefs.DeleteAll ();
		}
#endif

		/// <summary>
		/// Converts the seconds to FPS
		/// </summary>
		/// <returns>The seconds to FPS</returns>
		/// <param name="seconds">Seconds.</param>
		public static int ConvertSecondsToFPS (float seconds)
		{
			return Mathf.Clamp (Mathf.Abs ((int) (Instance.targetFrameRate * seconds)), 1, int.MaxValue);
		}

		/// <summary>
		/// Resume the game
		/// </summary>
		public void Resume ()
		{
			if (paused)
			{
				paused = false;
				Time.timeScale = resumeTimeScale;
			}
		}

		/// <summary>
		/// Pause the application
		/// </summary>
		public void Pause ()
		{
			if (!paused)
			{
				resumeTimeScale = Time.timeScale;
				Time.timeScale = pausedTimeScale;

				if (onPause != null)
					onPause ();

				paused = true;
			}
		}

		/// <summary>
		/// Quit the application
		/// </summary>
		public void Quit ()
		{
			Application.Quit ();
		}

		/// <summary>
		/// Add history queue
		/// </summary>
		/// <param name="action"></param>
		public void AddHistory (System.Action action)
		{
			history.Push (action);
		}

		/// <summary>
		/// Pop the user history action
		/// </summary>
		public void PopHistory ()
		{
			history.Pop () ();
		}

		#endregion // PUBLIC_METHODS
	}
}