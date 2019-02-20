﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController {
	
	#region inputBufferVariables
	//TODO: change to dictionary 
	private InputBuffer[] inputBuffers;

	private string[] inputNames = { "Weapon1", "Weapon2", "Jump", "Dodge" };

	private const int WEAPON1INDEX = 0;
	private const int WEAPON2INDEX = 1;

	public string Weapon1;

	public string Weapon2;

	private int dodgeCount;

	private bool doubleJump;


	#endregion inputBufferVariables

	// Use this for initialization
	public override void Start () {
		base.Start();
		inputBuffers = new InputBuffer[inputNames.Length];
		for (int i = 0; i < inputBuffers.Length; i++)
		{
			inputBuffers[i] = new InputBuffer(inputNames[i]);
		}
		dodgeCount = 0;
	}

	protected override Vector2 EntityUpdate(Vector2 previousTarget)
	{
		if (Input.GetKey("escape"))
			Application.Quit();
		Vector2 animatorVec = Vector2.zero;
		Vector2 movementInput = Vector2.zero;
		movementInput.x = Input.GetAxisRaw("Horizontal");
		movementInput.y = Input.GetAxisRaw("Vertical");
		foreach (InputBuffer b in inputBuffers)
		{
			b.Update();
		}
		if((flagData.commonFlags & CommonFlags.CanTurn) != CommonFlags.None)
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
		}
		if ((flagData.commonFlags & CommonFlags.MoveWithInput) != CommonFlags.None)
		{
			targetVelocity.x = movementInput.x;
			if ((flagData.commonFlags & CommonFlags.YMovement) != CommonFlags.None)
			{
				targetVelocity.y = movementInput.y;
			}
			targetVelocity *= movementSpeed;
		}
		if ((flagData.commonFlags & CommonFlags.MovementCancel) != CommonFlags.None)
		{
			if(movementInput.x != 0)
			{
				EnterGenericState(0.3f);
			}
		}
		if((flagData.commonFlags & CommonFlags.Dodgeing) != CommonFlags.None)
		{
			if (targetVelocity == Vector2.zero)
			{
				if (previousTarget == Vector2.zero)
				{
					targetVelocity.x = Facing * movementSpeed;
					targetVelocity *= 3f;
				}
				else
				{
					targetVelocity = previousTarget;
				}
			}
			else {
				
				targetVelocity.y /= 2f;
				
				targetVelocity *=3f;
			}
			animatorVec = targetVelocity;
			animatorVec.x *= Facing;
			return animatorVec;
		}

		return base.EntityUpdate(previousTarget);

	}

	protected override bool TestCondition(LinkCondition condition)
	{
		switch (condition.conditionType)
		{
			case ConditionType.inputCondition:
				return inputBuffers[condition.buttonIndex].CanUse();
			case ConditionType.weaponCondition:
				if (Weapon1 == condition.weapon)
				{
					return inputBuffers[WEAPON1INDEX].CanUse();
				}
				else if (Weapon2 == condition.weapon)
				{
					return inputBuffers[WEAPON2INDEX].CanUse();
				}
				return false;
			case ConditionType.holdCondition:
				return inputBuffers[condition.buttonIndex].Hold() >= condition.holdNumber;
			case ConditionType.weaponHoldCondition:
				if (Weapon1 == condition.weapon)
				{
					return inputBuffers[WEAPON1INDEX].Hold() >= condition.holdNumber;
				}
				else if (Weapon2 == condition.weapon)
				{
					return inputBuffers[WEAPON2INDEX].Hold() >= condition.holdNumber;
				}
				return false;
			case ConditionType.AttackFlagCondition:
				return ((flagData.commonFlags & CommonFlags.CanAttack) != CommonFlags.None) == condition.boolSetting;
			case ConditionType.CanDodge:
				return dodgeCount < 2;
			default:
				return base.TestCondition(condition);
		}
	}

	protected override void EnterGenericState(float transitionTime = 0)
	{
		base.EnterGenericState(transitionTime);
		dodgeCount = 0;
	}

	protected override void ExecuteCondition(LinkCondition condition)
	{
		switch (condition.conditionType)
		{
			case ConditionType.inputCondition:
				inputBuffers[condition.buttonIndex].Execute();
				break;
			case ConditionType.weaponCondition:
				int ind = WEAPON1INDEX;
				if (Weapon2 == condition.weapon)
				{
					ind = WEAPON2INDEX;
				}
				inputBuffers[ind].Execute();
				break;
			case ConditionType.CanDodge:
				dodgeCount++;
				break;
			default:
				base.ExecuteCondition(condition);
				break;
		}
		/*
		if (condition.conditionType == ConditionType.inputCondition)
		{
			inputBuffers[condition.buttonIndex].Execute();
		}
		else if (condition.conditionType == ConditionType.weaponCondition)
		{
			int ind = WEAPON1INDEX;
			if (Weapon2 == condition.weapon)
			{
				ind = WEAPON2INDEX;
			}
			inputBuffers[ind].Execute();
		}*/
	}
}
