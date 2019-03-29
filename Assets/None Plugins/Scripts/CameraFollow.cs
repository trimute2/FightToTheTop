using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {
	//this script is mainly bassed off of an example project by unity, will diverge from the example later 
	public Transform Target;
	public Rect deadzone;
	private Vector3 smoothPos;

	private Vector3 _currentVelocity;
	//private Camera _camera;

	// Use this for initialization
	void Start () {
		//Vector3 pos = Target.position;
		//pos.z = transform.position.z;
		//transform.position = pos;
		smoothPos = Target.position;
		smoothPos.z = transform.position.z;
		_currentVelocity = Vector3.zero;
		//_camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		float localX = Target.position.x - transform.position.x;
		float localY = Target.position.y - transform.position.y;

		if (localX < deadzone.xMin)
		{
			smoothPos.x += localX - deadzone.xMin;
		}else if(localX > deadzone.xMax)
		{
			smoothPos.x += localX - deadzone.xMax;
		}

		if (localY < deadzone.yMin)
		{
			smoothPos.y += localY - deadzone.yMin;
		}
		else if (localY > deadzone.yMax)
		{
			smoothPos.y += localY - deadzone.yMax;
		}

		Vector3 current = transform.position;
		current.x = smoothPos.x;
		current.y = smoothPos.y;
		transform.position = smoothPos;
		//transform.position = Vector3.SmoothDamp(current, smoothPos, ref _currentVelocity, 0.1f);
	}

}

#if UNITY_EDITOR

[CustomEditor(typeof(CameraFollow))]
public class DeadZonEditor : Editor
{
	public void OnSceneGUI()
	{
		CameraFollow cam = target as CameraFollow;
		Vector3 pos = cam.transform.position;
		pos.z = 0;
		Vector3[] vert =
		{
			pos + new Vector3(cam.deadzone.xMin, cam.deadzone.yMin, 0),
			pos + new Vector3(cam.deadzone.xMax, cam.deadzone.yMin, 0),
			pos + new Vector3(cam.deadzone.xMax, cam.deadzone.yMax, 0),
			pos + new Vector3(cam.deadzone.xMin, cam.deadzone.yMax, 0)
		};

		Color transp = new Color(0, 0, 0, 0);
		Handles.DrawSolidRectangleWithOutline(vert, transp, Color.red);
	}
}
#endif