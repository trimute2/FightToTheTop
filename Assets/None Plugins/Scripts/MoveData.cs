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
	public AnimationCurve[] commonFlagsCurves = new AnimationCurve[Enum.GetValues(typeof(CommonFlags)).Length - 1];
	public AnimationCurve[] valueFlagCurves = new AnimationCurve[Enum.GetValues(typeof(ValueFlags)).Length - 1];
#endif
	public string animationStateName;
	public float length;
	public float frameRate;
	public float playBackSpeed = 1;
	public int priority = 0;

	[SerializeField]
	public FlagData data = new FlagData(CommonFlags.None,ValueFlags.None);
	/// <summary>the flags effected by the move</summary>
	[EnumFlags]
	public CommonFlags commonFlagsEffected;
	// I spent several hours trying to find a way to save all of these enums in one data type, it really doesnt work

	public AnimationCurve[] valueCurves = new AnimationCurve[Enum.GetValues(typeof(ValueFlags)).Length - 1];

	[SerializeField]
	private AnimationCurve[] combinedFlagCurves = new AnimationCurve[Enum.GetValues(typeof(FlagTypes)).Length];
	/*TODO: its possible that instead of saving a list of curves I save one that is parsed into several. but figuring out how to do that could be a head ache*/
	// turns out not as much of a pain as i thought, though it still needs some work to intigrate it better

	// Update is called once per frame
	void Update () {
		
	}

	public int GetTrackedFlags(FlagTypes flag)
	{
		return data[flag];
	}

	public int GetActiveFlags(float time, FlagTypes flagType)
	{
		return (int)combinedFlagCurves[(int)flagType].Evaluate(time);
	}
	
	public float GetAnimatedValue(float time, ValueFlags valueFlag)
	{
		int i = 0;
		int flag = (int)valueFlag;
		while((flag & 1) != 1)
		{
			i += 1;
			flag = flag >> 1;
		}
		return valueCurves[i].Evaluate(time);
	}

	public bool EndMove(float time)
	{
		return time >= length / playBackSpeed;
	}
	//for functions only called in the editor
#if UNITY_EDITOR
	#region EditorFunctions
	/// <summary> Resizes the array of animation curves <para />
	/// used by move editor when a new flag is added</summary>
	public void ResizeCurves()
	{
		commonFlagsCurves = ResizeCurve(commonFlagsCurves, typeof(CommonFlags));
		valueFlagCurves = ResizeCurve(valueFlagCurves, typeof(ValueFlags));
		valueCurves = ResizeCurve(valueCurves, typeof(ValueFlags));
	}
	/// <summary>
	/// Resize array of curves to match the number of flags of an enum type
	/// </summary>
	/// <param name="oldCurves">the array you want resized</param>
	/// <param name="flagType">the enum type you are working to resize</param>
	/// <returns>an array of curves with a size that matches the number of flags in an enum</returns>
	private AnimationCurve[] ResizeCurve(AnimationCurve[] oldCurves, Type flagType)
	{
		AnimationCurve[] temp = oldCurves;
		oldCurves = new AnimationCurve[Enum.GetValues(flagType).Length - 1];
		int size = temp.Length < oldCurves.Length ? temp.Length : oldCurves.Length;
		for(int i = 0; i< size; i++)
		{
			oldCurves[i] = temp[i];
		}
		return oldCurves;
	}

	private AnimationCurve CombineFlagCurves(AnimationCurve[] flagCurves, int flagData)
	{
		List<float> keyFrames = new List<float>();
		int flagDatac = flagData;
		for (int i = 0; i < flagCurves.Length; i++)
		{
			if ((flagDatac & 1) != 0) {
				for (int j = 0; j < flagCurves[i].keys.Length; j++)
				{
					Keyframe ke = flagCurves[i].keys[j];
					if (ke.time <= length * playBackSpeed)
					{
						ke.value = (int)Math.Round(ke.value);
						flagCurves[i].MoveKey(j, ke);
						AnimationUtility.SetKeyLeftTangentMode(flagCurves[i], j, AnimationUtility.TangentMode.Constant);
						AnimationUtility.SetKeyRightTangentMode(flagCurves[i], j, AnimationUtility.TangentMode.Constant);
						float time = ke.time;
						bool contains = false;
						for (int k = 0; k < keyFrames.Count; k++)
						{
							if (keyFrames[k] == time)
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
					else
					{
						flagCurves[i].RemoveKey(j);
					}
				}
			}
			flagDatac = flagDatac >> 1;
		}
		AnimationCurve combinedCurve = new AnimationCurve();
		for(int i= 0; i < keyFrames.Count; i++)
		{
			Keyframe key = new Keyframe(keyFrames[i], 0);
			flagDatac = flagData;
			for (int j = 0; j < flagCurves.Length; j++)
			{
				if ((flagDatac & 1) != 0)
				{
					int flag = (1 << j);
					key.value += flag * (int)(flagCurves[j].Evaluate(key.time));
				}
				flagDatac >>= 1;
			}
			combinedCurve.AddKey(key);
		}
		for (int i = 0; i < combinedCurve.keys.Length; i++)
		{
			AnimationUtility.SetKeyLeftTangentMode(combinedCurve, i, AnimationUtility.TangentMode.Constant);
			AnimationUtility.SetKeyRightTangentMode(combinedCurve, i, AnimationUtility.TangentMode.Constant);
		}
		return combinedCurve;
	}
	/// <summary> Used to verrify that the move will work</summary>
	public void Validate()
	{
		//TODO: write function;
		#region curveStuff
		combinedFlagCurves[(int)FlagTypes.CommonFlags] = CombineFlagCurves(commonFlagsCurves, (int)data.commonFlags);
		combinedFlagCurves[(int)FlagTypes.ValueFlags] = CombineFlagCurves(valueFlagCurves, (int)data.valueFlags);
		#endregion curveStuff
		valid = true;
	}
	#endregion EditorFunctions
#endif
}
