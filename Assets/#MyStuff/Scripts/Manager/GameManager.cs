using UnityEngine;
using UnityEngine.SceneManagement;

namespace NonViolentFPS.Manager
{
	public class GameManager : MonoBehaviour
	{
		#region Singleton
		public static GameManager Instance { get; private set; }

		private void Awake()
		{
			if ( Instance != null && Instance != this )
			{
				Destroy( this );
			}
			else
			{
				Instance = this;
			}
		}
		#endregion

		[SerializeField] private GameObject player;
		public GameObject Player
		{
			get => player;
			set => player = value;
		}

		//FIXME: Only for testing
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}
	}
}
