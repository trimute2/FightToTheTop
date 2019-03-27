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
	// Start is called before the first frame update
	void Start()
    {
		healthSlider.maxValue = player.maxHealth;
		healthSlider.value = player.maxHealth;
		player.HealthUpdateEvent += UpdateHealthSlider;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}
	}

	void UpdateHealthSlider()
	{
		healthSlider.value = player.Health;
	}

	public void Pause()
	{
		Time.timeScale = 0;
		GameUIPanel.gameObject.SetActive(false);
		PausePanel.gameObject.SetActive(true);
	}

	public void Unpause()
	{
		Time.timeScale = 1;
		GameUIPanel.gameObject.SetActive(true);
		PausePanel.gameObject.SetActive(false);
	}
}
