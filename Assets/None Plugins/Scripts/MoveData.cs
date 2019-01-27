using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

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

	public AnimationCurve test;



	// Update is called once per frame
	void Update () {
		
	}
}
