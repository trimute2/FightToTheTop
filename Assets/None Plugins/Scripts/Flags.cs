using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnumFlagsAttribute : PropertyAttribute
{
}

[System.Flags]
public enum CommonFlags
{
	None = 0,					//0000
	/**<summary>when active the controller changes velocity based off of input</summary>*/
	MoveWithInput = 1 << 0,     //0001
	/**<summary>Player can cancel the current move with directional input</summary>*/
	MovementCancel = 1 << 1,    //0010

	HitboxActive = 1 << 2,		//0100

	CanTurn = 1 << 3,			//1000
}

[System.Flags]
public enum ValueFlags
{
	None = 0,
	/**<summary>When the move effects the velocity along the x axis</summary>*/
	xVelocity = 1 << 0,
	/**<summary>When the move effects the velocity along the y axis</summary>*/
	yVelocity = 1 << 1,
}

public enum FlagTypes
{
	CommonFlags = 0,
	ValueFlags = 1
}

[Serializable]
public struct FlagData
{
	
	[EnumFlags]
	public CommonFlags commonFlags;
	[EnumFlags]
	public ValueFlags valueFlags;
	public FlagData(CommonFlags cf, ValueFlags vf)
	{
		commonFlags = cf;
		valueFlags = vf;
	}

	public int this[FlagTypes flag]
	{
		//I tried doing this with an array of generic enums but it caused problems so its a switch statement for now
		get
		{
			switch (flag)
			{
				case FlagTypes.CommonFlags:
					return (int)commonFlags;
				case FlagTypes.ValueFlags:
					return (int)valueFlags;
				default:
					return 0;
			}
		}
		set
		{
			switch (flag)
			{
				case FlagTypes.CommonFlags:
					commonFlags = (CommonFlags)value;
					break;
				case FlagTypes.ValueFlags:
					valueFlags = (ValueFlags)value;
					break;
			}
		}
	}
}