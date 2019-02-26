using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	private static EnemyManager _instance = null;

	public static EnemyManager Instance
	{
		get
		{
			if(_instance == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<EnemyManager>();
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

	}
}
