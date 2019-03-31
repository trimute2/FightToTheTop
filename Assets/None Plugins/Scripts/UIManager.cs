using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Slider healthSlider;
	public HealthComponent player;
	public RectTransform GameUIPanel;
	public RectTransform PausePanel;
	public RectTransform DeathPanel;
	public GameObject EventSystem;
	bool canPause;
	public PlayerInput p;
	// Start is called before the first frame update
	void Start()
    {
		healthSlider.maxValue = player.maxHealth;
		healthSlider.value = player.maxHealth;
		player.HealthUpdateEvent += UpdateHealthSlider;
		player.OnDeath += OnPlayerDeath;
		canPause = true;
		p.gameplay.Pause.performed += Pause_performed;
		p.gameplay.Pause.Enable();
	}

	private void Pause_performed(UnityEngine.Experimental.Input.InputAction.CallbackContext obj)
	{
		Pause();
	}

	/*
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}
	}*/

	void UpdateHealthSlider()
	{
		healthSlider.value = player.Health;
	}

	public void Pause()
	{
		if (canPause)
		{
			Time.timeScale = 0;
			GameUIPanel.gameObject.SetActive(false);
			PausePanel.gameObject.SetActive(true);
			EventSystem.SetActive(true);
		}
	}

	public void Unpause()
	{
		Time.timeScale = 1;
		GameUIPanel.gameObject.SetActive(true);
		PausePanel.gameObject.SetActive(false);
		EventSystem.SetActive(false);
	}

	public void OnPlayerDeath()
	{
		GameUIPanel.gameObject.SetActive(false);
		DeathPanel.gameObject.SetActive(true);
		canPause = false;
		EventSystem.SetActive(true);
	}
}
