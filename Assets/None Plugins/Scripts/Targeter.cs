using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour {
	public float longRange;
	public float midRange;
	public float closeRange;

	public int PlacementPriority = 0;

	private int targetRange = Target.LONG_RANGE;
	public int TargetRange
	{
		get
		{
			return targetRange;
		}
		set
		{
			targetRange = value;
		}
	}
	private int currentRange = Target.OUT_RANGE;
	public int CurrentRange
	{
		get
		{
			return currentRange;
		}
	}
	public Target target;
	public Target CurrentTarget
	{
		get
		{
			return target;
		}
		set
		{
			target = value;
			//active = true;
		}
	}
	private float xdistance;
	public float XDistance
	{
		get
		{
			return xdistance;
		}
	}
	public int Direction
	{
		get
		{
			return (int)Mathf.Sign(xdistance);
		}
	}

	private EntityControllerComp entityController;
	private FlagHandler flagHandler;

	// Use this for initialization
	void Start () {
		xdistance = float.MaxValue;
		entityController = GetComponent<EntityControllerComp>();
		flagHandler = GetComponent<FlagHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeScale == 0)
		{
			return;
		}
		if(target != null)
		{
			//TODO: possible method that does not require entity controller
			if(entityController != null && flagHandler != null)
			{
				if ((flagHandler.CommonFlags & CommonFlags.CanTurn) != CommonFlags.None)
				{
					if (Mathf.Sign(xdistance) != entityController.Facing)
					{
						Vector3 sca = transform.localScale;
						entityController.Facing = (int)Mathf.Sign(xdistance);
						sca.x = entityController.Facing;
						transform.localScale = sca;
					}
				}
			}
			xdistance = target.transform.position.x - transform.position.x;
			int previousRange = currentRange;
			float distance = Mathf.Abs(xdistance);
			if (distance <= closeRange)
			{
				currentRange = Target.CLOSE_RANGE;
			}
			else if (distance <= midRange)
			{
				currentRange = Target.MID_RANGE;
			}
			else if (distance <= longRange)
			{
				currentRange = Target.LONG_RANGE;
			}
			else
			{
				currentRange = Target.OUT_RANGE;
			}
			if(currentRange != previousRange)
			{
				target.SwapRanges(previousRange, currentRange, this);
			}
		}
	}

	public Vector2 TargetDirection()
	{
		Vector2 targetVelocity = Vector2.zero;
		if (targetRange != currentRange)
		{
			if (targetRange < currentRange)
			{
				targetVelocity.x = 1;
			}
			else if (targetRange > currentRange)
			{
				targetVelocity.x = -1;
			}
		}
		return targetVelocity;
	}

	private void OnDestroy()
	{
		if(target != null)
		{
			target.RemoveFromAllRanges(this);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if(target != null)
		{
			Vector3 p = target.transform.position;
			Gizmos.DrawWireSphere(p, longRange);
			Gizmos.DrawWireSphere(p, midRange);
			Gizmos.DrawWireSphere(p, closeRange);
		}
	}
}
