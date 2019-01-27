using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveEditor : Editor {
	SerializedProperty animationName;
	SerializedProperty length;
	SerializedProperty speed;

	private void OnEnable()
	{
		animationName = serializedObject.FindProperty("animationStateName");
		length = serializedObject.FindProperty("length");
		speed = serializedObject.FindProperty("playBackSpeed");
	}
}
