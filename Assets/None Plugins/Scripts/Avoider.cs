using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoider : MonoBehaviour {
	public delegate void AvoiderChangeDelegate();
	public event AvoiderChangeDelegate ChangedAvoiding;
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
	private Avoider currentlyAvoiding;
	private Avoider CurrentlyAvoiding
	{
		set
		{
			currentlyAvoiding = value;
			if (currentlyAvoiding == null)
			{
				avoidTransform = null;
			}
			else
			{
				avoidTransform = currentlyAvoiding.transform.root;
			}
		}
		get
		{
			return currentlyAvoiding;
		}
	}
	private Vector3 avoidVector;
	public Vector3 AvoidVector
	{
		get
		{
			return avoidVector;
		}
	}

	private Targeter targeter;
	private bool hasTargeter;

	public bool Ignore { get; set; }
	//bassed off of https://github.com/tutsplus/battle-circle-ai/blob/master/src/Assets/Scripts/AI/Avoider.cs

	private void OnTriggerEnter2D(Collider2D other)
	{
		Avoider av = other.GetComponent<Avoider>();
		if(av != null && thingsToAvoid.Contains(av.AvoiderType))
		{
			avoidList.Add(av);
			UpdateAvoiderTarget();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		Avoider av = other.GetComponent<Avoider>();
		if (av != null && thingsToAvoid.Contains(av.AvoiderType))
		{
			avoidList.Remove(av);
			if(av == CurrentlyAvoiding)
			{
				CurrentlyAvoiding = null;
				UpdateAvoiderTarget();
			}
		}
	}

	private void UpdateAvoiderTarget()
	{
		if(CurrentlyAvoiding == null)
		{
			for(int i = 0; i< avoidList.Count; i++)
			{
				if (ShouldAvoid(avoidList[i]))
				{
					CurrentlyAvoiding = avoidList[i];
					ChangedAvoiding();
					break;
				}
			}
		}
	}

	//change this so that it is not updated each frame
	private void Update()
	{
		avoidVector = Vector3.zero;
		if (currentlyAvoiding != null && !ShouldAvoid(currentlyAvoiding))
		{
			CurrentlyAvoiding = null;
		}
		UpdateAvoiderTarget();
		/*Vector3 currentPos = transform.parent.position;
		if(avoidList.Count != 0)
		{
			int count = 0;
			foreach(Avoider a in avoidList)
			{
				//first check if i can just ignore 
				if (!a.Ignore &&
					//if either avoider doesnot have a targeter it can be added to the list without further checks
					(!hasTargeter || !a.hasTargeter ||
					//if both avoiders have the same target add them to the list
					(targeter.TargetRange == a.targeter.TargetRange) ||
					//if the other avoider isnt moving and the current ranges are not the same add it to the list
					(!a.targeter.Moving && targeter.CurrentRange != a.targeter.CurrentRange)
					))
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
		}*/
	}

	private bool ShouldAvoid(Avoider a)
	{
		return !a.Ignore &&
			//if either avoider doesnot have a targeter it can be added to the list without further checks
			(!hasTargeter || !a.hasTargeter ||
			//if both avoiders have the same target add them to the list
			(targeter.TargetRange == a.targeter.TargetRange) ||
			//if the other avoider isnt moving and the current ranges are not the same add it to the list
			(!a.targeter.Moving && targeter.CurrentRange != a.targeter.CurrentRange));
	}

	private void Awake()
	{
		thingsToAvoid = new List<string>();
		avoidList = new List<Avoider>();
		avoidVector = Vector3.zero;
		targeter = transform.root.GetComponent<Targeter>();
		hasTargeter = targeter != null;
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

	private void OnDrawGizmosSelected()
	{
		Debug.DrawRay(transform.position, avoidVector);
		/*if (avoidList != null)
		{
			foreach (Avoider a in avoidList)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(a.transform.position, 1);
			}
		}*/
		if(CurrentlyAvoiding != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(CurrentlyAvoiding.transform.position, 1);
		}
	}
}
