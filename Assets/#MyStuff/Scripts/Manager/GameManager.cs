using System;
using UnityEngine;

public class GameManager :MonoBehaviour
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
}
