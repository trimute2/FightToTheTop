using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
[RequireComponent(typeof(Targeter))]
public abstract class Enemy : MonoBehaviour
{
	protected Targeter targeter;
	protected MoveHandler moveHandler;
	protected FlagHandler flagHandler;

	protected EntityControllerComp entityController;
	protected bool hasEntityController;

	protected Avoider avoider;
	protected bool hasAvoider;

	protected bool moveToRange;
	protected bool tryToAttack;

	protected List<MoveLink> linksToAttempt;

	protected Vector2 targetVelocity;

	public float walkSpeed = 4.5f;
	public float runSpeed = 9.0f;

	protected virtual void Start()
	{
		moveHandler = GetComponent<MoveHandler>();
		targeter = GetComponent<Targeter>();
		entityController = GetComponent<EntityControllerComp>();
		hasEntityController = (entityController != null);
		flagHandler = GetComponent<FlagHandler>();
		avoider = GetComponentInChildren<Avoider>();
		hasAvoider = (avoider != null);
		linksToAttempt = new List<MoveLink>();
	}

	void Update()
    {
        if(targeter.target != null)
		{
			targetVelocity = Vector2.zero;
			moveToRange = true;
			tryToAttack = true;
			switch (targeter.CurrentRange)
			{
				case Target.LONG_RANGE:
					LongRangeDecision();
					break;
				case Target.MID_RANGE:
					MidRangeDecision();
					break;
				case Target.CLOSE_RANGE:
					CloseRangeDecision();
					break;
			}

			if (hasEntityController && ((flagHandler.CommonFlags & CommonFlags.MoveWithInput) != CommonFlags.None))
			{
				if (moveToRange)
				{
					targetVelocity = targeter.TargetDirection();
					float speed = walkSpeed;
					if (targeter.CurrentRange == Target.OUT_RANGE)
					{
						speed = runSpeed;
					}
					targetVelocity.x *= entityController.Facing * speed;
					tryToAttack = false;
				}
				if (hasAvoider && avoider.AvoidVector != Vector3.zero)
				{
					if (!(Mathf.Sign(targetVelocity.x) == Mathf.Sign(avoider.AvoidVector.x) &&
						Mathf.Abs(targetVelocity.x) > Mathf.Abs(avoider.AvoidVector.x)))
					{
						targetVelocity.x = avoider.AvoidVector.x;
						tryToAttack = false;
					}
				}
				entityController.TargetVelocity = targetVelocity;
			}

			if (tryToAttack)
			{
				moveHandler.CheckMoves(linksToAttempt);
			}
		}
    }

	protected abstract void LongRangeDecision();

	protected abstract void MidRangeDecision();

	protected abstract void CloseRangeDecision();
}
