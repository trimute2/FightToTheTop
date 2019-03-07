using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnOn", menuName = "Data/Effects/TurnOnCommonFlag", order = 1)]
public class TurnOnCommonEntityFlag : EntityEffects
{

	[EnumFlags]
	public CommonFlags flags;
	public override void Effect(EntityController entity)
	{
		entity.TurnCommonFlagsOn(flags);
	}

	public override void Effect(MoveHandler moveHandler)
	{
		moveHandler.TurnCommonFlagsOn(flags);
	}
}
