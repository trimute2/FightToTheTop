using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(FlagHandler))]
[RequireComponent(typeof(MoveHandler))]
[RequireComponent(typeof(EntityControllerComp))]
public class PlayerInputHandler : MonoBehaviour {

	public PlayerInput p;

	public float movementSpeed = 4.5f;
	public float airMovementSpeed = 6.5f;

	public WeaponData Weapon1;

	public WeaponData Weapon2;

	public List<MoveLink> playerMoves;

	private List<MoveLink> CurrentMoves;

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

	private bool doubleJump;
	public bool DoubleJump
	{
		get
		{
			return doubleJump;
		}
		set
		{
			doubleJump = value;
		}
	}

	private Vector2 directionalInput;

	private FlagHandler flagHandler;
	private MoveHandler moveHandler;
	private EntityControllerComp entityController;
	private HealthComponent healthComp;

	private bool actionsHooked;

	// Use this for initialization
	/*void Start () {
		moveHandler.GenericStateEvent += EnterGenericState;
	}*/

	private void Awake()
	{
		directionalInput = Vector2.zero;
		inputBuffers = new InputBuffer[inputNames.Length];
		for (int i = 0; i < inputBuffers.Length; i++)
		{
			inputBuffers[i] = new InputBuffer(inputNames[i]);
		}
		HookInputActions();
		flagHandler = GetComponent<FlagHandler>();
		moveHandler = GetComponent<MoveHandler>();
		entityController = GetComponent<EntityControllerComp>();
		healthComp = GetComponent<HealthComponent>();
		CurrentMoves = new List<MoveLink>();
		GenerateMoveList();
		entityController.LandingEvent += OnLandingEvent;
		GameManager.Instance.PlayerHealth = healthComp;
	}

	private void OnEnable()
	{
		HookInputActions();
	}

	private void HookInputActions()
	{
		if (actionsHooked)
		{
			return;
		}
		actionsHooked = true;
		SetUpBuffer(inputBuffers[0], p.gameplay.Weapon1);
		SetUpBuffer(inputBuffers[1], p.gameplay.Weapon2);
		SetUpBuffer(inputBuffers[2], p.gameplay.Jump);
		SetUpBuffer(inputBuffers[3], p.gameplay.Dodge);
		p.gameplay.Movement.performed += UpdateMoveInput;
		p.gameplay.Movement.cancelled += UpdateMoveInput;
		p.gameplay.Movement.Enable();
	}

	private void UnHookInputActions()
	{
		if (!actionsHooked)
		{
			return;
		}
		actionsHooked = false;
		ReleaseInputBuffer(inputBuffers[0], p.gameplay.Weapon1);
		ReleaseInputBuffer(inputBuffers[1], p.gameplay.Weapon2);
		ReleaseInputBuffer(inputBuffers[2], p.gameplay.Jump);
		ReleaseInputBuffer(inputBuffers[3], p.gameplay.Dodge);
		p.gameplay.Movement.performed -= UpdateMoveInput;
		p.gameplay.Movement.cancelled -= UpdateMoveInput;
	}

	private void OnLandingEvent()
	{
		doubleJump = true;
	}

	private void SetUpBuffer(InputBuffer buffer, InputAction inputAction)
	{
		inputAction.performed += buffer.ReadInput;
		inputAction.cancelled += buffer.ReadInput;
		inputAction.Enable();
	}

	private void ReleaseInputBuffer(InputBuffer buffer, InputAction inputAction)
	{
		inputAction.performed -= buffer.ReadInput;
		inputAction.cancelled -= buffer.ReadInput;
	}

	private void UpdateMoveInput(InputAction.CallbackContext ctx)
	{
		directionalInput = ctx.ReadValue<Vector2>();

	}

	// Update is called once per frame
	void Update () {
		if(Time.timeScale == 0)
		{
			return;
		}
		FlagData flagData = flagHandler.Flags;
		Vector2 movementInput = Vector2.zero;
		movementInput.x = directionalInput.x;
		movementInput.y = directionalInput.y;
		//movementInput.x = Input.GetAxisRaw("Horizontal");
		//movementInput.y = Input.GetAxisRaw("Vertical");
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
				targetVelocity.y = movementInput.y * movementSpeed;
			}
			float xSpeedMult = movementSpeed;
			if (!entityController.Grounded)
			{
				xSpeedMult = airMovementSpeed;
			}
			targetVelocity.x *= xSpeedMult;
		}
		if ((flagData.commonFlags & CommonFlags.MovementCancel) != CommonFlags.None)
		{
			if (movementInput.x != 0)
			{
				moveHandler.EnterGenericState("",0.3f);
			}
		}
		if ((flagData.commonFlags & CommonFlags.Dodgeing) != CommonFlags.None)
		{
			if (targetVelocity == Vector2.zero)
			{
				if (previousTarget == Vector2.zero)
				{
					targetVelocity.x = entityController.Facing * movementSpeed;
					targetVelocity *= 6f;
				}
				else
				{
					targetVelocity = previousTarget;
				}
			}
			else
			{
				//targetVelocity.y /= 2f;

				targetVelocity *= 6f;
			}

			
			if (moveHandler.OverDodge != 0)
			{
				targetVelocity.y = movementSpeed;
			}
			/*animatorVec = targetVelocity;
			animatorVec.x *= facing;
			return animatorVec;*/
		}

		entityController.TargetVelocity = targetVelocity;
		moveHandler.CheckMoves(CurrentMoves);
	}

	public void GenerateMoveList()
	{
		CurrentMoves.Clear();
		CurrentMoves.AddRange(playerMoves);
		if (Weapon1 != null)
		{
			CurrentMoves.AddRange(Weapon1.Moves);
		}
		if (Weapon2 != null)
		{
			CurrentMoves.AddRange(Weapon2.Moves);
		}
	}

	private void OnDestroy()
	{
		UnHookInputActions();
	}

	private void OnDisable()
	{
		UnHookInputActions();
	}
}
