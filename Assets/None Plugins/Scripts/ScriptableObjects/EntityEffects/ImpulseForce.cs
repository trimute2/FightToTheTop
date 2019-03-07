using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Impulse Force", menuName = "Data/Effects/Impulse Force", order = 1)]
public class ImpulseForce : EntityEffects {
	public Vector2 force;

	public override void Effect(EntityController entity)
	{
		entity.AddForce(force);
	}

	public override void Effect(MoveHandler moveHandler)
	{
		moveHandler.AddForce(force);
	}
}
