using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
public class AnimationAimComponent : MonoBehaviour {

	private MoveHandler moveHandler;

	private Targeter targeter;
	private bool hasTargeter;

	public List<Transform> aimingControls;
	public List<Transform> aimingPoints;

	private bool targeting = false;
	private bool holding = false;

	private Transform animationPoint;
	private Transform animationTargeter;
	private Transform animationTarget;

	private Vector3 holdPoint;

	private Vector3 TargetPoint
	{
		get
		{
			if (holding)
			{
				return holdPoint;
			}
			return animationTarget.position;
		}
	}

	private List<VisualEffects.IVisualEffect> attatchedEffects;

	// Use this for initialization
	void Start () {
		moveHandler = GetComponent<MoveHandler>();
		moveHandler.GenericStateEvent += StopTargeting;

		targeter = GetComponent<Targeter>();
		hasTargeter = (targeter != null);

		attatchedEffects = new List<VisualEffects.IVisualEffect>();
	}

	private void LateUpdate()
	{
		bool updateEffects = false;
		if (targeting)
		{
			animationTargeter.position = animationTarget.position;
			updateEffects = true;
		}else if (holding)
		{
			animationTargeter.position = holdPoint;
			updateEffects = true;
		}
		if (updateEffects && attatchedEffects.Count > 0)
		{
			Vector3[] updateList = new[] { animationPoint.position, TargetPoint };
			foreach(VisualEffects.IVisualEffect e in attatchedEffects)
			{
				if(e != null)
				{
					e.EffectUpdate(updateList);
				}
			}
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
		if(pointIndex >= 0 && pointIndex < aimingControls.Count)
		{
			animationPoint = aimingPoints[pointIndex];
			animationTargeter = aimingControls[pointIndex];
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
		RemoveVisualEffect();
	}

	public void AttatchVisualEffect(VisualEffects.IVisualEffect visualEffect)
	{
		VisualEffects.IVisualEffect ve = (VisualEffects.IVisualEffect)visualEffect;
		attatchedEffects.Add(ve);
	}

	public void DetatchVisualEffect(int index)
	{
		if(index >= 0 && attatchedEffects.Count < index)
		{
			attatchedEffects.RemoveAt(index);
		}
	}

	public void RemoveVisualEffect()
	{
		for(int i = attatchedEffects.Count - 1; i >= 0; i--)
		{
			if (attatchedEffects[i] != null)
			{
				attatchedEffects[i].EndEffect();
			}
		}
		attatchedEffects.Clear();
	}

	public void SpawnAndAttatchEffect(GameObject prefab)
	{
		if (prefab.GetComponentInChildren<VisualEffects.IVisualEffect>() != null) {
			GameObject obj = Instantiate(prefab);
			VisualEffects.IVisualEffect attatch = (VisualEffects.IVisualEffect)obj.GetComponent<VisualEffects.IVisualEffect>();
			AttatchVisualEffect(attatch);
		}
	}
}
