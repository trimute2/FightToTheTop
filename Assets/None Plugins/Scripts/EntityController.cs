using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

	public float movementSpeed = 4.5f;

	/**<summary>The direction the player is facing</summary>*/
	private int Facing = 1;
	/**<summary>The rigid body for the entity</summary>*/
	private Rigidbody2D rb2d;
	/**<summary>The animator for the entity</summary>*/
	private Animator animator;
	private CommonFlags controllerFlags;

	// physics variables
	private ContactFilter2D contactFilter;
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

	private Vector2 velocity;
	private Vector2 targetVelocity;

	private bool grounded;

	private float moveTime;

	private MoveData currentMove;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		controllerFlags = CommonFlags.MoveWithInput;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
		currentMove = null;
	}

	private void FixedUpdate()
	{
		velocity += Physics2D.gravity * Time.deltaTime;
		Vector2 deltaPosition = velocity;
		deltaPosition += targetVelocity;
		deltaPosition *= Time.deltaTime;
		grounded = false;
		Movement(deltaPosition * Vector2.right);
		Movement(deltaPosition * Vector2.up);
	}

	void Movement(Vector2 move)
	{
		/* im copying this over from the prototype, a lot of it is based on a
		 tutorial for a platformer so I will mostlikely need to change parts of
		 it later*/
		float distance = move.magnitude;
		int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + 0.01f);
		hitBufferList.Clear();
		for(int i = 0; i < count; i++)
		{
			hitBufferList.Add(hitBuffer[i]);
		}

		for(int i = 0; i < hitBufferList.Count; i++)
		{
			Vector2 currentNormal = hitBufferList[i].normal;
			if(currentNormal.y > 0.65f)
			{
				grounded = true;
			}
			float projection = Vector2.Dot(velocity, currentNormal);
			if(projection< 0)
			{
				velocity -= projection * currentNormal;
			}

			float modifiedDistance = hitBufferList[i].distance - 0.01f;
			distance = modifiedDistance < distance ? modifiedDistance : distance;
		}
		rb2d.position = rb2d.position + move.normalized * distance;
	}

	// Update is called once per frame
	void Update () {
		targetVelocity = Vector2.zero;
		Vector2 movementInput = Vector2.zero;
		movementInput.x = Input.GetAxisRaw("Horizontal");
		movementInput.y = Input.GetAxisRaw("Vertical");
		//TODO: method for checking moves
		if ((controllerFlags & CommonFlags.MoveWithInput) != CommonFlags.None)
		{
			
			if (movementInput.x != 0)
			{
				Vector3 sca = transform.localScale;
				if (movementInput.x > 0)
				{
					sca.x = 1;
					Facing = 1;
				}
				else
				{
					sca.x = -1;
					Facing = -1;
				}
				transform.localScale = sca;
			}
			targetVelocity.x = movementInput.x * movementSpeed;
		}
		animator.SetFloat("VelocityX", Mathf.Abs(targetVelocity.x));
		animator.SetFloat("VelocityY", velocity.y);
		//Debug.Log(1 / Time.deltaTime);
	}

	//late update is called once per frame after the internal animation update
	private void LateUpdate()
	{
		if(currentMove != null)
		{
			if (moveTime == -1)
			{
				moveTime = 0;
			}
			else
			{
				moveTime += Time.deltaTime;
			}
			//get the state of the flags tracked by the move
			CommonFlags moveFlags = (CommonFlags)currentMove.GetActiveFlags(moveTime);
			//activate the flags that are active according to the move
			controllerFlags |= moveFlags;
			//in the moveflags variable activate all flags not tracked by the move
			moveFlags |= ~currentMove.GetTrackedFlags();
			//turn off all flags not active acording to the move while leaving those not tracked by the move in their original state
			controllerFlags &= moveFlags;
			
		}
	}
}
