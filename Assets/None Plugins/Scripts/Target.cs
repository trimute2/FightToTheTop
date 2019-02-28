using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	public float LongRange;
	private float longRangeTension;
	private List<Enemy> LongRangeEnemies;
	public float MidRange;
	private float midRangeTension;
	private List<Enemy> MidRangeEnemies;
	public float CloseRange;
	private float closeRangeTension;
	private List<Enemy> CloseRangeEnemies;
	public float Tension
	{
		get
		{
			return longRangeTension + midRangeTension + closeRangeTension;
		}
	}

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
		//TODO: implement RequestEnemyCount
		throw new System.NotImplementedException();
	}
}
