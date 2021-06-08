#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NonViolentFPS.SceneManagement
{
	[CreateAssetMenu(fileName = "SceneLoader", menuName = "SceneManagement/Loader", order = 0)]
	public class SceneSetupLoader : SerializedScriptableObject
	{
		[SerializeField]private SceneSetup[] setup;

		[Button("Save Scene Setup", ButtonSizes.Large)]
		private void SaveConfig()
		{
			setup = EditorSceneManager.GetSceneManagerSetup();
		}

		[Button("Load Scene Setup", ButtonSizes.Large)]
		private void LoadConfig()
		{
			EditorSceneManager.RestoreSceneManagerSetup(setup);
		}
	}
}
#endif