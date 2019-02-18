using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EntityController : MonoBehaviour {

	public float movementSpeed = 4.5f;
	public int maxHealth = 100;

	private int health;


	/**<summary>The direction the player is facing</summary>*/
	protected int Facing = 1;
	/**<summary>The rigid body for the entity</summary>*/
	private Rigidbody2D rb2d;
	// im including this due to an issue with one of unitys features
	private Collider2D collider;
	/**<summary>The animator for the entity</summary>*/
	private Animator animator;
	protected FlagData flagData;
	//private CommonFlags controllerFlags;
	private ValueFlags entityValueFlags;
	//will set this up later
	private int entityID;

	public int EntityID
	{
		get
		{
			return entityID;
		}
	}

	#region inputBufferVariables
	//TODO: change to dictionary 
	//private InputBuffer[] inputBuffers;

	//private string[] inputNames = { "Weapon1", "Weapon2", "Jump", "Dodge" };

	//private const int WEAPON1INDEX = 0;
	//private const int WEAPON2INDEX = 1;

	#endregion inputBufferVariables
	// physics variables
	private ContactFilter2D contactFilter;
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

	private Vector2 velocity;
	protected Vector2 targetVelocity;

	private bool grounded;

	private float moveTime;

	private MoveData currentMove;

	public MoveData test;

	public List<HitBoxScript> HitBoxes;

	//public string Weapon1;

	//public string Weapon2;

	public List<MoveLink> defaultMoves;

	// Use this for initialization
	public virtual void Start () {
		Facing = (int)gameObject.transform.localScale.x;
		rb2d = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();
		flagData = new FlagData((CommonFlags.MoveWithInput | CommonFlags.CanTurn | CommonFlags.CanAttack), ValueFlags.None);
		//controllerFlags = CommonFlags.MoveWithInput;
		entityValueFlags = ValueFlags.None;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		//LayerMask mask1 = LayerMask.NameToLayer("PlayerHurtBoxes");
		//contactFilter.SetLayerMask(mask1);
		contactFilter.useLayerMask = true;
		currentMove = null;
		health = maxHealth;
		entityID = GameManager.Instance.GetNewId();
	}

	#region fixedUpdateFunctions
	private void FixedUpdate()
	{
		velocity += Physics2D.gravity * Time.deltaTime;
		Vector2 deltaPosition = velocity;
		deltaPosition += targetVelocity;
		deltaPosition *= Time.deltaTime;
		grounded = false;
		Movement(deltaPosition * Vector2.right);
		Movement(deltaPosition * Vector2.up);
	}

	void Movement(Vector2 move)
	{
		/* im copying this over from the prototype, a lot of it is based on a
		 tutorial for a platformer so I will mostlikely need to change parts of
		 it later*/
		float distance = move.magnitude;
		//int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + 0.01f);
		int count = collider.Cast(move, contactFilter, hitBuffer, distance + 0.01f);
		hitBufferList.Clear();
		for(int i = 0; i < count; i++)
		{
			hitBufferList.Add(hitBuffer[i]);
		}

		for(int i = 0; i < hitBufferList.Count; i++)
		{
			Vector2 currentNormal = hitBufferList[i].normal;
			if(currentNormal.y > 0.65f)
			{
				grounded = true;
			}
			float projection = Vector2.Dot(velocity, currentNormal);
			if(projection< 0)
			{
				velocity -= projection * currentNormal;
			}

			float modifiedDistance = hitBufferList[i].distance - 0.01f;
			distance = modifiedDistance < distance ? modifiedDistance : distance;
		}
		rb2d.position = rb2d.position + move.normalized * distance;
	}

	//will flesh this out later, maybe add stun and knock back
	public virtual void Damage(int damage)
	{
		health -= damage;
		EnterGenericState();
		animator.CrossFade("Damage", 0.001111111f);
	}
	#endregion fixedUpdateFunctions

	#region updateFunctions
	// Update is called once per frame
	void Update () {
		targetVelocity = Vector2.zero;
		EntityUpdate();
		CheckMoves();
		animator.SetFloat("VelocityX", Mathf.Abs(targetVelocity.x));
		animator.SetFloat("VelocityY", velocity.y);
		//Debug.Log(1 / Time.deltaTime);
	}

	//maybe I will make entity abstract as well as this by extension
	protected virtual void EntityUpdate()
	{

	}

	private void CheckMoves()
	{
		List<MoveLink> links = new List<MoveLink>();
		links.AddRange(defaultMoves);
		if (currentMove != null)
		{
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
		for(int i = 0; i< links.Count; i++)
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
		if(nextMoveIndex != -1)
		{
			foreach(LinkCondition l in links[nextMoveIndex].conditions)
			{
				ExecuteCondition(l);
			}
			currentMove = links[nextMoveIndex].move;
			moveTime = -1;
			animator.Play(currentMove.animationStateName);
			animator.speed = currentMove.playBackSpeed;
		}
	}

	public virtual void StartMove(MoveData move)
	{
		currentMove = move;
		moveTime = -1;
		animator.Play(currentMove.animationStateName);
		animator.speed = currentMove.playBackSpeed;
	}

	protected virtual bool TestCondition(LinkCondition condition)
	{
		//in process of generalizing to all entities and this is the only condition type i could think of that applied to all
		return (condition.conditionType == ConditionType.groundCondition) && (grounded == condition.boolSetting);
		/*
		switch(condition.conditionType)
		{
			case ConditionType.groundCondition:
				return grounded == condition.boolSetting;
		}
		return false*/
	}

	protected virtual void ExecuteCondition(LinkCondition condition)
	{

	}

	#endregion updateFunctions
	//late update is called once per frame after the internal animation update
	private void LateUpdate()
	{
		if(currentMove != null)
		{
			moveTime = moveTime == -1 ? 0 : moveTime + Time.deltaTime;
			//get the state of the flags tracked by the move
			CommonFlags moveFlags = (CommonFlags)currentMove.GetActiveFlags(moveTime,FlagTypes.CommonFlags);
			//activate the flags that are active according to the move
			//controllerFlags |= moveFlags;
			flagData.commonFlags |= moveFlags;
			//in the moveflags variable activate all flags not tracked by the move
			moveFlags |= (CommonFlags)~currentMove.GetTrackedFlags(FlagTypes.CommonFlags);
			//turn off all flags not active acording to the move while leaving those not tracked by the move in their original state
			//controllerFlags &= moveFlags;
			flagData.commonFlags &= moveFlags;

			entityValueFlags = (ValueFlags)currentMove.GetActiveFlags(moveTime,FlagTypes.ValueFlags);
			

			if (entityValueFlags != ValueFlags.None)
			{
				float val = 0;
				if (GetValue(ValueFlags.xVelocity, out val))
				{
					targetVelocity.x = val * Facing;
				}
				if(GetValue(ValueFlags.yVelocity, out val))
				{
					targetVelocity.y = val;
				}
			}
			if (currentMove.EndMove(moveTime))
			{
				EnterGenericState();
			}
		}
	}

	protected virtual void EnterGenericState()
	{
		if (grounded)
		{
			animator.Play("Idle");
		}
		else
		{
			animator.Play("Falling");
		}
		flagData.valueFlags = ValueFlags.None;
		flagData.commonFlags = CommonFlags.CanTurn | CommonFlags.MoveWithInput | CommonFlags.CanAttack; 
		animator.speed = 1;
		moveTime = -1;
		currentMove = null;
		for(int i = 0; i < HitBoxes.Count; i++)
		{
			HitBoxes[i].DisableHitBox();
		}
	}

	private bool GetValue(ValueFlags flag, out float value)
	{
		if((entityValueFlags & flag) != ValueFlags.None)
		{
			value = currentMove.GetAnimatedValue(moveTime, flag);
			return true;
		}
		value = 0;
		return false;
	}

	public void AddForce(Vector2 force)
	{
		velocity += force;
	}

	public void ActivateHitBox(int HitboxIndex)
	{
		HitBoxes[HitboxIndex].EnableHitBox();
	}

	public void DeactivateHitBox(int HitboxIndex)
	{
		HitBoxes[HitboxIndex].DisableHitBox();
	}
}
