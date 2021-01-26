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
			var moodNPC = GetComponentInParent<MoodNPC>(); //Because only the main body collider object has a MoodNPC component attached
			if (moodNPC == null)
			{
				NPC.ThrowComponentMissingError(typeof(MoodNPC));
				return;
			}

			moodNPC.Mood += moodIncrease;

			Destroy();
		}

		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}