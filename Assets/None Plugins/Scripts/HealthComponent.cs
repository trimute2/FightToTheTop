﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthComponent : MonoBehaviour {
	public delegate void HealthUpdate();
	public event HealthUpdate HealthUpdateEvent;

	public int maxHealth = 100;
	private int health;
	public int Health
	{
		get
		{
			return health;
		}
	}

	private StunComponent stunComponent;
	private bool hasStunComponent;
	private FlagHandler flagHandler;
	private bool hasFlagHandler;

	private void Start()
	{
		health = maxHealth;
		stunComponent = GetComponent<StunComponent>();
		hasStunComponent = (stunComponent != null);
		flagHandler = GetComponent<FlagHandler>();
		hasFlagHandler = (flagHandler != null);
	}

	public void Damage(int damage, Vector2 knockBack, float stunDuration = 0.3f, int stunPoints = 0)
	{
		if (hasFlagHandler && flagHandler.CheckCommonFlag(CommonFlags.Dodgeing))
		{
			return;
		}
		health -= damage;
		if (hasStunComponent)
		{
			stunComponent.Stun(knockBack, stunDuration, stunPoints);
		}

		//call health update event
		if(HealthUpdateEvent != null)
		{
			HealthUpdateEvent();
		}
	}
}
