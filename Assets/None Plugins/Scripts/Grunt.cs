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
		switch (currentTargetRange)
		{
			case Target.LONG_RANGE:
				//Try and move in
				if(target.RequestEnemyCount(Target.CLOSE_RANGE) < 1)
				{
					SetTargetRange(Target.CLOSE_RANGE);
				}else if(target.RequestEnemyCount(Target.MID_RANGE) < 4)
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
				} else if(target.RequestEnemyCount(Target.MID_RANGE) > 4)
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
				break;
		}
		//EnemyCommands previousCommand = currentCommand;
		/*switch (currentRange)
		{
			case AttackRange.Close:
				//if just attacked allow to move back

				currentCommand = EnemyCommands.Attack;
				break;
			case AttackRange.ShortRange:
				//check if there is space to move forward
				int Close = EnemyManager.Instance.GetTargetEntitiesInRange(AttackRange.Close);
				if(Close == 0)
				{
					currentCommand = EnemyCommands.MoveIn;
				}
				break;
			case AttackRange.Medium:
				//check if there is space in close
				//otherwise try and attack
				break;
			case AttackRange.Far:
				//check if there is range to move into medium
				//otherwise idle
				break;
		}
		if(currentCommand != previousCommand)
		{
			//EnemyManager.Instance
		}*/
	}
}
