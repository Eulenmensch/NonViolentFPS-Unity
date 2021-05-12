using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NonViolentFPS.Player
{
	public class EnemyAttachmentPoints : SerializedMonoBehaviour
	{
		[field: SerializeField] public List<Transform> AttachmentPoints { get; set; }
	}
}