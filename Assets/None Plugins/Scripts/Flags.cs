using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumFlagsAttribute : PropertyAttribute
{
}

[System.Flags]
public enum CommonFlags
{
	None = 0,					//000
	/**<summary>when active the controller changes velocity based off of input</summary>*/
	MoveWithInput = 1 << 0,     //001
	/**<summary>Player can cancel the current move with directional input</summary>*/
	MovementCancel = 1 << 1,    //010

	HitboxActive = 1 << 2,		//100
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

