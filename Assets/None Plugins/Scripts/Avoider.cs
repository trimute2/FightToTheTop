using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoider : MonoBehaviour {
	//TODO: avoid multiple objects at the same time
	private string avoiderType;
	public string AvoiderType
	{
		get
		{
			return avoiderType;
		}
		set
		{
			avoiderType = value;
		}
	}

	private List<string> thingsToAvoid;
	public List<string> ThingsToAvoid
	{
		get
		{
			return thingsToAvoid;
		}
		set
		{
			thingsToAvoid = value;
		}
	}
	private List<Transform> avoidList;
	[HideInInspector]
	public Transform avoidTransform;
	private Vector3 avoidVector;
	public Vector3 AvoidVector
	{
		get
		{
			return avoidVector;
		}
	}
	//bassed off of https://github.com/tutsplus/battle-circle-ai/blob/master/src/Assets/Scripts/AI/Avoider.cs

	private void OnTriggerEnter2D(Collider2D other)
	{
		Avoider av = other.GetComponent<Avoider>();
		if(av != null && thingsToAvoid.Contains(av.AvoiderType))
		{
			//avoidTransform = other.transform;
			avoidList.Add(other.transform);
			//UpdateAvoiderTarget();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		Avoider av = other.GetComponent<Avoider>();
		if (av != null && thingsToAvoid.Contains(av.AvoiderType))
		{
			//avoidTransform = null;
			avoidList.Remove(other.transform);
			if(other.transform == avoidTransform)
			{
				avoidTransform = null;
				//UpdateAvoiderTarget();
			}
		}
	}

	private void UpdateAvoiderTarget()
	{
		if(avoidTransform == null)
		{
			if(avoidList.Count != 0)
			{
				avoidTransform = avoidList[0];
			}
		}
	}

	private void Update()
	{
		avoidVector = Vector3.zero;
		Vector3 currentPos = transform.parent.position;
		if(avoidList.Count != 0)
		{
			foreach(Transform t in avoidList)
			{
				Vector3 diff = currentPos - t.position;
				diff.Normalize();
				avoidVector += diff;
			}
			avoidVector /= avoidList.Count;
		}
	}

	private void Awake()
	{
		thingsToAvoid = new List<string>();
		avoidList = new List<Transform>();
		avoidVector = Vector3.zero;
	}
}
