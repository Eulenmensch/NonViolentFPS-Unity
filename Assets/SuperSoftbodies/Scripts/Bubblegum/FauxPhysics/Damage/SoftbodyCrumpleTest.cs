using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Bubblegum.FauxPhysics.Damage
{

	/// <summary>
	/// Crumple animaton we play at runtime
	/// </summary>
	public class SoftbodyCrumpleTest : MonoBehaviour
	{
		#region VARIABLES

		/// <summary>
		/// If we should play the crumple in the start method
		/// </summary>
		[SerializeField, Tooltip ("If we should play the crumple in the start method")]
		private bool playOnStart = true;

		/// <summary>
		/// The speed to crumple at
		/// </summary>
		[SerializeField, Tooltip ("The speed to crumple at")]
		private float crumpleSpeed = 2f;

		/// <summary>
		/// Delay between each node crumple
		/// </summary>
		[SerializeField, Tooltip ("Delay between each node crumple")]
		private float crumpleNodeDelay = 1f;

		/// <summary>
		/// Events to invoke when crumple is complete
		/// </summary>
		[SerializeField, Tooltip ("Events to invoke when crumple is complete")]
		private UnityEvent onCrumpleFinished;

		#endregion

		#region METHODS

		/// <summary>
		/// Start this object
		/// </summary>
		private void Start ()
		{
			if (playOnStart)
				StartCoroutine (CrumpleAnimation ());
		}

		/// <summary>
		/// Start the crumple
		/// </summary>
		public void Crumple ()
		{
			StopAllCoroutines ();
			StartCoroutine (CrumpleAnimation ());
		}

		/// <summary>
		/// Crumple our softbody
		/// </summary>
		/// <returns></returns>
		private IEnumerator CrumpleAnimation ()
		{
			SoftbodyNode[] nodes = GetComponentsInChildren<SoftbodyNode> ();

			foreach (SoftbodyNode node in nodes)
			{
				float move = 0f;

				while (move < 1f)
				{
					move += Time.deltaTime * crumpleSpeed;
					node.Move (move);

					yield return null;
				}

				yield return new WaitForSeconds (crumpleNodeDelay);
			}

			onCrumpleFinished.Invoke ();
		}

		#endregion
	}
}