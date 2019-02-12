using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class LinkWindow : EditorWindow {
	private SerializedProperty link;
	private ReorderableList conditions;

	static void Init()
	{
		LinkWindow window = (LinkWindow)EditorWindow.GetWindow(typeof(LinkWindow));
		window.Show();
	}

	// Use this for initialization
	private void OnGUI()
	{
		if (link != null)
		{
			EditorGUILayout.PropertyField(link.FindPropertyRelative("move"), new GUIContent("move"));
			EditorGUILayout.PropertyField(link.FindPropertyRelative("priority"), new GUIContent("priority"));
			EditorGUILayout.PropertyField(link.FindPropertyRelative("minTime"), new GUIContent("earliest entry"));
			EditorGUILayout.PropertyField(link.FindPropertyRelative("maxTime"), new GUIContent("latest exit"));
			conditions.DoLayoutList();
			link.serializedObject.ApplyModifiedProperties();
		}
		else
		{
			GUILayout.Label("No Link loaded");
		}
	}

	public void SetLink(SerializedProperty nl)
	{
		link = nl;
		conditions = new ReorderableList(link.serializedObject, link.FindPropertyRelative("conditions"), true, true, true, true);
		conditions.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			SerializedProperty element = link.FindPropertyRelative("conditions").GetArrayElementAtIndex(index);
			EditorGUI.PropertyField(rect, element, GUIContent.none);
		};
		conditions.drawHeaderCallback = (Rect rect) =>
		{
			EditorGUI.LabelField(rect, "conditons");
		};
	}
}
