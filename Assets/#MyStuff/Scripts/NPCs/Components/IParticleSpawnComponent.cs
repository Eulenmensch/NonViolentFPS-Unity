using System.Collections.Generic;
using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IParticleSpawnComponent
	{
		Transform SpawnPoint { get; set; }
		HashSet<GameObject> Particles { get; set; }
	}
}