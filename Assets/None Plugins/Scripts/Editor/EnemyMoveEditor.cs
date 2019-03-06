using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
[CustomEditor(typeof(EnemyMoveData))]
public class EnemyMoveEditor : MoveEditor
{
	private enum AttackLength
	{
		longRange = Target.LONG_RANGE,
		midRange = Target.MID_RANGE,
		closeRange = Target.CLOSE_RANGE,
	}

	bool EnemyDataFoldOut = false;

	SerializedProperty aType;
	SerializedProperty range;

	protected override void OnEnable()
	{
		base.OnEnable();
		aType = serializedObject.FindProperty("aType");
		range = serializedObject.FindProperty("range");
	}

	protected override void InheritedEditor()
	{
		EnemyDataFoldOut = EditorGUILayout.Foldout(EnemyDataFoldOut, new GUIContent("Enemy Data"));
		if (EnemyDataFoldOut)
		{
			SerializedProperty CloseRange = range.GetArrayElementAtIndex(Target.CLOSE_RANGE);
			EditorGUILayout.PropertyField(CloseRange, new GUIContent("Close Range"));
			SerializedProperty MidRange = range.GetArrayElementAtIndex(Target.MID_RANGE);
			EditorGUILayout.PropertyField(MidRange, new GUIContent("Mid Range"));
			SerializedProperty LongRange = range.GetArrayElementAtIndex(Target.LONG_RANGE);
			EditorGUILayout.PropertyField(LongRange, new GUIContent("Long Range"));

			EditorGUILayout.PropertyField(aType, new GUIContent("Attack Type"));
		}
	}
}
