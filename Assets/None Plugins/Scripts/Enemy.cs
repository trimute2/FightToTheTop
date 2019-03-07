using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityController {
	//the structure of enemy is currently unorganized
	//currently changing it so that it will be organized around this tutorial
	//https://gamedevelopment.tutsplus.com/tutorials/battle-circle-ai-let-your-player-feel-like-theyre-fighting-lots-of-enemies--gamedev-13535
	private bool active;
	public float longRange;
	public float midRange;
	public float closeRange;
	public List<MoveLink> enemyMoves;
	//does this enemy have permission to go to close 
	private int targetRange = Target.LONG_RANGE;
	public int TargetRange
	{
		get
		{
			return targetRange;
		}
	}
	protected int currentTargetRange = Target.OUT_RANGE;
	protected Avoider avoider;
	private Vector3 avoidVec = Vector3.zero;
	protected bool attackPermission = false;

	public float baseAgression;
	private float agro = 0;

	protected bool shouldAvoid = false;
	protected float AttackTime;

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
	protected Target target;

	/// <summary>
	/// The Target of the enemies attacks
	/// </summary>
	public Target CurrentTarget
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

	public int Direction
	{
		get
		{
			return (int)Mathf.Sign(xdistance);
		}
	}
	
	// Use this for initialization
	public override void Start()
	{
		base.Start();
		active = false;
		xdistance = float.MaxValue;
		avoider = GetComponentInChildren<Avoider>();
		AttackTime = Time.time;
		Test();
	}

	protected virtual void Test()
	{

	}

	protected override Vector2 EntityUpdate(Vector2 previousTarget)
	{
		targetVelocity = Vector2.zero;
		if (!active || target == null)
		{
			//if the enemy is not active or the target has not been set dont do anything
			return Vector2.zero;
		}
		//update distance to target
		xdistance = target.transform.position.x - transform.position.x;
		int previousRange = currentTargetRange;
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
		float distance = Mathf.Abs(xdistance);
		if (distance <= closeRange)
		{
			currentTargetRange = Target.CLOSE_RANGE;
		}
		else if (distance <= midRange)
		{
			currentTargetRange = Target.MID_RANGE;
		}
		else if (distance <= longRange)
		{
			currentTargetRange = Target.LONG_RANGE;
		}
		else
		{
			currentTargetRange = Target.OUT_RANGE;
		}
		if (currentTargetRange != previousRange)
		{
			target.SwapRanges(previousRange, currentTargetRange, this);
		}
		if (vulnrabilityTimer != 0)
		{
			vulnrabilityTimer -= Time.deltaTime;
			if (vulnrabilityTimer <= 0)
			{
				vulnrabilityTimer = 0;
				velocity.x = 0;
				EnterGenericState();
			}
		}
		else
		{
			if (currentTargetRange != Target.OUT_RANGE)
			{
				//set variables on what to do
				EnemyDecision();
				UpdateAvoidVec();
				if (targetRange != currentTargetRange)
				{
					if (targetRange < currentTargetRange)
					{
						targetVelocity.x = facing * movementSpeed;
					}
					else if (targetRange > currentTargetRange)
					{
						targetVelocity.x = -facing * movementSpeed;
					}
				}
				else
				{
					if (attackPermission && currentMove == null)
					{
						CheckMoves();
					}
					else
					{
						if (shouldAvoid&&avoidVec != Vector3.zero)
						{
							targetVelocity.x = avoidVec.x;
						}
					}
				}
			}
			else
			{
				targetVelocity.x = facing * movementSpeed;
			}
		}

		return new Vector2(Mathf.Abs(targetVelocity.x), velocity.y);
	}

	// Update is called once per frame
	protected override void CheckMoves()
	{
		List<MoveLink> links = new List<MoveLink>();
		links.AddRange(enemyMoves);
		if (currentMove != null)
		{
			float moveTime = MoveTime;
			foreach (MoveLink l in currentMove.links)
			{
				if ((l.minTime <= moveTime) && (l.maxTime >= moveTime))
				{
					links.Add(l);
				}
			}
		}
		int nextMoveIndex = -1;
		int priority = -100;
		for (int i = 0; i < links.Count; i++)
		{
			MoveLink currentLink = links[i];
			if (currentLink.priority > priority)
			{
				bool meetsConditions = false;
				for (int j = 0; j < currentLink.conditions.Count; j++)
				{
					LinkCondition condition = currentLink.conditions[j];
					meetsConditions = TestCondition(condition);
					if (!meetsConditions)
					{
						break;
					}
				}
				if (meetsConditions)
				{
					nextMoveIndex = i;
					priority = currentLink.priority;
				}
			}
		}
		if (nextMoveIndex != -1)
		{
			foreach (LinkCondition l in links[nextMoveIndex].conditions)
			{
				ExecuteCondition(l);
			}
			StartMove(links[nextMoveIndex].move);
		}
	}

	protected override bool TestCondition(LinkCondition condition)
	{
		switch (condition.conditionType)
		{
			case ConditionType.RangeCondition:
				if(target != null)
				{
					// I should probably rename button index to something else
					return currentTargetRange == condition.buttonIndex;
				}
				return false;
			case ConditionType.AttackTimeCondition:
				if(target != null)
				{
					return target.LastAttack(condition.buttonIndex) > condition.TimeCondition;
				}
				return false;
			case ConditionType.MoveTimeCondition:
				return (Time.time - AttackTime) > condition.TimeCondition;
			default:
				return base.TestCondition(condition);
		}
	}

	protected virtual void EnemyDecision()
	{
		targetRange = Target.MID_RANGE;
	}

	//TODO: either add more functinallity to this or make target range protected
	protected virtual void SetTargetRange(int Range)
	{
		targetRange = Range;
	}

	protected void UpdateAvoidVec()
	{
		if(avoider != null && avoider.avoidTransform != null)
		{
			avoidVec = transform.position - avoider.avoidTransform.position;
			if (avoidVec == Vector3.zero)
			{
				avoidVec.x = Random.Range(-1, 1);
			}
			avoidVec.Normalize();
		}
		else
		{
			avoidVec = Vector3.zero;
		}
	}

	public float GetTension()
	{
		//TODO: add Tension calculations, requires certain stuff I have yet to add in yet
		//for now this is a temp variable
		if (currentMove != null && currentMove.GetType() == typeof(EnemyMoveData))
		{
			return ((EnemyMoveData)currentMove).range[currentTargetRange];
		}
		return 12.0f;
	}

	protected override void ExecuteCondition(LinkCondition condition)
	{
		if(condition.conditionType == ConditionType.RangeCondition && target != null)
		{
			target.StartAttack(condition.buttonIndex);
		}
	}

	private void OnDrawGizmos()
	{
		if(target != null)
		{
			Gizmos.DrawWireSphere(target.transform.position, closeRange);
			Gizmos.DrawWireSphere(target.transform.position, midRange);
			Gizmos.DrawWireSphere(target.transform.position, longRange);
		}
	}

	protected override void EnterGenericState(float transitionTime = 0)
	{
		AttackTime = Time.time;
		base.EnterGenericState(transitionTime);
	}
}
