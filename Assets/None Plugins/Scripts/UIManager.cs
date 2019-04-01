using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	public Slider healthSlider;
	public HealthComponent player;
	public PlayerInputHandler pi;
	public RectTransform GameUIPanel;
	public RectTransform PausePanel;
	public RectTransform DeathPanel;
	public Button PauseButton;
	public Button DeathButton;
	public EventSystem eventSystem;
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
			eventSystem.gameObject.SetActive(true);
			eventSystem.UpdateModules();
			//PauseButton.Select();
			pi.UnHookInputActions();
			Debug.Log(eventSystem.isFocused);
		}
	}

	public void Unpause()
	{
		Time.timeScale = 1;
		GameUIPanel.gameObject.SetActive(true);
		PausePanel.gameObject.SetActive(false);
		eventSystem.gameObject.SetActive(false);
		pi.HookInputActions();
	}

	public void OnPlayerDeath()
	{
		GameUIPanel.gameObject.SetActive(false);
		DeathPanel.gameObject.SetActive(true);
		canPause = false;
		eventSystem.gameObject.SetActive(true);
		eventSystem.UpdateModules();
		DeathButton.Select();
	}

	private void OnDestroy()
	{
		p.gameplay.Pause.performed -= Pause_performed;
		p.Disable();
	}
}
