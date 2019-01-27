using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Data/Move Data", order = 1)]
public class MoveData : ScriptableObject {
	/*WARNING: if you change he names of any of these variables also change the
	name of the variable in the Move editor*/
#if UNITY_EDITOR
	public CharacterController controller;
#endif
	public string animationStateName;
	public float length;
	public float playBackSpeed;

	public AnimationCurve test;



	// Update is called once per frame
	void Update () {
		
	}
}
