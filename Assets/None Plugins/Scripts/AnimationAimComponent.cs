using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
public class AnimationAimComponent : MonoBehaviour {

	private MoveHandler moveHandler;

	private Targeter targeter;
	private bool hasTargeter;

	public List<Transform> aimingPoints;

	private bool targeting = false;
	private bool holding = false;

	private Transform animationTargeter;
	private Transform animationTarget;

	private Vector3 holdPoint;

	// Use this for initialization
	void Start () {
		moveHandler = GetComponent<MoveHandler>();
		moveHandler.GenericStateEvent += StopTargeting;

		targeter = GetComponent<Targeter>();
		hasTargeter = (targeter != null);
	}

	private void LateUpdate()
	{
		if (targeting)
		{
			animationTargeter.position = animationTarget.position;
		}else if (holding)
		{
			animationTargeter.position = holdPoint;
		}
	}

	public void StartTargetingTarget(int pointIndex)
	{
		if(hasTargeter && targeter.target != null)
		{
			StartTargeting(pointIndex, targeter.target.transform);
		}
	}

	public void StartTargeting(int pointIndex, Transform target)
	{
		if(pointIndex >= 0 && pointIndex < aimingPoints.Count)
		{
			animationTargeter = aimingPoints[pointIndex];
			animationTarget = target;
			targeting = true;
			animationTargeter.position = animationTarget.position;
		}
	}

	public void HoldPosition()
	{
		if (targeting)
		{
			holdPoint = animationTargeter.position;
			targeting = false;
			holding = true;
		}
	}

	public void StopTargeting()
	{
		targeting = false;
		holding = false;
	}
}
