using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitBoxScript : MonoBehaviour {

	private MoveHandler moveHandler;
	private Collider2D damageCollider;
	private int damage;
	private Vector2 knockBack;
	private List<int> entitiesHit;
	private List<HealthComponent> hasHit;
	// Use this for initialization
	void Start () {
		moveHandler = transform.root.GetComponent<MoveHandler>();
		damageCollider = this.GetComponent<Collider2D>();
		damageCollider.enabled = false;
		entitiesHit = new List<int>();
		hasHit = new List<HealthComponent>();
		damage = 0;
		knockBack = Vector2.zero;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		HealthComponent target = collision.transform.root.GetComponent<HealthComponent>();
		if(target != null && !hasHit.Contains(target))
		{
			//todo: re write this so that the oponent does not tnee a move handler and so that its all inside the damage check
			int direction = 1;
			if (knockBack != Vector2.zero)
			{
				direction = (int)Mathf.Sign(target.transform.position.x - moveHandler.transform.position.x);
			}
			Vector2 kb = knockBack;
			kb.x *= direction;
			float hitStun = moveHandler.GetHitStun();

			if(target.Damage(damage, kb, hitStun))
			{
				hasHit.Add(target);
				MoveHandler enemyHandler = target.GetComponent<MoveHandler>();
				moveHandler.HitEnemy(enemyHandler);
				if(enemyHandler != null)
				{
					enemyHandler.HitDirection = direction;
				}
			}
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
		hasHit.Clear();
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
