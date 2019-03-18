using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
public class StunComponent : MonoBehaviour {

	public float damageTime = 0.3f;
	public float knockBackTime = 0.7f;

	private float hitTime = 0;
	private int vunlrabilityType = 0;

	private MoveHandler moveHandler;

	private bool stuned = false;

	private const int DAMAGE_STUN = 0;
	private const int KNOCKBACK_STUN = 1;

	private void Awake()
	{
		moveHandler = GetComponent<MoveHandler>();
	}

	// Update is called once per frame
	void Update () {
		if (stuned)
		{
			float waitTime = damageTime;
			if(vunlrabilityType == KNOCKBACK_STUN)
			{
				waitTime = knockBackTime;
			}
			if(Time.time - hitTime >= waitTime)
			{
				//TODO: get out of stun;
			}
		}
	}

	public void Stun()
	{

	}
}
