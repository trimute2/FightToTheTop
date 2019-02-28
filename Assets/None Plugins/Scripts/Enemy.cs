using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyCommands
{
	None = 0,
	Idle,
	MoveIn,
	MakeSpace,
	Attack,
}

public class Enemy : EntityController {
	//the structure of enemy is currently unorganized
	//currently changing it so that it will be organized around this tutorial
	//https://gamedevelopment.tutsplus.com/tutorials/battle-circle-ai-let-your-player-feel-like-theyre-fighting-lots-of-enemies--gamedev-13535
	private bool active;
	private float longRange;
	private float midRange;
	private float closeRange;
	private float decisionRange;
	protected EnemyCommands currentCommand;

	public EnemyCommands CurrentCommand
	{
		get
		{
			return currentCommand;
		}
		set
		{
			currentCommand = value;
		}
	}

	public float baseAgression;
	private float agro;

	public float Agro
	{
		get
		{
			return agro;
		}
	}

	public int enemySize;

	
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

	public float XDistance
	{
		get
		{
			return xdistance;
		}
	}

	/// <summary>
	/// Is this enemy close enough to the player to start considering attacks
	/// </summary>
	private bool inDecisionRange;

	protected AttackRange currentRange;

	public AttackRange CurrentRange
	{
		get
		{
			return currentRange;
		}
		set
		{
			if(currentRange != value)
			{
				currentCommand = EnemyCommands.None;
			}
			currentRange = value;
		}
	}


	/// <summary>
	/// Does the manager need to send this enemy a command
	/// </summary>
	public bool ManagerDecision
	{
		get
		{
			return (inDecisionRange && currentMove == null);
		}
	}
	
	// Use this for initialization
	public override void Start()
	{
		base.Start();
		//TODO calculate decisionRange

		active = false;
		xdistance = float.MaxValue;
		//temp decisionRange for test
		decisionRange = 7.5f;

		currentCommand = EnemyCommands.Idle;
	}

	protected override void EntityFixedUpdate()
	{
		if(currentMove != null)
		{
			return;
		}
		
	}

	protected virtual void Think()
	{

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
			EnemyDecision();

			switch (currentCommand)
			{
				case EnemyCommands.MoveIn:
					targetVelocity.x = facing * movementSpeed;
					break;
				case EnemyCommands.MakeSpace:
					targetVelocity.x = -facing * movementSpeed;
					break;
			}
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

	protected virtual void EnemyDecision()
	{
		/*
		switch (currentRange)
		{
			case AttackRange.Close =

		}*/
	}

	protected override void ExecuteCondition(LinkCondition condition)
	{
	}
}
