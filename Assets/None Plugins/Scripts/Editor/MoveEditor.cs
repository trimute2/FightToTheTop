using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorInternal;

[CustomEditor(typeof(MoveData))]
public class MoveEditor : Editor {
	SerializedProperty animationName;
	SerializedProperty length;
	SerializedProperty frameRateProp;
	SerializedProperty speed;
	SerializedProperty damage;
	SerializedProperty hitStun;
	SerializedProperty hitStunFrame;
	SerializedProperty knockBack;
	SerializedProperty controllerProp;
	SerializedProperty cFCurvesProp;
	SerializedProperty vFCurvesProp;
	SerializedProperty vCurvesProp;
	SerializedProperty flagsProp;
	SerializedProperty flagData;
	SerializedProperty endTime;
	SerializedProperty holdTime;
	SerializedProperty hitVisualEffect;
	SerializedProperty EffectsOnEnter;
	SerializedProperty EffectsOnExit;
	List<AnimatorState> stateList;
	ReorderableList links;
	SerializedProperty HitTargetEffects;
	SerializedProperty HitUserEffects;

	/** <summary>Does the name of the current animation state match
	 * one in the controller</summary>*/
	bool validName = true;

	bool[][] foldOuts = new bool[2][];
	bool foldOutValueCurves = false;


	protected virtual void OnEnable()
	{
		stateList = new List<AnimatorState>();
		animationName = serializedObject.FindProperty("animationStateName");
		length = serializedObject.FindProperty("length");
		frameRateProp = serializedObject.FindProperty("frameRate");
		speed = serializedObject.FindProperty("playBackSpeed");
		damage = serializedObject.FindProperty("damage");
		hitStun = serializedObject.FindProperty("hitStun");
		hitStunFrame = serializedObject.FindProperty("hitStunFrame");
		knockBack = serializedObject.FindProperty("knockBack");
		controllerProp = serializedObject.FindProperty("controller");
		cFCurvesProp = serializedObject.FindProperty("commonFlagsCurves");
		vFCurvesProp = serializedObject.FindProperty("valueFlagCurves");
		vCurvesProp = serializedObject.FindProperty("valueCurves");
		flagData = serializedObject.FindProperty("data");
		holdTime = serializedObject.FindProperty("holdTime");
		endTime = serializedObject.FindProperty("endTime");
		hitVisualEffect = serializedObject.FindProperty("HitVisualEffect");
		EffectsOnEnter = serializedObject.FindProperty("EffectsOnEnter");
		EffectsOnExit = serializedObject.FindProperty("EffectsOnExit");
		links = new ReorderableList(serializedObject, serializedObject.FindProperty("links"), true, true, true, true);
		links.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			SerializedProperty element = links.serializedProperty.GetArrayElementAtIndex(index);
			//rect.y += 2;
			//EditorGUI.LabelField(rect, new GUIContent("Move"));
			EditorGUI.PropertyField(new Rect(rect.x,rect.y,rect.width*0.50f,rect.height),element.FindPropertyRelative("move"), GUIContent.none);
			EditorGUI.PropertyField(new Rect(rect.x+rect.width * 0.5f, rect.y, rect.width * 0.3f, rect.height), element.FindPropertyRelative("priority"), GUIContent.none);
			if (GUI.Button(new Rect(rect.width * 0.9f, rect.y,rect.width*0.15f,rect.height),new GUIContent("detail"))){
				LinkWindow window = (LinkWindow)EditorWindow.GetWindow(typeof(LinkWindow));
				window.SetLink(element);
				window.Show();
			}
		};
		links.drawHeaderCallback = (Rect rect) =>
		{
			EditorGUI.LabelField(rect, "Links");
		};
		HitTargetEffects = serializedObject.FindProperty("HitTargetEffects");
		
		HitUserEffects = serializedObject.FindProperty("HitUserEffects");
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
		EditorGUILayout.PropertyField(holdTime, new GUIContent("Hold Time"));
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
				AnimationClip clip;
				if (stateList[index].motion is UnityEditor.Animations.BlendTree)
				{
					clip = (stateList[index].motion as UnityEditor.Animations.BlendTree).children[0].motion as AnimationClip;
				}
				else
				{
					clip = stateList[index].motion as AnimationClip;
				}
				if(clip != null)
				{
					length.floatValue = clip.length;
					endTime.floatValue = length.floatValue + holdTime.floatValue;
					frameRateProp.floatValue = 1.0f/clip.frameRate;
					validName = true;
				}
			}
		}
		EditorGUILayout.PropertyField(speed, new GUIContent("playback speed"));
		EditorGUILayout.PropertyField(damage, new GUIContent("damage"));
		EditorGUILayout.PropertyField(hitStun, new GUIContent("Hit Stun"));
		EditorGUILayout.PropertyField(hitStunFrame, new GUIContent("Hit Stun Frame"));
		EditorGUILayout.PropertyField(knockBack, new GUIContent("knockBack"));
		InheritedEditor();
		DisplayFlags("commonFlags", cFCurvesProp, 0, typeof(CommonFlags), "Common Flags");
		DisplayFlags("valueFlags", vFCurvesProp, 1, typeof(ValueFlags), "Value Flags");
		foldOutValueCurves = EditorGUILayout.Foldout(foldOutValueCurves, "Value Curves");
		if (foldOutValueCurves)
		{
			DrawFlagCurves(flagData.FindPropertyRelative("valueFlags").intValue, vCurvesProp, typeof(ValueFlags), false);
		}
		links.DoLayoutList();
		EditorGUILayout.PropertyField(HitTargetEffects, true);
		EditorGUILayout.PropertyField(HitUserEffects, true);
		EditorGUILayout.PropertyField(EffectsOnEnter, true);
		EditorGUILayout.PropertyField(EffectsOnExit, true);
		EditorGUILayout.PropertyField(hitVisualEffect, new GUIContent("Visual Effect"));
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

	protected virtual void InheritedEditor()
	{

	}

	private void DrawFlagCurves(int flags, SerializedProperty curves, Type flagType, bool restraint = true, float minCurve = 0, float maxCurve = 1)
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
		Rect range = new Rect(0, minCurve, (length.floatValue / speed.floatValue) + holdTime.floatValue, maxCurve);
		for (int i = 0; i < arrayLength; i++)
		{
			if ((flags & test) == test)
			{
				if (restraint)
				{
					EditorGUILayout.CurveField(curves.GetArrayElementAtIndex(i), Color.green, range, new GUIContent(names[i + 1]));
				}
				else
				{
					EditorGUILayout.PropertyField(curves.GetArrayElementAtIndex(i), new GUIContent(names[i + 1]));
				}
			}
			test = test << 1;
		}
	}

	/**<summary>updates the list of states</summary>*/
	private void UpdateStateList()
	{
		stateList.Clear();
		UnityEditor.Animations.AnimatorController controller = (UnityEditor.Animations.AnimatorController)controllerProp.objectReferenceValue;
		if (controller != null)
		{
			UnityEditor.Animations.AnimatorControllerLayer[] layers = controller.layers;
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
