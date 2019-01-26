using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ControllerFlags
{
	None = 0x00,
	/**<summary>when active the controller changes velocity based off of input</summary>*/
	MoveWithInput = 0x01,
}
