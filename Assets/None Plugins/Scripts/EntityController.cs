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

	protected float vulnrabilityTimer;

	/**<summary>The direction the player is facing</summary>*/
	protected int Facing = 1;
	/**<summary>The rigid body for the entity</summary>*/
	private Rigidbody2D rb2d;
	// im including this due to an issue with one of unitys features
	private Collider2D entityCollider;
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

	public bool Dodgeing
	{
		get
		{
			return (flagData.commonFlags & CommonFlags.Dodgeing) != CommonFlags.None;
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

	private bool listenToMoveMotion;

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
		entityCollider = GetComponent<Collider2D>();
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
		listenToMoveMotion = true;
		entityID = GameManager.Instance.GetNewId();
		vulnrabilityTimer = 0;
	}

	#region fixedUpdateFunctions
	private void FixedUpdate()
	{
		//so far there are no situation where i would want to keep an xvelocity but that may change later
		if (vulnrabilityTimer == 0)
		{
			velocity.x = 0;
		}
		if ((flagData.commonFlags & CommonFlags.YMovement) == CommonFlags.None)
		{
			velocity += Physics2D.gravity * Time.deltaTime;
		}
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
		int count = entityCollider.Cast(move, contactFilter, hitBuffer, distance + 0.01f);
		hitBufferList.Clear();
		bool dodgeing = (flagData.commonFlags & CommonFlags.Dodgeing) != CommonFlags.None;
		for(int i = 0; i < count; i++)
		{
			bool add = true;
			if (hitBuffer[i].collider.tag == "Entity")
			{
				if (dodgeing|| hitBuffer[i].collider.GetComponent<EntityController>().Dodgeing)
				{
					add = false;
				}
			}
			if (add)
			{
				hitBufferList.Add(hitBuffer[i]);
			}
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
				if (vulnrabilityTimer != 0 && currentNormal.x != 0)
				{
					velocity.x *= -0.75f;
				}
				else
				{
					velocity -= projection * currentNormal;
				}
			}

			float modifiedDistance = hitBufferList[i].distance - 0.01f;
			distance = modifiedDistance < distance ? modifiedDistance : distance;
		}
		rb2d.position = rb2d.position + move.normalized * distance;
	}

	//will flesh this out later, maybe add stun and knock back
	public virtual void Damage(int damage, Vector2 knockBack)
	{
		health -= damage;
		EnterGenericState();
		vulnrabilityTimer = 0.3f;
		string toPlay = "Damage";
		if(knockBack != Vector2.zero)
		{
			vulnrabilityTimer = 0.7f;
			toPlay = "Knock Back";
			velocity = knockBack;
		}
		animator.CrossFade(toPlay, 0.001111111f);
	}
	#endregion fixedUpdateFunctions

	#region updateFunctions
	// Update is called once per frame
	void Update () {
		Vector2 previousTarget = targetVelocity;
		targetVelocity = Vector2.zero;
		Vector2 animVel = EntityUpdate(previousTarget);
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
			CheckMoves();
		}
		animator.SetFloat("VelocityX", animVel.x);
		animator.SetFloat("VelocityY", animVel.y);
		//Debug.Log(1 / Time.deltaTime);
	}

	//maybe I will make entity abstract as well as this by extension
	protected virtual Vector2 EntityUpdate(Vector2 previousTarget)
	{
		return new Vector2(Mathf.Abs(targetVelocity.x), velocity.y);
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
			StartMove(links[nextMoveIndex].move);
			/*
			currentMove = links[nextMoveIndex].move;
			moveTime = -1;
			animator.Play(currentMove.animationStateName);
			animator.speed = currentMove.playBackSpeed;*/
		}
	}

	public virtual void StartMove(MoveData move)
	{
		if (move == null)
		{
			EnterGenericState();
		}
		else
		{
			currentMove = move;
			moveTime = -1;
			listenToMoveMotion = true;
			foreach (EntityEffects e in move.EffectsOnEnter)
			{
				e.Effect(this);
			}
			animator.Play(currentMove.animationStateName);
			animator.speed = currentMove.playBackSpeed;
		}
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
			

			if (listenToMoveMotion && entityValueFlags != ValueFlags.None)
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

	protected virtual void EnterGenericState(float transitionTime = 0)
	{
		if (currentMove != null)
		{
			foreach (EntityEffects e in currentMove.EffectsOnExit)
			{
				e.Effect(this);
			}
		}
		string toPlay;
		if (grounded)
		{
			//animator.Play("Idle");
			toPlay = "Idle";
		}
		else
		{
			if (velocity.y < 0)
			{
				toPlay = "Falling";
				//animator.Play("Falling");
			}
			else
			{
				toPlay = "GoingUp";
				//animator.Play("GoingUp");
			}
		}
		//animator.Play(toPlay);
		animator.CrossFade(toPlay, transitionTime);
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
		HitBoxes[HitboxIndex].EnableHitBox(currentMove.damage,currentMove.knockBack);
	}

	public void DeactivateHitBox(int HitboxIndex)
	{
		HitBoxes[HitboxIndex].DisableHitBox();
	}

	public virtual void SetVelocity(Vector2 vector)
	{
		listenToMoveMotion = false;
		targetVelocity = vector;
	}


	//TODO: should make interface to handle damage and such may do later
	public virtual void HitEnemy(EntityController target)
	{
		foreach(EntityEffects effect in currentMove.HitTargetEffects)
		{
			effect.Effect(target);
		}
		foreach (EntityEffects effect in currentMove.HitUserEffects)
		{
			effect.Effect(this);
		}
	}

	public void SpawnVisualEffect(Vector2 vector)
	{
		if (currentMove.HitVisualEffect != null)
		{
			Instantiate(currentMove.HitVisualEffect, new Vector3(vector.x, vector.y, 0), Quaternion.identity);
		}
	}
}
