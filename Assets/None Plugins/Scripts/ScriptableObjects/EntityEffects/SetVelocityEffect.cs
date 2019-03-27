using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetVelocity", menuName = "Data/Effects/SetVelocity", order = 1)]
public class SetVelocityEffect : EntityEffects
{
	public Vector2 velocity;

	public override void Effect(MoveHandler moveHandler)
	{
		moveHandler.SetVelocity(velocity);
	}
}
