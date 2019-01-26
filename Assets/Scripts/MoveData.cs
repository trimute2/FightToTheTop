using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Data/Move Data", order = 1)]
public class MoveData : ScriptableObject {
	public string AnimationStateName;
	public float length;
	public float playBackSpeed;

	public AnimationCurve test;
	
	// Update is called once per frame
	void Update () {
		
	}
}
