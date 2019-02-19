using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitBoxScript : MonoBehaviour {

	private EntityController entityController;
	private Collider2D damageCollider;
	private List<int> entitiesHit;
	// Use this for initialization
	void Start () {
		entityController = transform.root.GetComponent<EntityController>();
		damageCollider = this.GetComponent<Collider2D>();
		entitiesHit = new List<int>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		EntityController target = collision.transform.root.GetComponent<EntityController>();
		if (target != null)
		{
			if (!entitiesHit.Contains(target.EntityID))
			{
				entityController.HitEnemy(target);
				target.Damage(1);
				entitiesHit.Add(target.EntityID);
			}
		}
	}
	// Update is called once per frame

	public void ResetHitBox()
	{
		entitiesHit.Clear();
	}

	public void EnableHitBox()
	{
		damageCollider.enabled = true;
		ResetHitBox();
	}

	public void DisableHitBox()
	{
		damageCollider.enabled = false;
	}
}
