using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static GameManager _instance = null;
	//TODO: Track Entities through dictionary
	private List<int> freeIds;
	private int highestID;
	public static GameManager Instance
	{
		get
		{
			if(_instance == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<GameManager>();
			}
			return _instance;
		}
	}

	void Awake()
	{
		if(_instance != null && _instance != this)
		{
			Destroy(this);
			return;
		}
		_instance = this;
		freeIds = new List<int>();
		highestID = 0;
	}

	public int GetNewId()
	{
		if(freeIds.Count != 0)
		{
			int id = freeIds[freeIds.Count - 1];
			freeIds.RemoveAt(freeIds.Count - 1);
			return id;
		}
		highestID++;
		return highestID - 1;
	}

	public void ReleaseId(int id)
	{
		if(id == highestID - 1)
		{
			highestID--;
		}
		else
		{
			freeIds.Add(id);
		}
	}
}
