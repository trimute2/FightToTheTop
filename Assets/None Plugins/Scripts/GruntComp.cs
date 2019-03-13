using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
[RequireComponent(typeof(Targeter))]
[RequireComponent(typeof(EntityControllerComp))]
public class GruntComp : MonoBehaviour {
	public float movementSpeed = 4.5f;
	private MoveHandler moveHandler;
	private Targeter targeter;
	private EntityControllerComp entityController;
	private FlagHandler flagHandler;
	// Use this for initialization
	void Start () {
		moveHandler = GetComponent<MoveHandler>();
		targeter = GetComponent<Targeter>();
		entityController = GetComponent<EntityControllerComp>();
		flagHandler = GetComponent<FlagHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		Target target = targeter.target;
		if (target != null)
		{
			bool moveToRange = true;
			Vector2 targetVelocity = Vector2.zero;
			switch (targeter.CurrentRange)
			{
				case Target.LONG_RANGE:
					//Try and move in
					if (target.RequestTargeterCount(Target.CLOSE_RANGE) < 1)
					{
						targeter.TargetRange = Target.CLOSE_RANGE;
					}
					else if (target.RequestEnemyCount(Target.MID_RANGE, targeter.Direction) < 2)
					{
						targeter.TargetRange = Target.MID_RANGE;
					}
					else
					{
						targeter.TargetRange = Target.LONG_RANGE;
						//shouldAvoid = true;

						//any long range attack decisions go here
					}
					break;
				case Target.MID_RANGE:
					if (target.RequestEnemyCount(Target.CLOSE_RANGE) < 1)
					{
						targeter.TargetRange = Target.CLOSE_RANGE;
					} //else if(target.RequestEnemyCount(Target.MID_RANGE) > 4)
					else if (target.RequestEnemyCount(Target.MID_RANGE, targeter.Direction) > 2)
					{
						targeter.TargetRange = Target.LONG_RANGE;
					}
					else
					{
						targeter.TargetRange = Target.MID_RANGE;
						//shouldAvoid = true;
					}
					break;
				case Target.CLOSE_RANGE:
					if (target.RequestEnemyRemaining(Target.CLOSE_RANGE) > 1)
					{
						targeter.TargetRange = Target.MID_RANGE;
					}
					else
					{
						targeter.TargetRange = Target.CLOSE_RANGE;
						//attackPermission = true;
					}
					break;
			}
			if ((flagHandler.CommonFlags & CommonFlags.MoveWithInput) != CommonFlags.None) {
				//any movement stuff goes here
				if (moveToRange)
				{
					targetVelocity = targeter.TargetDirection();
					targetVelocity.x *= entityController.Facing * movementSpeed;
				}
			}

			entityController.TargetVelocity = targetVelocity;
		}
	}
}
