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
		if (stunned)
		{
			if(Time.time - hitTime < stunDuration)
			{
				//object is stunned
				if (hasEntityController)
				{
					entityController.TargetVelocity = knockBack;
				}
			}
			else
			{
				stunned = false;
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
