using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteComp : Enemy
{

	//private bool ChargeState;

	protected override void Start()
	{
		base.Start();
		string[] thingsToAvoid = new string[] {"Brute"};
		AvoiderSetup("Brute", thingsToAvoid);

	}

	protected override void LongRangeDecision(Target target)
	{
		//try to move into range ingoring lower priority targets
		if (target.RequestPriorityTargeterCount(Target.CLOSE_RANGE, PlacementPriority) < 1)
		{
			targeter.TargetRange = Target.CLOSE_RANGE;
		}
		else if (target.RequestRangeTotalPriority(Target.MID_RANGE)<2)
		{
			targeter.TargetRange = Target.MID_RANGE;
		}
		else
		{
			targeter.TargetRange = Target.LONG_RANGE;
			moveToRange = false;
			//shouldAvoid = true;
		}
	}

	protected override void MidRangeDecision(Target target)
	{
		if (target.RequestPriorityTargeterCount(Target.CLOSE_RANGE, PlacementPriority) < 1)
		{
			targeter.TargetRange = Target.CLOSE_RANGE;
		}
		else if (target.RequestTargeterCount(Target.MID_RANGE, targeter.Direction) > 2)
		{
			targeter.TargetRange = Target.LONG_RANGE;
		}
		else
		{
			targeter.TargetRange = Target.MID_RANGE;
			moveToRange = false;
			//shouldAvoid = true;
		}
	}

	protected override void CloseRangeDecision(Target target)
	{
		if (target.RequestPriorityTargeterRemaining(Target.CLOSE_RANGE, PlacementPriority) > 1)
		{
			targeter.TargetRange = Target.MID_RANGE;
		}
		else
		{
			targeter.TargetRange = Target.CLOSE_RANGE;
			moveToRange = false;
		}
	}
	
}
