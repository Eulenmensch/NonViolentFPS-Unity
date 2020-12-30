using Ludiq.OdinSerializer;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class IncreaseMoodEffect : SerializedMonoBehaviour, IHitscanEffect
	{
		[SerializeField] private float moodIncrease;

		public void Initialize(RaycastHit _hit)
		{
			var moodNPC = _hit.collider.GetComponent<MoodNPC>();
			if (moodNPC == null)
			{
				NPC.ThrowComponentMissingError(typeof(MoodNPC));
				return;
			}

			moodNPC.Mood += moodIncrease;
		}

		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}