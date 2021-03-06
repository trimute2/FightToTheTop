﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnOff", menuName = "Data/Effects/TurnOffCommonFlag", order = 1)]
public class TurnOffCommonEntityFlag : EntityEffects
{
	[EnumFlags]
	public CommonFlags flags;

	public override void Effect(MoveHandler moveHandler)
	{
		moveHandler.TurnCommonFlagsOff(flags);
	}
}