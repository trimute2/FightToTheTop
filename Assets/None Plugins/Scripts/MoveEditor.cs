﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

[CustomEditor(typeof(MoveData))]
public class MoveEditor : Editor {
	SerializedProperty animationName;
	SerializedProperty length;
	SerializedProperty frameRateProp;
	SerializedProperty speed;
	SerializedProperty controllerProp;
	SerializedProperty commonFlagsProp;
	SerializedProperty cFCurvesProp;
	List<AnimatorState> stateList;

	/** <summary>Does the name of the current animation state match
	 * one in the controller</summary>*/
	bool validName = true;

	bool foldOutCommonFlags = true;

	private void OnEnable()
	{
		stateList = new List<AnimatorState>();
		animationName = serializedObject.FindProperty("animationStateName");
		length = serializedObject.FindProperty("length");
		frameRateProp = serializedObject.FindProperty("frameRate");
		speed = serializedObject.FindProperty("playBackSpeed");
		controllerProp = serializedObject.FindProperty("controller");
		commonFlagsProp = serializedObject.FindProperty("commonFlagsEffected");
		cFCurvesProp = serializedObject.FindProperty("commonFlagsCurves");
		UpdateStateList();
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(controllerProp, new GUIContent("Controller"));
		if (GUI.changed)
		{
			UpdateStateList();
		}
		EditorGUI.BeginChangeCheck();
		if (!validName)
		{
			GUI.backgroundColor = Color.red;
		}
		else
		{
			GUI.backgroundColor = Color.white;
		}
		EditorGUILayout.PropertyField(animationName, new GUIContent("State Name"));
		if (EditorGUI.EndChangeCheck())
		{
			validName = false;
			int index = GetStateIndex(animationName.stringValue);
			if(index != -1)
			{
				AnimationClip clip = stateList[index].motion as AnimationClip;
				if(clip != null)
				{
					length.floatValue = clip.length;
					frameRateProp.floatValue = clip.frameRate;
					validName = true;
				}
			}
		}
		EditorGUILayout.PropertyField(speed, new GUIContent("playback speed"));
		foldOutCommonFlags = EditorGUILayout.Foldout(foldOutCommonFlags, "Flags effected");
		if (foldOutCommonFlags)
		{
			EditorGUILayout.PropertyField(commonFlagsProp, new GUIContent("Effected Flags"));
		}
		serializedObject.ApplyModifiedProperties();
		//base.OnInspectorGUI();
	}

	/**<summary>updates the list of states</summary>*/
	private void UpdateStateList()
	{
		stateList.Clear();
		AnimatorController controller = (AnimatorController)controllerProp.objectReferenceValue;
		if (controller != null)
		{
			AnimatorControllerLayer[] layers = controller.layers;
			for(int i = 0; i < layers.Length; i++)
			{
				StatesFromMachine(layers[i].stateMachine);
			}
		}
	}
	/**<summary>recercively checks sub state machines for states</summary>*/
	private void StatesFromMachine(AnimatorStateMachine stateMachine)
	{
		ChildAnimatorStateMachine[] stateMachines = stateMachine.stateMachines;
		for(int i = 0; i < stateMachines.Length; i++)
		{
			StatesFromMachine(stateMachines[i].stateMachine);
		}
		ChildAnimatorState[] states = stateMachine.states;
		for(int i = 0; i< states.Length; i++)
		{
			stateList.Add(states[i].state);
		}
	}

	private int GetStateIndex(string name)
	{
		for(int i = 0; i < stateList.Count; i++)
		{
			if(stateList[i].name == name)
			{
				return i;
			}
		}
		return -1;
	}
}
