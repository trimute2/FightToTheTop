using System.Collections;
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

	#endregion inputBufferVariables

	// Use this for initialization
	public override void Start () {
		base.Start();
		inputBuffers = new InputBuffer[inputNames.Length];
		for (int i = 0; i < inputBuffers.Length; i++)
		{
			inputBuffers[i] = new InputBuffer(inputNames[i]);
		}
	}

	protected override void EntityUpdate()
	{
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
			targetVelocity.x = movementInput.x * movementSpeed;
		}
		if ((flagData.commonFlags & CommonFlags.MovementCancel) != CommonFlags.None)
		{
			if(targetVelocity.x != 0)
			{
				EnterGenericState();
			}
		}


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
			default:
				return base.TestCondition(condition);
		}
	}

	protected override void ExecuteCondition(LinkCondition condition)
	{
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
		}
	}
}
