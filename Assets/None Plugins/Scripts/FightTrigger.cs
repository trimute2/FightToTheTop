using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FightTrigger : MonoBehaviour {

	//private Collider2D FightArea;

	public Enemy enemy;
	// Use this for initialization
	/*void Start () {
		FightArea = GetComponent<Collider2D>();
	}*/

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerController p = collision.transform.root.GetComponent<PlayerController>();
		if (p != null)
		{
			enemy.Targetv = p;
			Destroy(this);
		}
	}
}
