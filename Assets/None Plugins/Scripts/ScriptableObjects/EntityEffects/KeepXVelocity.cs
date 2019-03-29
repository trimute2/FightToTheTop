using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeepXVelocity", menuName = "Data/Effects/KeepXVelocity", order = 1)]
public class KeepXVelocity : EntityEffects
{
	public override void Effect(MoveHandler moveHandler)
	{
		Vector2 targetVel;
		targetVel.x = moveHandler.TargetVelocity.x;
		targetVel.y = moveHandler.Velocity.y - moveHandler.TargetVelocity.y;
		moveHandler.SetEntityVelocity(targetVel);
		moveHandler.AllowXVelocity();
	}
}
