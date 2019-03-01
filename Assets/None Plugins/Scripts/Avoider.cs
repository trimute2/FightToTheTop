using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoider : MonoBehaviour {
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
	[HideInInspector]
	public Transform avoidTransform;
	//bassed off of https://github.com/tutsplus/battle-circle-ai/blob/master/src/Assets/Scripts/AI/Avoider.cs
	// Use this for initialization

	private void OnTriggerEnter2D(Collider2D other)
	{
		Avoider av = other.GetComponent<Avoider>();
		if(av != null && thingsToAvoid.Contains(av.AvoiderType))
		{
			avoidTransform = other.transform;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		Avoider av = other.GetComponent<Avoider>();
		if (av != null && thingsToAvoid.Contains(av.AvoiderType))
		{
			avoidTransform = null;
		}
	}

	private void Awake()
	{
		thingsToAvoid = new List<string>();
	}
}
