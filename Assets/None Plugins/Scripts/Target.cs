using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
	//this was somewhat stupid, this should have been an array of three lists
	//TODO: clean up by making array of lists
	private float longRangeTension = 0;
	private List<Targeter> LongRangeTargeters;
	private float midRangeTension = 0;
	private List<Targeter> MidRangeTargeters;
	private float closeRangeTension = 0;
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
		LongRangeTargeters = new List<Targeter>();
		MidRangeTargeters = new List<Targeter>();
		CloseRangeTargeters = new List<Targeter>();
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

	public int RequestTargeterCount(int Range)
	{
		return GetRangeTargeterList(Range).Count;
	}

	public int RequestTargeterCount(int Range, int sign)
	{
		List<Targeter> target = GetRangeTargeterList(Range);
		target.RemoveAll(item => sign != Mathf.Sign(item.XDistance));
		return target.Count;
	}

	public int RequestTargeterRemaining(int Range)
	{
		List<Targeter> target = new List<Targeter>();
		switch (Range)
		{
			case MID_RANGE:
				target.AddRange(MidRangeTargeters);
				break;
			case CLOSE_RANGE:
				target.AddRange(CloseRangeTargeters);
				break;
			default:
				return 0;
		}
		target.RemoveAll(item => item.TargetRange != Range);
		return target.Count;
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

	public void SwapRanges(int removeRange, int addRange, Targeter targeter)
	{
		RemoveFromRange(removeRange, targeter);
		AddToRange(addRange, targeter);
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


	private List<Targeter> GetRangeTargeterList(int Range)
	{
		List<Targeter> target = new List<Targeter>();
		switch (Range)
		{
			case LONG_RANGE:
				target.AddRange(LongRangeTargeters);
				break;
			case MID_RANGE:
				target.AddRange(MidRangeTargeters);
				break;
			case CLOSE_RANGE:
				target.AddRange(CloseRangeTargeters);
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
