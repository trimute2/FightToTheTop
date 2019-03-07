using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MoveHandler : MonoBehaviour {
	public delegate void GenericStateListner();

	public event GenericStateListner GenericStateEvent;

	private FlagData flagData;
	public CommonFlags CommonFlags
	{
		get
		{
			return flagData.commonFlags;
		}
	}
	public ValueFlags ValueFlags
	{
		get
		{
			return flagData.valueFlags;
		}
	}
	private CommonFlags defaultFlagValues = CommonFlags.CanTurn | CommonFlags.MoveWithInput | CommonFlags.CanAttack;
	private EntityController entityController;
	private Animator animator;
	/// <summary>
	/// The current move
	/// </summary>
	private MoveData currentMove;

	private float moveTime;

	private bool listenToMoveMotion;

	public bool GenericState
	{
		get
		{
			return currentMove == null;
		}
	}

	// Use this for initialization
	void Start () {
		currentMove = null;
		animator = GetComponent<Animator>();
		flagData = new FlagData(defaultFlagValues, ValueFlags.None);
		moveTime = 0;
		listenToMoveMotion = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private bool CheckCondition(LinkCondition condition)
	{
		switch (condition.conditionType)
		{
			case ConditionType.groundCondition:
				if(entityController != null)
				{
					return entityController.Grounded == condition.boolSetting;
				}
				return true;
			default:
				return false;
		}
	}

	public bool CheckLink(MoveLink link)
	{
		foreach(LinkCondition lk in link.conditions)
		{
			if (!CheckCondition(lk))
			{
				return false;
			}
		}
		return true;
	}

	public void EnterGenericState(float transitionTime = 0)
	{
		if (currentMove != null)
		{
			foreach (EntityEffects e in currentMove.EffectsOnExit)
			{
				//e.Effect(this);
			}
		}
		string toPlay;
		if(entityController == null || entityController.Grounded)
		{
			toPlay = "Idle";
		}else if(entityController.Velocity.y < 0)
		{
			toPlay = "Falling";
		}
		else
		{
			toPlay = "GoingUp";
		}
		animator.CrossFade(toPlay, transitionTime);
		flagData.valueFlags = ValueFlags.None;
		flagData.commonFlags = defaultFlagValues;
		animator.speed = 1;

		currentMove = null;
		if (GenericStateEvent != null)
		{
			GenericStateEvent();
		}
	}

	private void LateUpdate()
	{
		if (currentMove != null)
		{
			moveTime = moveTime == -1 ? 0 : moveTime + Time.deltaTime;
			//get the state of the flags tracked by the move
			CommonFlags moveFlags = (CommonFlags)currentMove.GetActiveFlags(moveTime, FlagTypes.CommonFlags);
			//activate the flags that are active according to the move
			//controllerFlags |= moveFlags;
			flagData.commonFlags |= moveFlags;
			//in the moveflags variable activate all flags not tracked by the move
			moveFlags |= (CommonFlags)~currentMove.GetTrackedFlags(FlagTypes.CommonFlags);
			//turn off all flags not active acording to the move while leaving those not tracked by the move in their original state
			//controllerFlags &= moveFlags;
			flagData.commonFlags &= moveFlags;

			flagData.valueFlags = (ValueFlags)currentMove.GetActiveFlags(moveTime, FlagTypes.ValueFlags);

			if (listenToMoveMotion && entityController != null && flagData.valueFlags != ValueFlags.None)
			{
				Vector2 targetVelocity = entityController.TargetVelocity;
				float val = 0;
				if (GetValue(ValueFlags.xVelocity, out val))
				{
					//TODO: get facing so this works
					//targetVelocity.x = val * facing;
				}
				if (GetValue(ValueFlags.yVelocity, out val))
				{
					targetVelocity.y = val;
				}
				entityController.TargetVelocity = targetVelocity;
			}
			if (currentMove.EndMove(moveTime))
			{
				EnterGenericState();
			}
		}
	}

	private bool GetValue(ValueFlags flag, out float value)
	{
		if ((flagData.valueFlags & flag) != ValueFlags.None)
		{
			value = currentMove.GetAnimatedValue(moveTime, flag);
			return true;
		}
		value = 0;
		return false;
	}
}
