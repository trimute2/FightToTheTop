using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
[RequireComponent(typeof(Targeter))]
[RequireComponent(typeof(EntityController))]
public class GruntComp : MonoBehaviour {
	public float movementSpeed = 4.5f;
	private MoveHandler moveHandler;
	private Targeter targeter;
	private EntityControllerComp entityController;
	// Use this for initialization
	void Start () {
		moveHandler = GetComponent<MoveHandler>();
		targeter = GetComponent<Targeter>();
		entityController = GetComponent<EntityControllerComp>();
	}
	
	// Update is called once per frame
	void Update () {
		Target target = targeter.target;
		if (target != null)
		{
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
		}
	}
}
