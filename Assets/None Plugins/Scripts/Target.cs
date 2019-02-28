using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	private float longRangeTension = 0;
	private List<Enemy> LongRangeEnemies;
	private float midRangeTension = 0;
	private List<Enemy> MidRangeEnemies;
	private float closeRangeTension = 0;
	private List<Enemy> CloseRangeEnemies;
	public float Tension
	{
		get
		{
			return longRangeTension + midRangeTension + closeRangeTension;
		}
	}

	public const int OUT_RANGE = 3;
	public const int LONG_RANGE = 2;
	public const int MID_RANGE = 1;
	public const int CLOSE_RANGE = 0;

	private void Awake()
	{
		LongRangeEnemies = new List<Enemy>();
		MidRangeEnemies = new List<Enemy>();
		CloseRangeEnemies = new List<Enemy>();
	}

	public float RequestTension(int Range)
	{
		switch (Range)
		{
			case LONG_RANGE:
				return longRangeTension;
			case MID_RANGE:
				return midRangeTension;
			case CLOSE_RANGE:
				return closeRangeTension;
			default:
				return float.MaxValue;
		}
	}

	public int RequestEnemyCount(int Range)
	{
		switch (Range)
		{
			case LONG_RANGE:
				return LongRangeEnemies.Count;
			case MID_RANGE:
				return MidRangeEnemies.Count;
			case CLOSE_RANGE:
				return CloseRangeEnemies.Count;
			default:
				return 0;
		}
	}

	public void AddToRange(int Range, Enemy enemy)
	{
		switch (Range)
		{
			case LONG_RANGE:
				if (!LongRangeEnemies.Contains(enemy))
				{
					LongRangeEnemies.Add(enemy);
				}
				break;
			case MID_RANGE:
				if (!MidRangeEnemies.Contains(enemy))
				{
					MidRangeEnemies.Add(enemy);
				}
				break;
			case CLOSE_RANGE:
				if (!CloseRangeEnemies.Contains(enemy))
				{
					CloseRangeEnemies.Add(enemy);
				}
				break;
		}
	}

	public void RemoveFromAllRanges(Enemy enemy)
	{
		LongRangeEnemies.Remove(enemy);
		MidRangeEnemies.Remove(enemy);
		CloseRangeEnemies.Remove(enemy);
	}

	public void RemoveFromRange(int Range, Enemy enemy)
	{
		switch (Range)
		{
			case LONG_RANGE:
				LongRangeEnemies.Remove(enemy);
				break;
			case MID_RANGE:
				MidRangeEnemies.Remove(enemy);
				break;
			case CLOSE_RANGE:
				CloseRangeEnemies.Remove(enemy);
				break;
		}
	}
}
