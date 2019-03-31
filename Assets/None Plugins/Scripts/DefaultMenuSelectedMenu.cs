using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;

public class DefaultMenuSelectedMenu : MonoBehaviour
{
	//A lot of this is coppied from UIActionInput Module Since there is not a lot of documentation
	//on input Actions
	[SerializeField] private InputActionProperty m_MoveAction;
	[SerializeField] private List<GameObject> m_defaultSelections = null;
	[NonSerialized] private bool m_ActionsHooked;
	[NonSerialized] private Action<InputAction.CallbackContext> m_OnActionDelegate;

	private EventSystem eventSystem;
	public InputActionProperty move
	{
		get => m_MoveAction;
		set => SwapAction(ref m_MoveAction, value, m_ActionsHooked, OnAction);
	}

	void OnEnable()
	{
		eventSystem = GetComponent<EventSystem>();

		HookActions();
	}

	private void OnDisable()
	{
		UnhookActions();
	}

	private void OnDestroy()
	{
		UnhookActions();
	}

	void OnAction(InputAction.CallbackContext context)
	{
		var action = context.action;
		if (action == m_MoveAction && eventSystem.currentSelectedGameObject == null)
		{
			foreach (GameObject obj in m_defaultSelections)
			{
				if (obj.activeInHierarchy)
				{
					eventSystem.SetSelectedGameObject(obj);
					break;
				}
			}
		}
	}

	private static void SwapAction(ref InputActionProperty property, InputActionProperty newValue, bool actionsHooked, Action<InputAction.CallbackContext> actionCallback)
	{
		if (property != null && actionsHooked)
		{
			property.action.performed -= actionCallback;
			property.action.cancelled -= actionCallback;
		}
		property = newValue;
		if (newValue != null && actionsHooked)
		{
			property.action.performed += actionCallback;
			property.action.cancelled += actionCallback;
		}
	}

	private void HookActions()
	{
		if (m_ActionsHooked)
		{
			return;
		}
		m_ActionsHooked = true;

		if (m_OnActionDelegate == null)
		{
			m_OnActionDelegate = OnAction;
		}

		var moveAction = m_MoveAction.action;
		if (moveAction != null)
		{
			moveAction.performed += m_OnActionDelegate;
			moveAction.cancelled += m_OnActionDelegate;
		}

	}

	private void UnhookActions()
	{
		if (!m_ActionsHooked) {
			return;
		}
		m_ActionsHooked = false;

		var moveAction = m_MoveAction.action;
		if (moveAction != null)
		{
			moveAction.performed -= m_OnActionDelegate;
			moveAction.cancelled -= m_OnActionDelegate;
		}
	}

	public void EnableAllActions()
	{
		var moveAction = m_MoveAction.action;
		if (moveAction != null)
		{
			moveAction.Enable();
		}

		Debug.Log("action " + moveAction.enabled);
	}

	public void DisableAllActions()
	{
		var moveAction = m_MoveAction.action;
		if (moveAction != null)
		{
			moveAction.Disable();
		}
	}
}