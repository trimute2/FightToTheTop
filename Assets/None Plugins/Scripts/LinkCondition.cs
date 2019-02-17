using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
	none,
	inputCondition,
	holdCondition,
	groundCondition,
	weaponCondition,
	weaponHoldCondition,
	AttackFlagCondition,
}

[System.Serializable]
[CreateAssetMenu(fileName = "Move", menuName = "Data/Link Condition", order = 1)]
public class LinkCondition : ScriptableObject {
	//TODO: later it might be better for this to just hold data and have the player do the checks, this is good for now
	public ConditionType conditionType;
	public string button;
	public string weapon;
	public bool boolSetting;
	public int holdNumber;
	//TODO: figure out how to properly set this
	public int buttonIndex;

	public bool BoolCondition(bool input)
	{
		return input && boolSetting;
	}

	public bool InputCondition(InputBuffer[] inputBuffers)
	{
		return InputCondition(inputBuffers, button);
	}

	public bool InputCondition(InputBuffer[] inputBuffers, string buttonInput)
	{
		for(int i = 0; i< inputBuffers.Length; i++)
		{
			if(inputBuffers[i].Button == buttonInput)
			{
				return inputBuffers[i].CanUse();
			}
		}
		return false;
	}

	public bool InputHoldConditon(InputBuffer[] inputBuffers)
	{
		return InputHoldConditon(inputBuffers, button);
	}


	public bool InputHoldConditon(InputBuffer[] inputBuffers, string buttonInput)
	{
		for (int i = 0; i < inputBuffers.Length; i++)
		{
			if (inputBuffers[i].Button == buttonInput)
			{
				return inputBuffers[i].Hold() == holdNumber;
			}
		}
		return false;
	}

	public bool WeaponCondition(InputBuffer[] inputBuffers, string weapon1, string weapon2)
	{
		if(weapon1 == weapon)
		{
			return InputCondition(inputBuffers, "Weapon1");
		}
		if(weapon2 == weapon)
		{
			return InputCondition(inputBuffers, "Weapon2");
		}
		return false;
	}

	public bool WeaponHoldCondition(InputBuffer[] inputBuffers, string weapon1, string weapon2)
	{
		if (weapon1 == weapon)
		{
			return InputHoldConditon(inputBuffers, "Weapon1");
		}
		if (weapon2 == weapon)
		{
			return InputHoldConditon(inputBuffers, "Weapon2");
		}
		return false;
	}

	public void ExecuteInput(InputBuffer[] inputBuffers, string buttonInput)
	{
		for (int i = 0; i < inputBuffers.Length; i++)
		{
			if (inputBuffers[i].Button == buttonInput)
			{
				inputBuffers[i].Execute();
			}
		}
	}

	public void ExecuteInput (InputBuffer[] inputBuffers)
	{
		ExecuteInput(inputBuffers, button);
	}

	public void ExecuteInput(InputBuffer[] inputBuffers, string weapon1, string weapon2)
	{
		if (weapon1 == weapon)
		{
			ExecuteInput(inputBuffers, "Weapon1");
		}else if (weapon2 == weapon)
		{
			ExecuteInput(inputBuffers, "Weapon2");
		}
	}
}