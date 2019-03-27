using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEditor;

public class MatchPosition : ScriptableWizard
{

	public Transform target;
	public Transform targetPosition;
	private static Transform LastTarget;
	private static Transform LastTargetPosition;

	[MenuItem("Debug/Match Position")]
	static void CreateWizard()
	{
		MatchPosition p = DisplayWizard<MatchPosition>("MatchPosition", "Match Position", "Match Rotation");
		p.target = LastTarget;
		p.targetPosition = LastTargetPosition;
	}

	void OnWizardCreate()
	{
		UpdateTarget();
		target.position = targetPosition.position;
	}

	private void OnWizardOtherButton()
	{
		UpdateTarget();
		target.rotation = targetPosition.rotation;
	}

	private void UpdateTarget()
	{
		LastTarget = target;
		LastTargetPosition = targetPosition;
	}
}
