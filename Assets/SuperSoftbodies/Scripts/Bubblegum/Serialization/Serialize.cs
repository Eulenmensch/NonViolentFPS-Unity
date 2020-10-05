using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

using System.Runtime.Serialization;
#if !UNITY_WINRT
using System.Runtime.Serialization.Formatters.Binary;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Bubblegum.Serialization
{

	/// <summary>
	/// Model type class for serializing objects
	/// </summary>
	public static class Serialize
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// The formatter for UWP
		/// </summary>
		public static DataContractSerializer formatter;

#if !UNITY_WINRT
		/// <summary>
		/// The binary formatter used to format the class to serialize
		/// </summary>
		public static BinaryFormatter oldFormatter = new BinaryFormatter ();
#endif

		#endregion // PUBLIC_VARIABLES

		#region PRIVATE_VARIABLES

		/// <summary>
		/// The special types that can be serialized using special methods
		/// </summary>
		private static List<Type> specialTypes = new List<Type>
		{
			typeof (Vector2),
			typeof (Vector3),
			typeof (Vector4),
			typeof (Quaternion),
			typeof (Color),
			typeof (AnimationCurve),
			typeof (Keyframe)
		};

		#endregion

		#region PUBLIC_METHODS

		/// <summary>
		/// Check if the type is convertable
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool Convertable (Type type)
		{
			return specialTypes.Contains (type);
		}

		/// <summary>
		/// Convert the value to a special 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static object ConvertToSerializable (object value)
		{
			return SerializeSpecialObject (value);
		}

		/// <summary>
		/// Convert serializable back to orignal value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static object UnconvertFromSerializable (object value, Type type)
		{
			return DeserializeSpecialObject (value, type);
		}

		/// <summary>
		/// Save the specified object under the preferences tag given
		/// </summary>
		/// <param name="prefsTag">Prefs tag.</param>
		/// <param name="obj">Object.</param>
		public static string Save (string prefsTag, object obj)
		{
			string saveObj = SerializeObject (obj);
			PlayerPrefs.SetString (prefsTag, saveObj);
			return saveObj;
		}

		/// <summary>
		/// Load the object from the specified prefs tag, prefs key must exist, otherwise we will return null
		/// </summary>
		/// <param name="prefsTag">Prefs tag.</param>
		public static object Load (string prefsTag, Type type)
		{
			if (string.IsNullOrEmpty (prefsTag) || PlayerPrefs.HasKey (prefsTag))
			{
				string loadObj = PlayerPrefs.GetString (prefsTag);

				//Make sure the string exists
				if (string.IsNullOrEmpty (loadObj))
					return null;

				return DeserializeObject (loadObj, type);
			}
			else
			{
				Debug.LogError ("Prefs tag " + prefsTag + " does not exist, you must make sure it exists before trying to deserialize an object");
				return null;
			}
		}

		/// <summary>
		/// Serialize the object and return the string value
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string SerializeObject (object obj)
		{
			if (specialTypes.Contains (obj.GetType ()))
				obj = SerializeSpecialObject (obj);

			formatter = new DataContractSerializer (obj.GetType ());

			using (MemoryStream memoryStream = new MemoryStream ())
			{
				formatter.WriteObject (memoryStream, obj);
				return System.Convert.ToBase64String (memoryStream.ToArray ());
			}
		}

		/// <summary>
		/// Deserialize the string and return the object value
		/// </summary>
		/// <param name="objValue"></param>
		/// <returns></returns>
		public static object DeserializeObject (string objValue, Type type)
		{
			Type specialType = specialTypes.Contains (type) ? ConvertToSpecialType (type) : null;
			formatter = new DataContractSerializer (specialType != null ? specialType : type);
			object obj = null;

			using (MemoryStream memoryStream = new MemoryStream (System.Convert.FromBase64String (objValue)))
			{
				try
				{
					obj = formatter.ReadObject (memoryStream);

					if (specialType != null)
						obj = DeserializeSpecialObject (obj, type);

					return obj;
				}
				catch (System.Xml.XmlException exception)
				{
#if !UNITY_WINRT
					Debug.LogError ("Attempting to fix exception: " + exception.ToString ());

					memoryStream.Seek (0, SeekOrigin.Begin);
					obj = oldFormatter.Deserialize (memoryStream);
					SerializeObject (obj);
					return obj;
#else
					throw exception;
#endif
				}
			}
		}

#if UNITY_EDITOR

		/// <summary>
		/// Save the specified object under the preferences tag given
		/// </summary>
		/// <param name="prefsTag">Prefs tag.</param>
		/// <param name="obj">Object.</param>
		public static void SaveEditor (string prefsTag, object obj)
		{			
			EditorPrefs.SetString (prefsTag, SerializeObject (obj));
		}

		/// <summary>
		/// Load the object from the specified prefs tag, prefs key must exist, otherwise we will return null
		/// </summary>
		/// <param name="prefsTag">Prefs tag.</param>
		public static object LoadEditor (string prefsTag, Type type)
		{
			if (string.IsNullOrEmpty (prefsTag) || EditorPrefs.HasKey (prefsTag))
				return DeserializeObject (EditorPrefs.GetString (prefsTag), type);
			else
			{
				Debug.LogError ("Prefs tag " + prefsTag + " does not exist, you must make sure it exists before trying to deserialize an object");
				return null;
			}
		}

#endif

		/// <summary>
		/// Clear the specified save file
		/// </summary>
		/// <param name="prefsTag">Prefs tag.</param>
		public static void Clear (string prefsTag)
		{
			PlayerPrefs.DeleteKey (prefsTag);
		}

#endregion

#region PRIVATE_METHODS

		/// <summary>
		/// Serialize the object by converting to a serializable type
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private static object SerializeSpecialObject (object obj)
		{
			if (obj is Vector2)
				return new Float2 ((Vector2) obj);

			else if (obj is Vector3)
				return new Float3 ((Vector3) obj);

			else if (obj is Vector4)
				return new Float4 ((Vector4) obj);

			else if (obj is Quaternion)
				return new Float4 ((Quaternion) obj);

			else if (obj is Color)
				return new ColorSerializable ((Color) obj);

			else if (obj is AnimationCurve)
				return new AnimationCurveSerializable ((AnimationCurve) obj);

			else if (obj is Keyframe)
				return new KeyframeSerializable ((Keyframe) obj);

			else
				throw new NotSupportedException ("Object of type " + obj.GetType () + " is not serializable and can't be converted");
		}

		/// <summary>
		/// Deserialize the given object
		/// </summary>
		/// <param name="objValue"></param>
		/// <returns></returns>
		private static object DeserializeSpecialObject (object obj, Type type)
		{
			if (type == typeof (Vector2))
				return ((Float2) obj).ToVector2 ();

			else if (type == typeof (Vector3))
				return ((Float3) obj).ToVector3 ();

			else if (type == typeof (Vector4) || type == typeof (Quaternion))
				return ((Float4) obj).ToSavedType ();

			else if (type == typeof (Color))
				return ((ColorSerializable) obj).ToColor ();

			else if (obj is AnimationCurve)
				return ((AnimationCurveSerializable) obj).ToAnimationCurve ();

			else if (obj is Keyframe)
				return ((KeyframeSerializable) obj).ToKeyframe ();

			else
				throw new NotSupportedException ("Could not deserialize object to type " + type.Name);
		}

		/// <summary>
		/// Convert the type to it's special type alternative
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private static Type ConvertToSpecialType (Type type)
		{
			if (type == typeof (Vector2))
				return typeof (Float2);

			else if (type == typeof (Vector3))
				return typeof (Float3);

			else if (type == typeof (Vector4) || type == typeof (Quaternion))
				return typeof (Float4);

			else if (type == typeof (Color))
				return typeof (ColorSerializable);

			else if (type == typeof (AnimationCurve))
				return typeof (AnimationCurveSerializable);

			else if (type == typeof (Keyframe))
				return typeof (KeyframeSerializable);

			else
				throw new NotSupportedException ("Type " + type.Name + " is not convertible as a special type");
		}

#endregion

	}
}