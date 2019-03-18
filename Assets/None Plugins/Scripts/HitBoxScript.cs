using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitBoxScript : MonoBehaviour {

	private MoveHandler moveHandler;
	private EntityController entityController;
	private Collider2D damageCollider;
	private int damage;
	private Vector2 knockBack;
	private List<int> entitiesHit;
	// Use this for initialization
	void Start () {
		entityController = transform.root.GetComponent<EntityController>();
		damageCollider = this.GetComponent<Collider2D>();
		damageCollider.enabled = false;
		entitiesHit = new List<int>();
		damage = 0;
		knockBack = Vector2.zero;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		HealthComponent target = collision.transform.root.GetComponent<HealthComponent>();
		if(target != null)
		{
			MoveHandler enemyHandler = target.GetComponent<MoveHandler>();
			moveHandler.HitEnemy(enemyHandler);
			int direction = 1;
			if (knockBack != Vector2.zero)
			{
				direction = (int)Mathf.Sign(target.transform.position.x - entityController.transform.position.x);
			}
			target.Damage(damage, knockBack*direction);
		}
		/*EntityController target = collision.transform.root.GetComponent<EntityController>();
		if (target != null)
		{
			if (!entitiesHit.Contains(target.EntityID))
			{
				entityController.HitEnemy(target);
				int direction = 1;
				if (knockBack != Vector2.zero)
				{
					direction = (int)Mathf.Sign(target.transform.position.x - entityController.transform.position.x);
				}
				target.Damage(damage,knockBack*direction);
				entitiesHit.Add(target.EntityID);
			}
		}*/
	}
	// Update is called once per frame

	public void ResetHitBox()
	{
		entitiesHit.Clear();
	}

	public void EnableHitBox(int damage, Vector2 knockBack)
	{
		ResetHitBox();
		this.damage = damage;
		this.knockBack = knockBack;
		damageCollider.enabled = true;
	}

	public void DisableHitBox()
	{
		damageCollider.enabled = false;
	}
}
