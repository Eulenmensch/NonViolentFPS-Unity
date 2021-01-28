using NonViolentFPS.SceneManagement;
using UnityEngine;

namespace NonViolentFPS.GameModes
{
	public abstract class GameMode
	{
		[field: SerializeField] public SceneSetupLoader SceneSetup { get; set; }
		
	}
}