using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour {
	public float longRange;
	public float midRange;
	public float closeRange;
	private int targetRange = Target.LONG_RANGE;
	public int TargetRange
	{
		get
		{
			return targetRange;
		}
		set
		{
			targetRange = value;
		}
	}
	private int currentRange = Target.OUT_RANGE;
	public int CurrentRange
	{
		get
		{
			return currentRange;
		}
	}
	private Target target;
	public Target CurrentTarget
	{
		get
		{
			return target;
		}
		set
		{
			target = value;
			//active = true;
		}
	}
	private float xdistance;
	public float XDistance
	{
		get
		{
			return xdistance;
		}
	}
	public int Direction
	{
		get
		{
			return (int)Mathf.Sign(xdistance);
		}
	}


	// Use this for initialization
	void Start () {
		xdistance = float.MaxValue;
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null)
		{
			xdistance = target.transform.position.x - transform.position.x;
			int previousRange = currentRange;
			float distance = Mathf.Abs(xdistance);
			if (distance <= closeRange)
			{
				currentRange = Target.CLOSE_RANGE;
			}
			else if (distance <= midRange)
			{
				currentRange = Target.MID_RANGE;
			}
			else if (distance <= longRange)
			{
				currentRange = Target.LONG_RANGE;
			}
			else
			{
				currentRange = Target.OUT_RANGE;
			}
			if(currentRange != previousRange)
			{
				//target.SwapRanges(previousRange, currentRange, this);
			}
			//TODO: either call some form of AI Component, or have the AI update on its own and call functions here
		}
	}
}
