using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public delegate void OnGameObjectLoaded();
	public event OnGameObjectLoaded PlayerLoaded;
	private static GameManager _instance = null;
	private HealthComponent playerHealth;
	public HealthComponent PlayerHealth
	{
		get
		{
			return playerHealth;
		}
		set
		{
			playerHealth = value;
			if (PlayerLoaded != null)
			{
				PlayerLoaded();
			}
		}
	}

	public static GameManager Instance
	{
		get
		{
			if(_instance == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<GameManager>();
			}
			return _instance;
		}
	}

	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
			return;
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(this);
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if(mode == LoadSceneMode.Single)
		{
			Time.timeScale = 1;
		}
	}
}
