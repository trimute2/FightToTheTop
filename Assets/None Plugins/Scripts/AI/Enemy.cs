using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
[RequireComponent(typeof(Targeter))]
public abstract class Enemy : MonoBehaviour
{
	//Required components
	protected Targeter targeter;
	protected MoveHandler moveHandler;
	protected FlagHandler flagHandler;

	//optional components
	protected EntityControllerComp entityController;
	protected bool hasEntityController;

	protected Avoider avoider;
	protected bool hasAvoider;

	//bools to control desicion making
	protected bool moveToRange;
	protected bool tryToAttack;

	protected List<MoveLink> linksToAttempt;

	protected Vector2 targetVelocity;

	private Vector3 AvoidVector = Vector3.zero;

	public int PlacementPriority
	{
		get
		{
			return targeter.PlacementPriority;
		}
	}

	public float walkSpeed = 4.5f;
	public float runSpeed = 9.0f;

	public float thinkPeriod = 0.4f;

	private float lastThought = 0.0f;

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

	protected void AvoiderSetup(string avoiderType, IEnumerable<string> thingsToAvoid)
	{
		if (hasAvoider)
		{
			avoider.AvoiderType = avoiderType;
			avoider.ThingsToAvoid.AddRange(thingsToAvoid);
			moveHandler.GenericStateEvent += avoider.OnEnterGenericState;
			avoider.ChangedAvoiding += ChangedAvoiderTarget;
		}
	}

	private void ChangedAvoiderTarget()
	{
		if(avoider.avoidTransform != null)
		{
			UpdateAvoidVec();
		}
	}

	void Update()
    {
        if(targeter.target != null)
		{

			targetVelocity = Vector2.zero;
			
			moveToRange = true;
			tryToAttack = true;
			//todo allow for optional wait times so that more complex if conditions dont happen every frame
			switch (targeter.CurrentRange)
			{
				case Target.LONG_RANGE:
					LongRangeDecision(targeter.target);
					break;
				case Target.MID_RANGE:
					MidRangeDecision(targeter.target);
					break;
				case Target.CLOSE_RANGE:
					CloseRangeDecision(targeter.target);
					break;
			}
			if ((Time.time - lastThought) > thinkPeriod)
			{
				lastThought = Time.time;
				UpdateAvoidVec();
			}
			if (hasEntityController)
			{
				if ((flagHandler.CommonFlags & CommonFlags.MoveWithInput) != CommonFlags.None)
				{
					if (moveToRange)
					{
						targetVelocity = targeter.TargetDirection();
						float speed = SpeedDecision();
						targetVelocity.x *= entityController.Facing * speed;
						tryToAttack = false;
					}
					if(hasAvoider && AvoidVector != Vector3.zero)
					{
						if (!(Mathf.Sign(targetVelocity.x) == Mathf.Sign(AvoidVector.x) &&
							Mathf.Abs(targetVelocity.x) > Mathf.Abs(AvoidVector.x)))
						{
							targetVelocity.x = AvoidVector.x;
							tryToAttack = false;
						}
					}
					/*
					if (hasAvoider && avoider.AvoidVector != Vector3.zero)
					{
						if (!(Mathf.Sign(targetVelocity.x) == Mathf.Sign(avoider.AvoidVector.x) &&
							Mathf.Abs(targetVelocity.x) > Mathf.Abs(avoider.AvoidVector.x)))
						{
							targetVelocity.x = avoider.AvoidVector.x;
							tryToAttack = false;
						}
					}*/
				}
				entityController.TargetVelocity = targetVelocity;
			}

			if (tryToAttack)
			{
				moveHandler.CheckMoves(linksToAttempt);
			}
		}
    }

	protected virtual float SpeedDecision()
	{
		float speed = walkSpeed;
		if (targeter.CurrentRange == Target.OUT_RANGE)
		{
			speed = runSpeed;
		}
		return speed;
	}

	protected void UpdateAvoidVec()
	{
		if(avoider == null || avoider.avoidTransform == null)
		{
			AvoidVector = Vector3.zero;
		}
		else
		{
			AvoidVector = transform.position - avoider.avoidTransform.position;
			AvoidVector = Vector3.ClampMagnitude(AvoidVector, runSpeed);
			if(AvoidVector.sqrMagnitude < walkSpeed*walkSpeed)
			{
				
				AvoidVector = AvoidVector.normalized * walkSpeed;
			}
		}
	}

	protected abstract void LongRangeDecision(Target target);

	protected abstract void MidRangeDecision(Target target);

	protected abstract void CloseRangeDecision(Target target);

	public void Gizmo()
	{
		Debug.DrawRay(transform.position,AvoidVector);
	}
}
