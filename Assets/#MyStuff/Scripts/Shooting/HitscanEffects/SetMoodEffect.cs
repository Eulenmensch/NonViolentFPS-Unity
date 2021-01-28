using NonViolentFPS.NPCs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class SetMoodEffect : SerializedMonoBehaviour, IHitscanEffect
	{
		[SerializeField] private float mood;
		
		public void Initialize(RaycastHit _hit)
		{
			var moodNPC = GetComponentInParent<MoodNPC>(); //Because only the main body collider object has a MoodNPC component attached
			if (moodNPC == null)
			{
				NPC.ThrowComponentMissingError(typeof(MoodNPC));
				return;
			}

			moodNPC.Mood = mood;

			Destroy();
		}

		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}