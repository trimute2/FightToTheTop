using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
[RequireComponent(typeof(EntityController))]
public class PlayerInputHandler : MonoBehaviour {

	public float movementSpeed = 4.5f;

	public int maxDodge;

	public string Weapon1;

	public string Weapon2;

	public List<MoveLink> playerMoves;

	private InputBuffer[] inputBuffers;

	public InputBuffer this[int i]
	{
		get
		{
			return inputBuffers[i];
		}
	}

	private string[] inputNames = { "Weapon1", "Weapon2", "Jump", "Dodge" };

	public const int WEAPON1INDEX = 0;
	public const int WEAPON2INDEX = 1;

	private int dodgeCount;

	private bool doubleJump;

	private MoveHandler moveHandler;
	private EntityController entityController;

	// Use this for initialization
	void Start () {
		moveHandler.GenericStateEvent += EnterGenericState;
	}

	private void Awake()
	{
		inputBuffers = new InputBuffer[inputNames.Length];
		for (int i = 0; i < inputBuffers.Length; i++)
		{
			inputBuffers[i] = new InputBuffer(inputNames[i]);
		}
		dodgeCount = 0;
		moveHandler = GetComponent<MoveHandler>();
		entityController = GetComponent<EntityController>();
	}

	// Update is called once per frame
	void Update () {
		FlagData flagData = moveHandler.Flags;
		Vector2 movementInput = Vector2.zero;
		movementInput.x = Input.GetAxisRaw("Horizontal");
		movementInput.y = Input.GetAxisRaw("Vertical");
		Vector2 previousTarget = entityController.TargetVelocity;
		Vector2 targetVelocity = Vector2.zero;
		foreach (InputBuffer b in inputBuffers)
		{
			b.Update();
		}
		if ((flagData.commonFlags & CommonFlags.CanTurn) != CommonFlags.None)
		{
			if (movementInput.x != 0)
			{
				Vector3 sca = transform.localScale;
				if (movementInput.x > 0)
				{
					sca.x = 1;
					entityController.Facing = 1;
				}
				else
				{
					sca.x = -1;
					entityController.Facing = -1;
				}
				transform.localScale = sca;
			}
		}
		if ((flagData.commonFlags & CommonFlags.MoveWithInput) != CommonFlags.None)
		{
			targetVelocity.x = movementInput.x;
			if ((flagData.commonFlags & CommonFlags.YMovement) != CommonFlags.None)
			{
				targetVelocity.y = movementInput.y;
			}
			targetVelocity *= movementSpeed;
		}
		if ((flagData.commonFlags & CommonFlags.MovementCancel) != CommonFlags.None)
		{
			if (movementInput.x != 0)
			{
				moveHandler.EnterGenericState(0.3f);
			}
		}
		if ((flagData.commonFlags & CommonFlags.Dodgeing) != CommonFlags.None)
		{
			if (targetVelocity == Vector2.zero)
			{
				if (previousTarget == Vector2.zero)
				{
					targetVelocity.x = entityController.Facing * movementSpeed;
					targetVelocity *= 3f;
				}
				else
				{
					targetVelocity = previousTarget;
				}
			}
			else
			{
				targetVelocity.y /= 2f;

				targetVelocity *= 3f;
			}

			/*
			if (overDodge != 0)
			{
				targetVelocity.y = movementSpeed;
			}
			animatorVec = targetVelocity;
			animatorVec.x *= facing;
			return animatorVec;*/
		}

		/*
		if (vulnrabilityTimer != 0)
		{
			vulnrabilityTimer -= Time.deltaTime;
			if (vulnrabilityTimer <= 0)
			{
				vulnrabilityTimer = 0;
				velocity.x = 0;
				EnterGenericState();
			}
		}
		else
		{
			CheckMoves();
		}*/

		entityController.TargetVelocity = targetVelocity;
	}

	public void EnterGenericState()
	{
		dodgeCount = 0;
	}

	
}
