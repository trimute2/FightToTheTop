using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeakenGravity", menuName = "Data/Effects/WeakenGravity", order = 1)]
public class WeakenGravity : EntityEffects
{
	public float modifier;

	public override void Effect(MoveHandler moveHandler)
	{
		moveHandler.WeakenGravity(modifier);
	}
}
