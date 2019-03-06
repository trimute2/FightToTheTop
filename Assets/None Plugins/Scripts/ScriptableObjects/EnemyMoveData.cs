using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
	Melee,
	Charge,
	Range,
	Projectile,
	Area,
}

[CreateAssetMenu(fileName = "Enemy Move", menuName = "Data/Enemy Move Data", order = 1)]
public class EnemyMoveData : MoveData {
	public AttackType aType;
	public float[] range = new float[Target.OUT_RANGE];
}
/*
private enum AttackLength
{
	longRange = Target.LONG_RANGE,
	midRange = Target.MID_RANGE,
	closeRange = Target.CLOSE_RANGE,
}*/