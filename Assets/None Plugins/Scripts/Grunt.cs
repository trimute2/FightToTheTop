using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy {

	protected override void Test()
	{
		avoider.AvoiderType = "Grunt";
		avoider.ThingsToAvoid.Add("Grunt");
	}

	protected override void EnemyDecision()
	{
		shouldAvoid = false;
		attackPermission = false;
		switch (currentTargetRange)
		{
			case Target.LONG_RANGE:
				//Try and move in
				if(target.RequestEnemyCount(Target.CLOSE_RANGE) < 1)
				{
					SetTargetRange(Target.CLOSE_RANGE);
				}//else if(target.RequestEnemyCount(Target.MID_RANGE) < 4)
				else if (target.RequestEnemyCount(Target.MID_RANGE,Direction) < 2)
				{
					SetTargetRange(Target.MID_RANGE);
				}
				else
				{
					SetTargetRange(Target.LONG_RANGE);
					shouldAvoid = true;

					//any long range attack decisions go here
				}
				break;
			case Target.MID_RANGE:
				if (target.RequestEnemyCount(Target.CLOSE_RANGE) < 1)
				{
					SetTargetRange(Target.CLOSE_RANGE);
				} //else if(target.RequestEnemyCount(Target.MID_RANGE) > 4)
				else if (target.RequestEnemyCount(Target.MID_RANGE,Direction) > 2)
				{
					SetTargetRange(Target.LONG_RANGE);
				}
				else
				{
					SetTargetRange(Target.MID_RANGE);
					shouldAvoid = true;
				}
				break;
			case Target.CLOSE_RANGE:
				if (target.RequestEnemyRemaining(Target.CLOSE_RANGE) > 1)
				{
					SetTargetRange(Target.MID_RANGE);
				}
				else
				{
					SetTargetRange(Target.CLOSE_RANGE);
					attackPermission = true;
				}
				break;
		}
	}
}
