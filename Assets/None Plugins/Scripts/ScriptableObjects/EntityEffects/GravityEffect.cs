using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEffect : EntityEffects
{
	public bool setGravity;

	public override void Effect(MoveHandler moveHandler)
	{
		moveHandler.SetGravity(setGravity);
	}
}
