using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetEntityVelocity", menuName = "Data/Effects/SetEntityVelocity", order = 1)]
public class SetEntityVelocity : EntityEffects
{
	public Vector2 velocity;

	public override void Effect(EntityController entity)
	{
		throw new System.NotImplementedException();
	}

	public override void Effect(MoveHandler moveHandler)
	{
		moveHandler.SetEntityVelocity(velocity);
	}
}
