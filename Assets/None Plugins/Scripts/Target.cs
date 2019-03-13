using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
	//this was somewhat stupid, this should have been an array of three lists
	//TODO: clean up by making array of lists
	private float longRangeTension = 0;
	private List<Enemy> LongRangeEnemies;
	private List<Targeter> LongRangeTargeters;
	private float midRangeTension = 0;
	private List<Enemy> MidRangeEnemies;
	private List<Enemy> MidRangeTargets;
	private List<Targeter> MidRangeTargeters;
	private float closeRangeTension = 0;
	private List<Enemy> CloseRangeEnemies;
	private List<Enemy> CloseRangeTargets;
	private List<Targeter> CloseRangeTargeters;
	private float[] LatestAttack;
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
		LongRangeTargeters = new List<Targeter>();
		MidRangeEnemies = new List<Enemy>();
		MidRangeTargeters = new List<Targeter>();
		MidRangeTargets = new List<Enemy>();
		CloseRangeEnemies = new List<Enemy>();
		CloseRangeTargeters = new List<Targeter>();
		CloseRangeTargets = new List<Enemy>();
		LatestAttack = new float[OUT_RANGE];
		for(int i = 0; i < OUT_RANGE; i++)
		{
			LatestAttack[i] = 0;
		}
	}

	//TODO: functoion that can let requester now what type of enemy is in a range


	//TODO: way of figuring out who is leaving a range
	
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
		return GetRangeEnemyList(Range).Count;
	}

	public int RequestEnemyCount(int Range, int sign)
	{
		List<Enemy> target = GetRangeEnemyList(Range);
		target.RemoveAll(item => sign != Mathf.Sign(item.XDistance));
		return target.Count;
	}

	public int RequestEnemyRemaining(int Range)
	{
		List<Enemy> target = new List<Enemy>();
		switch (Range)
		{
			case MID_RANGE:
				target.AddRange(MidRangeEnemies);
				break;
			case CLOSE_RANGE:
				target.AddRange(CloseRangeEnemies);
				break;
			default:
				return 0;
		}
		target.RemoveAll(item => item.TargetRange != Range);
		return target.Count;
	}

	public int RequestTargetCount(int Range)
	{
		switch (Range)
		{
			case MID_RANGE:
				return MidRangeTargets.Count;
			case CLOSE_RANGE:
				return CloseRangeTargets.Count;
			default:
				return 0;
		}
	}

	public int RequestTotalCount(int Range)
	{
		switch (Range)
		{
			case LONG_RANGE:
				return LongRangeEnemies.Count;
			case MID_RANGE:
				return MidRangeEnemies.Count + MidRangeTargets.Count;
			case CLOSE_RANGE:
				return CloseRangeEnemies.Count + CloseRangeTargets.Count;
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

	public void AddToRange(int Range, Targeter targeter)
	{
		switch (Range)
		{
			case LONG_RANGE:
				if (!LongRangeTargeters.Contains(targeter))
				{
					LongRangeTargeters.Add(targeter);
				}
				break;
			case MID_RANGE:
				if (!MidRangeTargeters.Contains(targeter))
				{
					MidRangeTargeters.Add(targeter);
				}
				break;
			case CLOSE_RANGE:
				if (!CloseRangeTargeters.Contains(targeter))
				{
					CloseRangeTargeters.Add(targeter);
				}
				break;
		}
	}

	public void AddToTargetRange(int Range, Enemy enemy)
	{
		switch (Range)
		{
			case MID_RANGE:
				if (!MidRangeTargets.Contains(enemy))
				{
					MidRangeTargets.Add(enemy);
				}
				break;
			case CLOSE_RANGE:
				if (!CloseRangeTargets.Contains(enemy))
				{
					CloseRangeTargets.Add(enemy);
				}
				break;
		}
	}

	public void RemoveFromTargetRange(int Range, Enemy enemy)
	{
		switch (Range)
		{
			case MID_RANGE:
				MidRangeTargets.Remove(enemy);
				break;
			case CLOSE_RANGE:
				CloseRangeTargets.Remove(enemy);
				break;
		}
	}

	public void RemoveFromAllRanges(Enemy enemy)
	{
		LongRangeEnemies.Remove(enemy);
		MidRangeEnemies.Remove(enemy);
		CloseRangeEnemies.Remove(enemy);
	}

	public void SwapRanges(int removeRange, int addRange, Enemy enemy)
	{
		RemoveFromRange(removeRange, enemy);
		AddToRange(addRange, enemy);
	}

	public void SwapRanges(int removeRange, int addRange, Targeter targeter)
	{
		RemoveFromRange(removeRange, targeter);
		AddToRange(addRange, targeter);
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

	public void RemoveFromRange(int Range, Targeter targeter)
	{
		switch (Range)
		{
			case LONG_RANGE:
				LongRangeTargeters.Remove(targeter);
				break;
			case MID_RANGE:
				MidRangeTargeters.Remove(targeter);
				break;
			case CLOSE_RANGE:
				CloseRangeTargeters.Remove(targeter);
				break;
		}
	}

	private List<Enemy> GetRangeEnemyList(int Range)
	{
		List<Enemy> target = new List<Enemy>();
		switch (Range)
		{
			case LONG_RANGE:
				target.AddRange(LongRangeEnemies);
				break;
			case MID_RANGE:
				target.AddRange(MidRangeEnemies);
				break;
			case CLOSE_RANGE:
				target.AddRange(CloseRangeEnemies);
				break;
		}
		return target;
	}

	public void StartAttack(int Range)
	{
		if(Range >= 0 && Range < OUT_RANGE)
		{
			LatestAttack[Range] = Time.time;
		}
	}

	public float LastAttack(int Range)
	{
		if(Range >= 0 && Range < OUT_RANGE)
		{
			return Time.time - LatestAttack[Range];
		}
		return LastAttack();
	}

	public float LastAttack()
	{
		float minimumTime = Time.time - LatestAttack[0];
		for (int i = 1; i < OUT_RANGE; i++)
		{
			float t = Time.time - LatestAttack[i];
			if(t < minimumTime)
			{
				minimumTime = t;
			}
		}
		return minimumTime;
	}
}
