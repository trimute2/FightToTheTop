using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

[CreateAssetMenu(fileName = "Move", menuName = "Data/Move Data", order = 1)]
public class MoveData : ScriptableObject {
	/*WARNING: if you change he names of any of these variables also change the
	name of the variable in the Move editor*/
#if UNITY_EDITOR
	public AnimatorController controller;
	public bool valid;
#endif
	public string animationStateName;
	public float length;
	public float frameRate;
	public float playBackSpeed = 1;

	/// <summary>the flags effected by the move</summary>
	[EnumFlags]
	public CommonFlags commonFlagsEffected;

	public AnimationCurve[] commonFlagsCurves = new AnimationCurve[Enum.GetValues(typeof(CommonFlags)).Length-1];
	private AnimationCurve combinedCurve;

	/*TODO: its possible that instead of saving a list of curves I save one that is parsed into several. but figuring out how to do that could be a head ache*/
	// turns out not as much of a pain as i thought, though it still needs some work to intigrate it better

	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// What flags are active according to the move
	/// </summary>
	/// <param name="time">the point in the move to evaluate</param>
	/// <returns>the integer value of the bit flag</returns>
	public int GetActiveFlags(float time)
	{
		if(commonFlagsEffected == CommonFlags.None)
		{
			return 0;
		}
		return (int)combinedCurve.Evaluate(time);
	}

	public CommonFlags GetTrackedFlags()
	{
		return commonFlagsEffected;
	}

	//for functions only called in the editor
#if UNITY_EDITOR
	#region EditorFunctions
	/// <summary> Resizes the array of animation curves <para /> used by move editor when a new flag is added</summary>
	public void ResizeCurves()
	{
		AnimationCurve[] temp = commonFlagsCurves;
		commonFlagsCurves = new AnimationCurve[Enum.GetValues(typeof(CommonFlags)).Length - 1];
		for (int i = 0; i < temp.Length; i++)
		{
			commonFlagsCurves[i] = temp[i];
		}
	}
	/// <summary> Used to verrify that the move will work</summary>
	public void Validate()
	{

		//TODO: write function;
		#region curveStuff
		combinedCurve = new AnimationCurve();
		//get what keyframes exist at what times
		List<float> keyFrames = new List<float>();
		for(int i = 0; i < commonFlagsCurves.Length; i++)
		{
			for(int j = 0; j<commonFlagsCurves[i].keys.Length; j++)
			{
				//while recording set the curve values to whole numbers and the tangent mode to constant
				commonFlagsCurves[i].keys[j].value = (int)commonFlagsCurves[i].keys[j].value;
				AnimationUtility.SetKeyLeftTangentMode(commonFlagsCurves[i], j, AnimationUtility.TangentMode.Constant);
				AnimationUtility.SetKeyRightTangentMode(commonFlagsCurves[i], j, AnimationUtility.TangentMode.Constant);
				float time = commonFlagsCurves[i].keys[j].time;
				bool contains = false;
				for(int k = 0; k < keyFrames.Count; k++)
				{
					if(keyFrames[k] == time)
					{
						contains = true;
						break;
					}
				}
				if (!contains)
				{
					keyFrames.Add(time);
				}
			}
		}
		//evaluate all the key frames across all the curves and add to the combined curve
		for(int i = 0; i < keyFrames.Count; i++)
		{
			Keyframe key = new Keyframe(keyFrames[i], 0);
			for(int k = 0; k < commonFlagsCurves.Length; k++)
			{
				int flag = (1 << k);
				if ((k & flag) == flag)
				{
					key.value += flag * (int)(commonFlagsCurves[k].Evaluate(key.time));
				}
			}
			combinedCurve.AddKey(key);
		}
		//set all the key frames so that the tangent mode is clamped
		for(int i = 0; i < combinedCurve.keys.Length; i++)
		{
			AnimationUtility.SetKeyLeftTangentMode(combinedCurve, i, AnimationUtility.TangentMode.Constant);
			AnimationUtility.SetKeyRightTangentMode(combinedCurve, i, AnimationUtility.TangentMode.Constant);
		}
		#endregion curveStuff
		valid = true;
	}
	#endregion EditorFunctions
#endif
}
