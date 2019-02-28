using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AttackType
{
	Melee,
	Charge,
	Range,
	Projectile,
	Area,
}


public enum AttackRange
{
	/// <summary>
	///Next to the player
	/// </summary>
	Close = 0,
	/// <summary>
	/// one grunt length away from the player
	/// </summary>
	ShortRange = 1,
	/// <summary>
	/// between two and three grunt lengths from the player
	/// </summary>
	Medium = 2,
	/// <summary>
	/// anything farther than three grunt lengths
	/// </summary>
	Far = 3,
}

[Serializable]
public struct EnemyAttackData
{
	public MoveData Move;
	public AttackType type;
	public AttackRange range;
	public int Priority;
}
[CreateAssetMenu(fileName = "EnemyMoveList", menuName = "Data/Enemy Move List", order = 2)]
public class EnemyMoveList : ScriptableObject {
	public List<EnemyAttackData> Attacks;
	//TODO write a custom inspector to make this reorderable
	private AttackRange[] rangePriority = { AttackRange.Close, AttackRange.ShortRange, AttackRange.Medium, AttackRange.Far };
	public AttackRange[] RangePriority
	{
		get
		{
			return rangePriority;
		}
	}

}