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
	private List<Avoider> avoidList;
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

	public bool Ignore { get; set; }
	//bassed off of https://github.com/tutsplus/battle-circle-ai/blob/master/src/Assets/Scripts/AI/Avoider.cs

	private void OnTriggerEnter2D(Collider2D other)
	{
		Avoider av = other.GetComponent<Avoider>();
		if(av != null && thingsToAvoid.Contains(av.AvoiderType))
		{
			avoidList.Add(av);
			//UpdateAvoiderTarget();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		Avoider av = other.GetComponent<Avoider>();
		if (av != null && thingsToAvoid.Contains(av.AvoiderType))
		{
			avoidList.Remove(av);
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
				avoidTransform = avoidList[0].transform;
			}
		}
	}

	private void Update()
	{
		avoidVector = Vector3.zero;
		Vector3 currentPos = transform.parent.position;
		if(avoidList.Count != 0)
		{
			int count = 0;
			foreach(Avoider a in avoidList)
			{
				if (!a.Ignore)
				{
					Vector3 diff = currentPos - a.transform.position;
					diff.Normalize();
					avoidVector += diff;
					count++;
				}
			}
			if (count != 0)
			{
				avoidVector /= count;
			}
		}
	}

	private void Awake()
	{
		thingsToAvoid = new List<string>();
		avoidList = new List<Avoider>();
		avoidVector = Vector3.zero;
	}

	public void OnEnterGenericState()
	{
		Ignore = false;
	}

#if UNITY_EDITOR
	public static void AvoiderValidationCheck(GameObject obj)
	{
		Avoider av = obj.GetComponentInChildren<Avoider>();
		if(av == null)
		{
			System.Type[] Components = { typeof(CapsuleCollider2D), typeof(Avoider) };
			GameObject gameObject = new GameObject("Avoider", Components);
			gameObject.transform.SetParent(obj.transform, false);
		}
	}
#endif
}
