using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyCommands
{
	None = 0,
	Idle,
	MoveCloser,
	MoveAway,
	Attack,
}

public class Enemy : EntityController {

	public float baseAgression;
	private bool active;
	private float decisionRange;
	private EnemyCommands currentComand;

	/// <summary>
	/// The target of the enemies attacks
	/// </summary>
	private EntityController target;

	/// <summary>
	/// The Target of the enemies attacks
	/// </summary>
	public EntityController Target
	{
		get
		{
			return target;
		}
		set
		{
			target = value;
			active = true;
		}
	}

	/// <summary>
	/// the Entity ID of the target
	/// </summary>
	public int TargetID
	{
		get
		{
			return target.EntityID;
		}
	}

	/// <summary>
	/// the distance to the enemies target
	/// </summary>
	private float xdistance;
	/// <summary>
	/// Is this enemy close enough to the player to start considering attacks
	/// </summary>
	private bool inDecisionRange;
	
	// Use this for initialization
	public override void Start()
	{
		base.Start();
		//TODO calculate decisionRange

		active = false;
		xdistance = float.MaxValue;
		//temp decisionRange for test
		decisionRange = 7.5f;

		currentComand = EnemyCommands.None;
	}

	protected override Vector2 EntityUpdate(Vector2 previousTarget)
	{
		if (!active || target == null)
		{
			inDecisionRange = false;
			//if the enemy is not active or the target has not been set dont do anything
			return Vector2.zero;
		}
		//update distance to target
		xdistance = target.transform.position.x - transform.position.x;
		inDecisionRange = (Mathf.Abs(xdistance) <= decisionRange);
		if((flagData.commonFlags & CommonFlags.CanTurn) != CommonFlags.None)
		{
			if(Mathf.Sign(xdistance) != facing)
			{
				Vector3 sca = transform.localScale;
				facing = (int)Mathf.Sign(xdistance);
				sca.x = facing;
				transform.localScale = sca;
			}
		}
		if (inDecisionRange)
		{
			//TODO check if sharing space with another enemy
			//It maybe aceptable to share a position with another enemy if that enemy is of a different type
		}
		else
		{
			targetVelocity.x = facing * movementSpeed;
		}

		return new Vector2(Mathf.Abs(targetVelocity.x), velocity.y);
	}

	// Update is called once per frame
	protected override void CheckMoves()
	{
		if(inDecisionRange)
		{
			
			
		}
	}


	protected override void ExecuteCondition(LinkCondition condition)
	{
	}

}
