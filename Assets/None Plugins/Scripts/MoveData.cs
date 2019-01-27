using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

[CreateAssetMenu(fileName = "Move", menuName = "Data/Move Data", order = 1)]
public class MoveData : ScriptableObject {
	/*WARNING: if you change he names of any of these variables also change the
	name of the variable in the Move editor*/
#if UNITY_EDITOR
	public AnimatorController controller;
#endif
	public string animationStateName;
	public float length;
	public float frameRate;
	public float playBackSpeed;

	/// <summary>the flags effected by the move</summary>
	[EnumFlags]
	public CommonFlags commonFlagsEffected;

	public List<AnimationCurve> commonFlagsCurves = new List<AnimationCurve>(Enum.GetValues(typeof(CommonFlags)).Length-1);

	/*TODO: its possible that instead of saving a list of curve I save one that is parsed into several. but figuring out how to do that could be a head ache*/

	// Update is called once per frame
	void Update () {
		
	}
}
