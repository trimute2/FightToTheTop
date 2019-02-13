using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

	public float movementSpeed = 4.5f;


	/**<summary>The direction the player is facing</summary>*/
	private int Facing = 1;
	/**<summary>The rigid body for the entity</summary>*/
	private Rigidbody2D rb2d;
	/**<summary>The animator for the entity</summary>*/
	private Animator animator;
	private FlagData flagData;
	//private CommonFlags controllerFlags;
	private ValueFlags entityValueFlags;


	#region inputBufferVariables
	//TODO: change to dictionary 
	private InputBuffer[] inputBuffers;

	private string[] inputNames = { "Weapon1", "Weapon2", "Jump", "Dodge" };

	private const int WEAPON1INDEX = 0;
	private const int WEAPON2INDEX = 1;

	#endregion inputBufferVariables
	// physics variables
	private ContactFilter2D contactFilter;
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

	private Vector2 velocity;
	private Vector2 targetVelocity;

	private bool grounded;

	private float moveTime;

	private MoveData currentMove;

	public MoveData test;

	public string Weapon1;

	public string Weapon2;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		flagData = new FlagData(CommonFlags.MoveWithInput, ValueFlags.None);
		//controllerFlags = CommonFlags.MoveWithInput;
		entityValueFlags = ValueFlags.None;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
		currentMove = null;
		inputBuffers = new InputBuffer[inputNames.Length];
		for(int i = 0; i < inputBuffers.Length; i++)
		{
			inputBuffers[i] = new InputBuffer(inputNames[i]);
		}
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
		int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + 0.01f);
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

	#endregion fixedUpdateFunctions

	#region updateFunctions
	// Update is called once per frame
	void Update () {
		targetVelocity = Vector2.zero;
		Vector2 movementInput = Vector2.zero;
		movementInput.x = Input.GetAxisRaw("Horizontal");
		movementInput.y = Input.GetAxisRaw("Vertical");
		foreach(InputBuffer b in inputBuffers)
		{
			b.Update();
		}
		if ((flagData.commonFlags & CommonFlags.MoveWithInput) != CommonFlags.None)
		{
			
			if (movementInput.x != 0)
			{
				Vector3 sca = transform.localScale;
				if (movementInput.x > 0)
				{
					sca.x = 1;
					Facing = 1;
				}
				else
				{
					sca.x = -1;
					Facing = -1;
				}
				transform.localScale = sca;
			}
			targetVelocity.x = movementInput.x * movementSpeed;
		}
		
		if (Input.GetButtonDown("Fire1") && currentMove == null)
		{
			currentMove = test;
			animator.Play(currentMove.animationStateName);
			//Debug.Log(currentMove.test().GetType());
			//Debug.Log(currentMove.test());
		}
		animator.SetFloat("VelocityX", Mathf.Abs(targetVelocity.x));
		animator.SetFloat("VelocityY", velocity.y);
		//Debug.Log(1 / Time.deltaTime);
	}


	private void CheckMoves()
	{
		List<MoveLink> links = new List<MoveLink>();

		int nextMoveIndex = -1;
		int priority = 0;
		for(int i = 0; i< links.Count; i++)
		{
			MoveLink currentLink = links[i];
			if (currentLink.priority > priority)
			{
				bool meetsConditions = false;
				for (int j = 0; j < currentLink.conditions.Count; j++)
				{
					LinkCondition condition = currentLink.conditions[j];
					switch (condition.conditionType)
					{
						case ConditionType.inputCondition:
							meetsConditions = inputBuffers[condition.buttonIndex].CanUse();
							break;
						case ConditionType.groundCondition:
							meetsConditions = grounded == condition.boolSetting;
							break;
						case ConditionType.weaponCondition:
							if(Weapon1 == condition.weapon)
							{
								meetsConditions = inputBuffers[WEAPON1INDEX].CanUse();
							}else if(Weapon2 == condition.weapon)
							{
								meetsConditions = inputBuffers[WEAPON2INDEX].CanUse();
							}
							else
							{
								meetsConditions = false;
							}
							break;
						case ConditionType.holdCondition:
							meetsConditions = inputBuffers[condition.buttonIndex].Hold() >= condition.holdNumber;
							break;
						case ConditionType.weaponHoldCondition:
							if (Weapon1 == condition.weapon)
							{
								meetsConditions = inputBuffers[WEAPON1INDEX].Hold() >= condition.holdNumber;
							}
							else if (Weapon2 == condition.weapon)
							{
								meetsConditions = inputBuffers[WEAPON2INDEX].Hold() >= condition.holdNumber;
							}
							else
							{
								meetsConditions = false;
							}
							break;
					}
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
				if(l.conditionType == ConditionType.inputCondition)
				{
					inputBuffers[l.buttonIndex].Execute();
				}else if(l.conditionType == ConditionType.weaponCondition)
				{
					int ind = WEAPON1INDEX;
					if(Weapon2 == l.weapon)
					{
						ind = WEAPON2INDEX;
					}
					inputBuffers[ind].Execute();
				}
			}
		}
	}

	/* old version
	private void CheckMoves()
	{
		List<MoveLink> links = new List<MoveLink>();
		//TODO: fill out list of links
		//right now the higher priority is the larger number
		int nextMoveIndex = -1;
		int priority = 0;
		for(int i = 0; i< links.Count; i++)
		{
			MoveLink currentLink = links[i];
			if(currentLink.priority > priority)
			{
				bool meetsConditions = true;
				for(int j = 0; j < currentLink.conditions.Count; j++)
				{
					LinkCondition condition = currentLink.conditions[j];
					//TODO: possible system to make sure the same condition is not tested twice
					//have another class keep a list of conditions, and if they were checked this frame
					//as well as there result
					switch (condition.conditionType)
					{
						case ConditionType.groundCondition:
							meetsConditions = condition.BoolCondition(grounded);
							break;
						case ConditionType.inputCondition:
							meetsConditions = condition.InputCondition(inputBuffers);
							break;
					}
					if (!meetsConditions)
					{
						break;
					}
				}
				if (meetsConditions)
				{
					priority = currentLink.priority;
					nextMoveIndex = i;
				}
			}
		}
		if(nextMoveIndex != -1)
		{
			//TODO: start next move
			foreach(LinkCondition l in links[nextMoveIndex].conditions)
			{
				
			}
		}
	}*/



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
				if (getValue(ValueFlags.xVelocity, out val))
				{
					targetVelocity.x = val * Facing;
				}
				if(getValue(ValueFlags.yVelocity, out val))
				{
					targetVelocity.y = val;
				}
			}
			if (currentMove.EndMove(moveTime))
			{
				animator.Play("Idle");
				moveTime = -1;
				currentMove = null;
			}
		}
	}

	private bool getValue(ValueFlags flag, out float value)
	{
		if((entityValueFlags & flag) != ValueFlags.None)
		{
			value = currentMove.GetAnimatedValue(moveTime, flag);
			return true;
		}
		value = 0;
		return false;
	}

	/*
	private int UpdateFlags<T>(T flags)
	{

	}*/
}
