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
		MatchPosition p = DisplayWizard<MatchPosition>("MatchPosition", "Match");
		p.target = LastTarget;
		p.targetPosition = LastTargetPosition;
	}

	void OnWizardCreate()
	{
		LastTarget = target;
		LastTargetPosition = targetPosition;
		target.position = targetPosition.position;
	}
}
