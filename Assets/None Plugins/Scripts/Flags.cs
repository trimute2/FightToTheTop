using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum CommonFlags
{
	None = 0,			//000
	/**<summary>when active the controller changes velocity based off of input</summary>*/
	MoveWithInput = 1,  //001
	/**<summary>Player can cancel the current move with directional input</summary>*/
	MovementCancel = 2  //010
}
