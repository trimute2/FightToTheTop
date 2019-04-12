using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
[RequireComponent(typeof(FlagHandler))]
public class StunComponent : MonoBehaviour {
	

	private float hitTime = 0;
	private float stunDuration;

	private Vector2 knockBack;

	private bool stunned = false;
	private bool liftOff = false;

	public bool Stunned
	{
		get
		{
			return stunned;
		}
	}

	private MoveHandler moveHandler;
	private FlagHandler flagHandler;
	private Animator animator;

	private EntityControllerComp entityController;
	private bool hasEntityController;

	public int baseStunGauge;
	private int stunGauge;
	


	private void Start()
	{
		moveHandler = GetComponent<MoveHandler>();
		flagHandler = GetComponent<FlagHandler>();
		animator = GetComponent<Animator>();
		stunGauge = baseStunGauge;

		entityController = GetComponent<EntityControllerComp>();
		hasEntityController = (entityController != null);

	}

	// Update is called once per frame
	void Update () {
		if(Time.timeScale == 0)
		{
			return;
		}
		if (stunned)
		{
			bool stopStun = false;
			if(liftOff && hasEntityController && knockBack != Vector2.zero)
			{
				if (entityController.Grounded)
				{
					stopStun = true;
				}
				else
				{
					entityController.TargetVelocity = knockBack;
				}
			} else if(Time.time - hitTime < stunDuration)
			{
				if(hasEntityController && knockBack != Vector2.zero)
				{
					entityController.TargetVelocity = knockBack;
					if (!entityController.Grounded)
					{
						liftOff = true;
					}
				}
			}
			else
			{
				stopStun = true;
			}
			if(stopStun)
			{
				stunned = false;
				liftOff = false;
				moveHandler.EnterGenericState();
			}
		}
	}

	public void Stun(Vector2 knockBack, float stunDuration, int stunPoints)
	{
		stunGauge -= stunPoints;
		if(stunGauge <= 0)
		{
			//TODO: stun gauge stuff
		}
		hitTime = Time.time;
		if (!stunned)
		{
			stunned = true;
			this.stunDuration = stunDuration;
			this.knockBack = knockBack;
			string toPlay = "Damage";
			if (this.knockBack != Vector2.zero)
			{
				toPlay = "Knock Back";
			}
			moveHandler.EnterGenericState(toPlay, 0.001111111f);
			flagHandler.CommonFlags = CommonFlags.None;
		}
		else
		{
			bool playAnimation = false;
			string toPlay = "Damage";
			if (this.knockBack == Vector2.zero) {
				if (stunDuration > this.stunDuration)
				{
					this.stunDuration = stunDuration;
					playAnimation = true;
				}
			}
			else
			{
				toPlay = "Knock Back";
				playAnimation = true;
			}
			if (knockBack != Vector2.zero)
			{
				toPlay = "Knock Back";
				this.knockBack = knockBack;
				playAnimation = true;
			}
			if (playAnimation)
			{
				animator.Play(toPlay, -1 , 0);
			}
		}
	}
}
