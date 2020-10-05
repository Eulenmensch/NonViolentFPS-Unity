using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bubblegum
{

	/// <summary>
	/// Behaviour that stores references to component for super quick access
	/// </summary>
	public abstract class CacheBehaviour : MonoBehaviour
	{
		#region PUBLIC_VARIABLES

		[Header ("Cache Mono")]

		/// <summary>
		/// If we should debug this component
		/// </summary>
		[SerializeField, Tooltip ("If we should debug this component")]
		protected bool debug;

		/// <summary>
		/// The attached transform
		/// </summary>
		public Transform CachedTransform
		{
			get
			{
				if (!cachedTransform)
					cachedTransform = transform;

				return cachedTransform;
			}
		}

		#endregion

		#region PRIVATE_VARIABLES

		/// <summary>
		/// The cached transform
		/// </summary>
		private Transform cachedTransform;

		/// <summary>
		/// All of the components attached to this object
		/// </summary>
		private Dictionary<Type, Component> cache = new Dictionary<Type, Component> ();

		#endregion

		#region PUBLIC_METHODS

		/// <summary>
		/// Force the object to cache a component of the given type
		/// </summary>
		/// <param name="type"></param>
		public void Cache<T> ()
		{
			Cache (typeof (T));
		}

		/// <summary>
		/// Force the object to cache a component of the given type
		/// </summary>
		/// <param name="type"></param>
		public void Cache (Type type)
		{
			if (!cache.ContainsKey (type))
				cache.Add (type, GetComponent (type));
			else
				Debug.LogWarning (name + " is already caching an object of type " + type.Name);
		}

		/// <summary>
		/// Check if the type of T is cached
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public bool IsCached<T> ()
		{
			return cache.ContainsKey (typeof (T));
		}

		/// <summary>
		/// Get the component of the given type
		/// </summary>
		/// <param name="type"></param>
		/// <param name="value"></param>
		public T GetCachedComponent<T> () where T : Component
		{
			Component value;

			if (!cache.TryGetValue (typeof (T), out value))
			{
				value = GetComponent<T> ();
				cache.Add (typeof (T), value);
			}

			return value as T;
		}

		#endregion
	}
}