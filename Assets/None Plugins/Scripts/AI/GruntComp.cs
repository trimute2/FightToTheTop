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
	private Avoider avoider;
	public MoveLink Punch;
	public MoveLink Shoot;
	// Use this for initialization
	void Start () {
		moveHandler = GetComponent<MoveHandler>();
		targeter = GetComponent<Targeter>();
		entityController = GetComponent<EntityControllerComp>();
		flagHandler = GetComponent<FlagHandler>();
		avoider = GetComponentInChildren<Avoider>();
		avoider.AvoiderType = "Grunt";
		avoider.ThingsToAvoid.Add("Grunt");
#if UNITY_EDITOR
		if (avoider == null)
		{
			Debug.LogWarning("This object should have an avoider as a child");
		}
#endif
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
					else if (target.RequestTargeterCount(Target.MID_RANGE, targeter.Direction) < 2)
					{
						targeter.TargetRange = Target.MID_RANGE;
					}
					else
					{
						targeter.TargetRange = Target.LONG_RANGE;
						moveToRange = false;
						//shouldAvoid = true;

						//any long range attack decisions go here
					}
					break;
				case Target.MID_RANGE:
					if (target.RequestTargeterCount(Target.CLOSE_RANGE) < 1)
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
					break;
				case Target.CLOSE_RANGE:
					if (target.RequestTargeterRemaining(Target.CLOSE_RANGE) > 1)
					{
						targeter.TargetRange = Target.MID_RANGE;
					}
					else
					{
						targeter.TargetRange = Target.CLOSE_RANGE;
						moveToRange = false;
						if (moveHandler.CheckMove(Punch))
						{
							moveHandler.ExecuteConditions(Punch);
							moveHandler.StartMove(Punch.move);
						}
					}
					break;
			}
			if ((flagHandler.CommonFlags & CommonFlags.MoveWithInput) != CommonFlags.None) {
				//any movement stuff goes here
				if (moveToRange)
				{
					targetVelocity = targeter.TargetDirection();
					targetVelocity.x *= entityController.Facing * movementSpeed;
				}else if(avoider.AvoidVector != Vector3.zero)
				{
					targetVelocity.x = avoider.AvoidVector.x;
				}
				else
				{
					if (moveHandler.CheckMove(Shoot))
					{
						moveHandler.ExecuteConditions(Shoot);
						moveHandler.StartMove(Shoot.move);
					}
				}
			}

			entityController.TargetVelocity = targetVelocity;
		}
	}
}
