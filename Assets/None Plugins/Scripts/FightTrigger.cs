using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FightTrigger : MonoBehaviour {

	//private Collider2D FightArea;

	public List<Enemy> enemies;
	// Use this for initialization
	/*void Start () {
		FightArea = GetComponent<Collider2D>();
	}*/

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Target p = collision.transform.root.GetComponent<Target>();
		if (p != null)
		{
			foreach(Enemy e in enemies)
			{
				e.CurrentTarget = p;
			}
			Destroy(this);
		}
	}
}
