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

	private EntityControllerComp entityController;
	private bool hasEntityController;


	private void Start()
	{
		moveHandler = GetComponent<MoveHandler>();
		flagHandler = GetComponent<FlagHandler>();

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
		stunned = true;
		hitTime = Time.time;
		this.stunDuration = stunDuration;
		this.knockBack = knockBack;
		string toPlay = "Damage";
		if(this.knockBack != Vector2.zero)
		{
			toPlay = "Knock Back";
		}
		moveHandler.EnterGenericState(toPlay, 0.001111111f);
		flagHandler.CommonFlags = CommonFlags.None;
	}
}
