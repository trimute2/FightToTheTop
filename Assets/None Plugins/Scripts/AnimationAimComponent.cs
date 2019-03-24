using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
public class AnimationAimComponent : MonoBehaviour {

	private MoveHandler moveHandler;

	private Targeter targeter;
	private bool hasTargeter;

	public Transform AimPoint;
	public float aimDist;

	public List<Transform> aimingControls;
	public List<Transform> aimingPoints;

	private bool targeting = false;
	private bool holding = false;

	private Transform animationTargeter;
	private Transform animationTarget;

	private Vector3 holdPoint;
	public float currentAim;

	public float maxAimSpeed = 15.0f;

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
		if (targeting)
		{
			float targetAngle = CalcAim();
			float clamp = maxAimSpeed * Mathf.Deg2Rad * Time.deltaTime;
			float changeAngle = Mathf.Clamp(targetAngle - currentAim, -clamp, clamp);
			currentAim += changeAngle;
			
		}
		if(targeting || holding)
		{
			Vector3 p = AimPoint.position;
			float xPos = Mathf.Cos(currentAim) * aimDist;
			float yPos = Mathf.Sin(currentAim) * aimDist;
			Vector3 test = new Vector3(p.x + xPos, p.y + yPos, 0);
			Debug.DrawLine(p, animationTarget.position);
			animationTargeter.position = test;
		}
		if (attatchedEffects.Count > 0)
		{
			for (int i = attatchedEffects.Count - 1; i >= 0; i--)
			{
				VisualEffects.IVisualEffect e = attatchedEffects[i];
				if (!e.Equals(null))
				{
					e.EffectUpdate(currentAim);
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
			animationTargeter = aimingControls[pointIndex];
			animationTarget = target;
			targeting = true;
			animationTargeter.position = animationTarget.position;
			currentAim = CalcAim();//CalculateAim();
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

	public void DetatchVisualEffect(VisualEffects.IVisualEffect visualEffect)
	{
		attatchedEffects.Remove(visualEffect);
	}

	public void RemoveVisualEffect()
	{
		for(int i = attatchedEffects.Count - 1; i >= 0; i--)
		{
			if (!attatchedEffects[i].Equals(null))
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

	public void SpawnAndAttatchEffectAE(AnimationEvent animationEvent)
	{
		if(animationEvent.objectReferenceParameter != null && animationEvent.objectReferenceParameter is GameObject &&
			((GameObject)animationEvent.objectReferenceParameter).GetComponentInChildren<VisualEffects.IVisualEffect>() != null)
		{
			GameObject obj = Instantiate(((GameObject)animationEvent.objectReferenceParameter), aimingPoints[animationEvent.intParameter]);
			VisualEffects.IVisualEffect attatch = (VisualEffects.IVisualEffect)obj.GetComponent<VisualEffects.IVisualEffect>();
			AttatchVisualEffect(attatch);
		}
	}



	private float CalculateAim()
	{
		Vector3 diff = animationTarget.position - AimPoint.position;
		diff.Normalize();
		float dir = Mathf.Sign(transform.localScale.x);
		float rot = Mathf.Atan2(diff.y, dir * diff.x) * Mathf.Rad2Deg;
		return rot;
	}

	private float CalcAim()
	{
		Vector3 p = AimPoint.position;
		Vector3 d1 = animationTarget.position - p;
		float angle = Mathf.Atan2(d1.y, d1.x);
		if(angle < 0)
		{
			angle += Mathf.PI * 2;
		}
		return angle;
	}
}
