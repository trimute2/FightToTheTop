using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlagHandler))]
[RequireComponent(typeof(MoveHandler))]
public class HealthComponent : MonoBehaviour {

	public int maxHealth = 100;
	private int health;
	private MoveHandler moveHandler;
	private FlagHandler flagHandler;
	public int Health
	{
		get
		{
			return health;
		}
	}

	private void Awake()
	{
		moveHandler = GetComponent<MoveHandler>();
		flagHandler = GetComponent<FlagHandler>();
	}

	private void Start()
	{
		health = maxHealth;
	}

	public void Damage(int damage, Vector2 knockBack)
	{
		if (flagHandler.CheckCommonFlag(CommonFlags.Dodgeing))
		{
			return;
		}
		health -= damage;
		moveHandler.EneterDamageState();
	}
}
