using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityController {

	private bool active;
	private float DecisionRange;
	
	// Use this for initialization
	public override void Start()
	{
		base.Start();

	}

	// Update is called once per frame
	protected override void CheckMoves()
	{
		
	}

	protected override Vector2 EntityUpdate(Vector2 previousTarget)
	{
		return new Vector2(Mathf.Abs(targetVelocity.x), velocity.y);
	}


	protected override void ExecuteCondition(LinkCondition condition)
	{
		throw new System.NotImplementedException();
	}

}
