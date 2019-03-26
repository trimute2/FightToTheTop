using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Slider healthSlider;
	public HealthComponent player;
	// Start is called before the first frame update
	void Start()
    {
		healthSlider.maxValue = player.maxHealth;
		healthSlider.value = player.maxHealth;
		player.HealthUpdateEvent += UpdateHealthSlider;
	}

	void UpdateHealthSlider()
	{
		healthSlider.value = player.Health;
	}
}
