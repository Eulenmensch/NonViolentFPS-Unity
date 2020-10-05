using System.Linq;
using UnityEngine;

namespace Bubblegum.Serialization
{

	/// <summary>
	/// Serializable version of unity animation curve
	/// </summary>
	[System.Serializable]
	public class AnimationCurveSerializable
	{
		#region VARIABLES

		/// <summary>
		/// Keys of the curve
		/// </summary>
		public KeyframeSerializable[] frames;

		/// <summary>
		/// Wrap mode after the curve
		/// </summary>
		public int postWrapMode;

		/// <summary>
		/// Wrap mode before the curve
		/// </summary>
		public int preWrapMode;

		#endregion

		#region CONSTRUCTORS

		/// <summary>
		/// Create a new curve
		/// </summary>
		/// <param name="curve"></param>
		public AnimationCurveSerializable (AnimationCurve curve)
		{
			frames = curve.keys.Select (key => new KeyframeSerializable (key)).ToArray ();
			postWrapMode = (int) curve.postWrapMode;
			preWrapMode = (int) curve.preWrapMode;
		}

		#endregion

		#region METHODS

		/// <summary>
		/// Get this frame as a keyframe
		/// </summary>
		/// <returns></returns>
		public AnimationCurve ToAnimationCurve ()
		{
			AnimationCurve curve = new AnimationCurve (frames.Select (frame => frame.ToKeyframe ()).ToArray ());
			curve.preWrapMode = (WrapMode) preWrapMode;
			curve.postWrapMode = (WrapMode) postWrapMode;
			return curve;
		}

		#endregion
	}

	/// <summary>
	/// Serializable version of Unity keyframe
	/// </summary>
	[System.Serializable]
	public struct KeyframeSerializable
	{
		#region VARIABLES

		/// <summary>
		/// Tangent approaching the curve
		/// </summary>
		public float inTangent;

		/// <summary>
		/// Tangent leaving the curve
		/// </summary>
		public float outTangent;

		/// <summary>
		/// Tangent mode
		/// </summary>
		public int tangentMode;

		/// <summary>
		/// Time of the keyframe
		/// </summary>
		public float time;

		/// <summary>
		/// Value of the key
		/// </summary>
		public float value;

		#endregion

		#region CONSTRUCTORS

		/// <summary>
		/// Create a new frame
		/// </summary>
		public KeyframeSerializable (Keyframe key)
		{
			inTangent = key.inTangent;
			outTangent = key.outTangent;
			tangentMode = key.tangentMode;
			time = key.time;
			value = key.value;
		}

		#endregion

		#region METHODS

		/// <summary>
		/// Get this frame as a keyframe
		/// </summary>
		/// <returns></returns>
		public Keyframe ToKeyframe ()
		{
			Keyframe keyframe = new Keyframe (time, value, inTangent, outTangent);
			keyframe.tangentMode = tangentMode;
			return keyframe;
		}

		#endregion
	}
}