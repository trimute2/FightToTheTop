﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlagHandler))]
[RequireComponent(typeof(Animator))]
public class MoveHandler : MonoBehaviour {
	public delegate void GenericStateListner();

	public event GenericStateListner GenericStateEvent;

	public int maxDodge;

	private int dodgeCount;

	public List<HitBoxScript> HitBoxes;

	private CommonFlags defaultFlagValues = CommonFlags.CanTurn | CommonFlags.MoveWithInput | CommonFlags.CanAttack;
	private FlagHandler flagHandler;
	private EntityControllerComp entityController;
	private Animator animator;
	private PlayerInputHandler playerInput;
	/// <summary>
	/// The current move
	/// </summary>
	private MoveData currentMove;

	private float moveTime;
	private float overDodge;
	public float OverDodge
	{
		get
		{
			return overDodge;
		}
	}

	private bool listenToMoveMotion;

	public bool GenericState
	{
		get
		{
			return currentMove == null;
		}
	}

	// Use this for initialization
	void Awake () {
		currentMove = null;
		animator = GetComponent<Animator>();
		flagHandler = GetComponent<FlagHandler>();
		entityController = GetComponent<EntityControllerComp>();
		playerInput = GetComponent<PlayerInputHandler>();
		flagHandler.Flags = new FlagData(defaultFlagValues, ValueFlags.None);
		moveTime = 0;
		dodgeCount = 0;
		overDodge = 0;
		listenToMoveMotion = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (entityController != null)
		{
			Vector2 velocity = entityController.Velocity;
			animator.SetFloat("VelocityX", velocity.x * entityController.Facing);
			animator.SetFloat("VelocityY", velocity.y);
		}
	}

	public void CheckMoves(List<MoveLink> moveLinks)
	{
		List<MoveLink> links = new List<MoveLink>();
		links.AddRange(moveLinks);
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
		for (int i = 0; i < links.Count; i++)
		{
			MoveLink currentLink = links[i];
			if (currentLink.priority > priority)
			{
				bool meetsConditions = false;
				for (int j = 0; j < currentLink.conditions.Count; j++)
				{
					LinkCondition condition = currentLink.conditions[j];
					meetsConditions = CheckCondition(condition);
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

	private bool CheckCondition(LinkCondition condition)
	{
		switch (condition.conditionType)
		{
			case ConditionType.inputCondition:
				return (playerInput != null && playerInput[condition.buttonIndex].CanUse());
			case ConditionType.holdCondition:
				return (playerInput != null && playerInput[condition.buttonIndex].Hold() >= condition.holdNumber);
			case ConditionType.releaseCondition:
				return (playerInput != null && playerInput[condition.buttonIndex].Hold() <= 0);
			case ConditionType.groundCondition:
				if(entityController != null)
				{
					return entityController.Grounded == condition.boolSetting;
				}
				return true;
			case ConditionType.weaponCondition:
				//TODO: make equipment class and change this
				if (playerInput != null)
				{
					if (playerInput.Weapon1 == condition.weapon)
					{
						return playerInput[PlayerInputHandler.WEAPON1INDEX].CanUse();
					}
					else if (playerInput.Weapon2 == condition.weapon)
					{
						return playerInput[PlayerInputHandler.WEAPON2INDEX].CanUse();
					}
				}
				return false;
			case ConditionType.weaponHoldCondition:
				if (playerInput != null)
				{
					if (playerInput.Weapon1 == condition.weapon)
					{
						return playerInput[PlayerInputHandler.WEAPON1INDEX].Hold() >= condition.holdNumber;
					}
					else if (playerInput.Weapon2 == condition.weapon)
					{
						return playerInput[PlayerInputHandler.WEAPON2INDEX].Hold() >= condition.holdNumber;
					}
				}
				return false;
			case ConditionType.AttackFlagCondition:
				return ((flagHandler.CommonFlags & CommonFlags.CanAttack) != CommonFlags.None) == condition.boolSetting;
			case ConditionType.CanDodge:
				return (maxDodge <= 0 || dodgeCount < maxDodge);
			default:
				return false;
		}
	}

	public void ExecuteCondition(LinkCondition condition)
	{
		switch (condition.conditionType)
		{
			case ConditionType.inputCondition:
				if (playerInput != null)
				{
					playerInput[condition.buttonIndex].Execute();
				}
				break;
			case ConditionType.weaponCondition:
				if (playerInput != null) {
					int ind = PlayerInputHandler.WEAPON1INDEX;
					if (playerInput.Weapon2 == condition.weapon)
					{
						ind = PlayerInputHandler.WEAPON2INDEX;
					}
					playerInput[ind].Execute();
				}
				break;
			case ConditionType.CanDodge:
				dodgeCount++;
				break;
		}
	}

	public bool CheckLink(MoveLink link)
	{
		foreach(LinkCondition lk in link.conditions)
		{
			if (!CheckCondition(lk))
			{
				return false;
			}
		}
		return true;
	}

	public void EnterGenericState(float transitionTime = 0)
	{
		if (((flagHandler.CommonFlags & CommonFlags.Dodgeing) != CommonFlags.None) &&
			entityController != null && !entityController.TestOverlap())
		{
			overDodge += Time.deltaTime;
			return;
		}
		overDodge = 0;
		dodgeCount = 0;
		if (currentMove != null)
		{
			foreach (EntityEffects e in currentMove.EffectsOnExit)
			{
				e.Effect(this);
			}
		}
		string toPlay;
		if(entityController == null || entityController.Grounded)
		{
			toPlay = "Idle";
		}else if(entityController.Velocity.y < 0)
		{
			toPlay = "Falling";
		}
		else
		{
			toPlay = "GoingUp";
		}
		animator.CrossFade(toPlay, transitionTime);
		flagHandler.ValueFlags = ValueFlags.None;
		flagHandler.CommonFlags = defaultFlagValues;
		animator.speed = 1;

		currentMove = null;
		for (int i = 0; i < HitBoxes.Count; i++)
		{
			HitBoxes[i].DisableHitBox();
		}

		if (GenericStateEvent != null)
		{
			GenericStateEvent();
		}
	}

	public void StartMove(MoveData move)
	{
		if(move == null)
		{
			EnterGenericState();
		}
		else
		{
			if (currentMove != null)
			{
				foreach (EntityEffects e in currentMove.EffectsOnExit)
				{
					e.Effect(this);
				}
			}
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

	private void LateUpdate()
	{
		if (currentMove != null)
		{
			FlagData flagData = flagHandler.Flags;
			moveTime = moveTime == -1 ? 0 : moveTime + Time.deltaTime;
			//get the state of the flags tracked by the move
			CommonFlags moveFlags = (CommonFlags)currentMove.GetActiveFlags(moveTime, FlagTypes.CommonFlags);
			//activate the flags that are active according to the move
			//controllerFlags |= moveFlags;
			flagData.commonFlags |= moveFlags;
			//in the moveflags variable activate all flags not tracked by the move
			moveFlags |= (CommonFlags)~currentMove.GetTrackedFlags(FlagTypes.CommonFlags);
			//turn off all flags not active acording to the move while leaving those not tracked by the move in their original state
			//controllerFlags &= moveFlags;
			flagData.commonFlags &= moveFlags;

			flagData.valueFlags = (ValueFlags)currentMove.GetActiveFlags(moveTime, FlagTypes.ValueFlags);

			flagHandler.Flags = flagData;
			if (listenToMoveMotion && entityController != null && flagHandler.ValueFlags != ValueFlags.None)
			{
				Vector2 targetVelocity = entityController.TargetVelocity;
				float val = 0;
				if (GetValue(ValueFlags.xVelocity, out val))
				{
					
					targetVelocity.x = val * entityController.Facing;
				}
				if (GetValue(ValueFlags.yVelocity, out val))
				{
					targetVelocity.y = val;
				}
				entityController.TargetVelocity = targetVelocity;
			}
			if (currentMove.EndMove(moveTime))
			{
				EnterGenericState();
			}
		}
		if (entityController != null)
		{
			entityController.AllowEntityCollision = (flagHandler.CommonFlags & CommonFlags.Dodgeing) == CommonFlags.None;
		}
	}

	private bool GetValue(ValueFlags flag, out float value)
	{
		if ((flagHandler.ValueFlags & flag) != ValueFlags.None)
		{
			value = currentMove.GetAnimatedValue(moveTime, flag);
			return true;
		}
		value = 0;
		return false;
	}

	#region EffectFunctions
	public void AddForce(Vector2 force)
	{
		//TODO: add force in move Handler
		throw new System.NotImplementedException();
	}

	public void SetVelocity(Vector2 vector)
	{
		listenToMoveMotion = false;
		if (entityController != null)
		{
			entityController.TargetVelocity = vector;
		}
	}

	public void TurnCommonFlagsOff(CommonFlags flags)
	{
		flagHandler.TurnCommonFlagsOff(flags);
	}

	public void TurnCommonFlagsOn(CommonFlags flags)
	{
		flagHandler.TurnCommonFlagsOn(flags);
	}

	public void SetGravity(bool gravity)
	{
		if(entityController != null)
		{
			entityController.GravityOn = gravity;
		}
	}

	#endregion EffectFunctions
	public void ActivateHitBox(int HitboxIndex)
	{
		if (currentMove != null)
		{
			HitBoxes[HitboxIndex].EnableHitBox(currentMove.damage, currentMove.knockBack);
		}
	}

	public void DeactivateHitBox(int HitboxIndex)
	{
		HitBoxes[HitboxIndex].DisableHitBox();
	}
}
