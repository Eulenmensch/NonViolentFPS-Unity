using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bubblegum
{

	/// <summary>
	/// Used as a key in the editor rather than strings in dictionaries and similar
	/// </summary>
	[CreateAssetMenu (menuName = "Scriptable Object/Key")]
	public class Key : ScriptableObject
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// Get the key that deifnes this entity
		/// </summary>
		public int ID { get { return id; } }

		[Header ("Key")]

		/// <summary>
		/// The key used for serialization of this object
		/// </summary>
		[SerializeField, ReadOnly]
		protected int id;

		/// <summary>
		/// If we should debug this object
		/// </summary>
		[Tooltip ("If we should debug this object")]
		public bool debug = true;

		#endregion

		#region PRIVATE_VARAIBLES

		/// <summary>
		/// If all keys have had initialization triggered
		/// </summary>
		private static bool initialized;

		/// <summary>
		/// All of the unique entities
		/// </summary>
		protected static Dictionary<int, Key> allKeys = new Dictionary<int, Key> ();

		/// <summary>
		/// All of the saved data we have run through this key
		/// </summary>
		private Dictionary<string, object> savedData = new Dictionary<string, object> ();

		#endregion

		#region SCRIPTABLEOBJECT_METHODS

		/// <summary>
		/// Enable this object
		/// </summary>
		protected virtual void OnEnable ()
		{
			if (!Application.isPlaying)
				Register ();

			if (!initialized)
			{
				initialized = true;
				Key[] keys = Resources.LoadAll<Key> ("");

				foreach (Key key in keys)
					key.Register ();
			}
		}

		/// <summary>
		/// Whem this object is disabled
		/// </summary>
		protected virtual void OnDestroy ()
		{
			Deregister ();
		}

		#endregion

		#region PUBLIC_METHODS

		/// <summary>
		/// Get the key with the given id
		/// </summary>
		/// <param name="id"></param>
		public static T GetKey <T>(int id) where T : Key
		{
			if (allKeys.ContainsKey (id))
				return allKeys[id] as T;

			return default (T);
		}

		/// <summary>
		/// Save into this key
		/// </summary>
		/// <param name="key"></param>
		public void Save (string key, object value)
		{
			if (value is int)
				PlayerPrefs.SetInt (key + ID, (int) value);

			else if (value is float)
				PlayerPrefs.SetFloat (key + ID, (float) value);

			else if (value is string)
				PlayerPrefs.SetString (key + ID, (string) value);

			else
				throw new System.Exception ("Saving data of type " + value.GetType ().Name + " is not supported");

			//Debug data presentation
			savedData[key] = value;
		}

		/// <summary>
		/// Save into this key
		/// </summary>
		/// <param name="key"></param>
		public object Load (string key, object defaultValue)
		{
			object value;

			if (defaultValue is int)
				value = PlayerPrefs.GetInt (key + ID, (int) defaultValue);

			else if (defaultValue is float)
				value = PlayerPrefs.GetFloat (key + ID, (float) defaultValue);

			else if (defaultValue is string)
				value = PlayerPrefs.GetString (key + ID, (string) defaultValue);

			else
				throw new System.Exception ("Saving data of type " + defaultValue.GetType ().Name + " is not supported");

			//Debug data presentation
			savedData[key] = value;

			return value;
		}

		/// <summary>
		/// Clear the object with the given key
		/// </summary>
		/// <param name="key"></param>
		public void Clear (string key)
		{
			PlayerPrefs.DeleteKey (key);

			//Debug data presentation
			savedData[key] = null;
		}

		/// <summary>
		/// Force new key registration
		/// </summary>
		public void RegisterAsNewKey ()
		{
			id = Random.Range (0, int.MaxValue).GetHashCode ();

			if (allKeys.ContainsKey (id))
				RegisterAsNewKey ();
		}

		#endregion

		#region PRIVATE_METHODS

		/// <summary>
		/// Register this unique entity
		/// </summary>
		protected virtual void Register ()
		{
			GenerateKey ();

			if (!allKeys.ContainsKey (ID))
			{
				allKeys.Add (ID, this);

				if (debug)
					Debug.Log ("Registering " + name + " with key " + id);
			}
		}

		/// <summary>
		/// Remove this object from the dictionary
		/// </summary>
		private void Deregister ()
		{
			if (debug)
				Debug.Log ("Deregistering " + name + " with key " + id);

			if (allKeys.ContainsKey (ID))
				allKeys.Remove (ID);
		}

		/// <summary>
		/// Create a unique key for this object
		/// </summary>
		private void GenerateKey ()
		{
			if (Application.isPlaying)
				return;

#if UNITY_EDITOR
			if (id == 0 || (allKeys.ContainsKey (id) && allKeys[id] != this))
			{
				RegisterAsNewKey ();

				EditorUtility.SetDirty (this);
				AssetDatabase.SaveAssets ();

				Debug.Log ("Generated new key for " + name + " - " + id);
			}
#endif
		}

		#endregion

	}
}