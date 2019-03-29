using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gravity Effect", menuName = "Data/Effects/Gravity Effect", order = 1)]
public class GravityEffect : EntityEffects
{
	public bool setGravity;

	public override void Effect(MoveHandler moveHandler)
	{
		moveHandler.SetGravity(setGravity);
	}
}
