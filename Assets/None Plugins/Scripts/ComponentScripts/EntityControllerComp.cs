using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody2D))]
public class EntityControllerComp : MonoBehaviour {
	public delegate void landingListner();
	public event landingListner LandingEvent;
	private int facing = 1;
	public int Facing
	{
		get
		{
			return facing;
		}
		set
		{
			facing = value;
		}
	}

	public bool gravityOn = true;
	public bool GravityOn
	{
		get
		{
			return gravityOn;
		}
		set
		{
			if(value != gravityOn)
			{
				velocity.y = 0;
			}
			gravityOn = value;
		}
	}

	private bool allowXVelocity = false;
	public bool AllowXVelocity
	{
		set
		{
			allowXVelocity = value;
		}
	}

	public bool allowEntityCollision;
	public bool AllowEntityCollision
	{
		get
		{
			return allowEntityCollision;
		}
		set
		{
			allowEntityCollision = value;
		}
	}
	/**<summary>The rigid body for the entity</summary>*/
	private Rigidbody2D rb2d;
	// im including this due to an issue with one of unitys features
	private Collider2D entityCollider;

	private ContactFilter2D contactFilter;
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

	private Vector2 velocity;
	private Vector2 targetVelocity;
	private Vector2 corrections;

	public Vector2 Velocity
	{
		get
		{
			return velocity + targetVelocity;
		}
	}

	public Vector2 TargetVelocity
	{
		get
		{
			return targetVelocity;
		}
		set
		{
			targetVelocity = value;
		}
	}

	private bool grounded;

	public bool Grounded
	{
		get
		{
			return grounded;
		}
	}

	private float gravityModifier;
	public float GravityModifier
	{
		set
		{
			gravityModifier = value;
		}
	}


	// Use this for initialization
	void Start () {
		facing = (int)gameObject.transform.localScale.x;
		rb2d = GetComponent<Rigidbody2D>();
		entityCollider = GetComponent<Collider2D>();
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
		allowEntityCollision = true;
		gravityModifier = 1;
		corrections = Vector2.zero;
	}

	/*private void Update()
	{
		if (!allowXVelocity)
		{
			velocity.x = 0;
		}
	}*/

	private void FixedUpdate()
	{
		bool landing = !grounded;
		//so far there are no situation where i would want to keep an xvelocity but that may change later
		if (gravityOn)
		{
			velocity += Physics2D.gravity * Time.deltaTime * gravityModifier;
		}
		if (!allowXVelocity)
		{
			velocity.x = 0;
		}
		//Vector2 deltaPosition = velocity;
		corrections.x = Mathf.Clamp(Mathf.Abs(corrections.x), 2.5f, 3) * Math.Sign(corrections.x);

		Vector2 deltaPosition = velocity + corrections;
		deltaPosition += targetVelocity;
		deltaPosition *= Time.deltaTime;
		//corrections = Vector2.zero;
		grounded = false;
		Movement(deltaPosition * Vector2.right);
		Movement(deltaPosition * Vector2.up);
		if(landing && grounded && LandingEvent != null)
		{
			LandingEvent();
		}
	}

	void Movement(Vector2 move)
	{
		/* im copying this over from the prototype, a lot of it is based on a
		 tutorial for a platformer so I will mostlikely need to change parts of
		 it later*/
		float distance = move.magnitude;
		//int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + 0.01f);
		int count = entityCollider.Cast(move, contactFilter, hitBuffer, distance + 0.01f);
		hitBufferList.Clear();
		for (int i = 0; i < count; i++)
		{
			bool add = true;
			//TODO: another way of checking that does not involve tags
			if (hitBuffer[i].collider.tag == "Entity" || hitBuffer[i].collider.tag == "Player")
			{
				if (!allowEntityCollision
					||(hitBuffer[i].collider.GetComponent<EntityControllerComp>() != null &&
					!hitBuffer[i].collider.GetComponent<EntityControllerComp>().AllowEntityCollision))
				{
					add = false;
				}
			}
			if (add)
			{
				hitBufferList.Add(hitBuffer[i]);
			}
		}

		for (int i = 0; i < hitBufferList.Count; i++)
		{
			Vector2 currentNormal = hitBufferList[i].normal;
			if (currentNormal.y > 0.65f)
			{ EntityControllerComp ec = hitBufferList[i].transform.gameObject.GetComponent<EntityControllerComp>();
				if (ec == null)
				{
					grounded = true;
				}
			}
			float projection = Vector2.Dot(velocity, currentNormal);
			if (projection < 0)
			{
				velocity -= projection * currentNormal;
				//corrections.x = velocity.x;
				corrections.x = -(projection * currentNormal.x);
			}

			float modifiedDistance = hitBufferList[i].distance - 0.01f;
			distance = modifiedDistance < distance ? modifiedDistance : distance;
		}
		rb2d.position = rb2d.position + move.normalized * distance;
	}

	public bool TestOverlap()
	{
		int count = entityCollider.Cast(Vector2.zero, contactFilter, hitBuffer, 0.01f);
		hitBufferList.Clear();
		for (int i = 0; i < count; i++)
		{
			if (hitBuffer[i].collider.tag == "Entity")
			{
				return false;
			}
		}
		return true;
	}

	public void SetVelocity(Vector2 vel)
	{
		velocity = vel;
	}

	private void OnValidate()
	{
		Rigidbody2D vrb = GetComponent<Rigidbody2D>();
		if (!vrb.isKinematic)
		{
			vrb.bodyType = RigidbodyType2D.Kinematic;
			vrb.simulated = true;
			vrb.useFullKinematicContacts = true;
		}
	}
}
