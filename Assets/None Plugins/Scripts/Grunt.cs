using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy {

	protected override void EnemyDecision()
	{
		EnemyCommands previousCommand = currentCommand;
		switch (currentRange)
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
		}
	}
}
