using System.Collections;
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
	SerializedProperty priority;
	SerializedProperty controllerProp;
	SerializedProperty cFCurvesProp;
	SerializedProperty vFCurvesProp;
	SerializedProperty vCurvesProp;
	SerializedProperty flagsProp;
	SerializedProperty flagData;
	List<AnimatorState> stateList;

	/** <summary>Does the name of the current animation state match
	 * one in the controller</summary>*/
	bool validName = true;

	bool[][] foldOuts = new bool[2][];
	bool foldOutValueCurves = false;


	private void OnEnable()
	{
		stateList = new List<AnimatorState>();
		animationName = serializedObject.FindProperty("animationStateName");
		length = serializedObject.FindProperty("length");
		frameRateProp = serializedObject.FindProperty("frameRate");
		speed = serializedObject.FindProperty("playBackSpeed");
		priority = serializedObject.FindProperty("priority");
		controllerProp = serializedObject.FindProperty("controller");
		cFCurvesProp = serializedObject.FindProperty("commonFlagsCurves");
		vFCurvesProp = serializedObject.FindProperty("valueFlagCurves");
		vCurvesProp = serializedObject.FindProperty("valueCurves");
		flagData = serializedObject.FindProperty("data");
		//TODO: better way of chacking that the curve sizes are correct
		SerializedProperty sp = cFCurvesProp.Copy();
		sp.Next(true);
		sp.Next(true);
		for(int i = 0; i < foldOuts.Length; i++)
		{
			foldOuts[i] = new bool[2];
			foldOuts[i][0] = false;
			foldOuts[i][1] = false;
		}
		if(sp.intValue != Enum.GetValues(typeof(CommonFlags)).Length - 1)
		{
			MoveData data = (MoveData)target;
			data.ResizeCurves();
		}
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
					frameRateProp.floatValue = 1.0f/clip.frameRate;
					validName = true;
				}
			}
		}
		EditorGUILayout.PropertyField(speed, new GUIContent("playback speed"));
		EditorGUILayout.PropertyField(priority, new GUIContent("priority"));
		DisplayFlags("commonFlags", cFCurvesProp, 0, typeof(CommonFlags), "Common Flags");
		DisplayFlags("valueFlags", vFCurvesProp, 1, typeof(ValueFlags), "Value");
		foldOutValueCurves = EditorGUILayout.Foldout(foldOutValueCurves, "Value Curves");
		if (foldOutValueCurves)
		{
			DrawFlagCurves(flagData.FindPropertyRelative("valueFlags").intValue, vCurvesProp, typeof(ValueFlags), -10, 20);
		}
		serializedObject.ApplyModifiedProperties();
		if (GUILayout.Button("Validate"))
		{
			MoveData data = (MoveData)target;
			data.Validate();
		}
		if (GUILayout.Button("Resize"))
		{
			MoveData data = (MoveData)target;
			data.ResizeCurves();
		}
		//base.OnInspectorGUI();
		//test(typeof(CommonFlags))
	}

	private void DrawFlagCurves(int flags, SerializedProperty curves, Type flagType, float minCurve = 0, float maxCurve = 1)
	{
		if(flags == 0 || !curves.isArray || !flagType.IsEnum)
		{
			return;
		}
		// partially based off of link https://answers.unity.com/questions/682932/using-generic-list-with-serializedproperty-inspect.html
		SerializedProperty sp = curves.Copy();
		int arrayLength = 0;
		int test = 1;
		sp.Next(true);
		sp.Next(true);
		string[] names = Enum.GetNames(flagType);

		arrayLength = sp.intValue;

		sp.Next(true);
		Rect range = new Rect(0, minCurve, length.floatValue / speed.floatValue, maxCurve);
		for (int i = 0; i < arrayLength; i++)
		{
			if ((flags & test) == test)
			{
				EditorGUILayout.CurveField(curves.GetArrayElementAtIndex(i), Color.green, range, new GUIContent(names[i + 1]));
			}
			test = test << 1;
		}
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

	private void DisplayFlags(string RelativePath, SerializedProperty curveProp, int fold, Type flagType, string name)
	{
		SerializedProperty property = flagData.FindPropertyRelative(RelativePath);
		int flag = property.intValue;
		foldOuts[fold][0] = EditorGUILayout.Foldout(foldOuts[fold][0], new GUIContent(name + " effected"));
		if (foldOuts[fold][0]) {
			EditorGUILayout.PropertyField(property, new GUIContent(name + " effected"));
		}
		foldOuts[fold][1] = EditorGUILayout.Foldout(foldOuts[fold][1], new GUIContent(name + " curves"));
		if (foldOuts[fold][1])
		{
			DrawFlagCurves(flag, curveProp, flagType);
		}
	}
}
