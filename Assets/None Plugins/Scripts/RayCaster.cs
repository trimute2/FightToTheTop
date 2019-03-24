using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
	private MoveHandler moveHandler;
	private LayerMask layerMask;
	private void Start()
	{
		moveHandler = transform.root.GetComponent<MoveHandler>();
		layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
	}

	public void RayCastAttack(int damage, Vector2 knockback)
	{
		Vector3 dir = transform.right * transform.root.localScale.x;
		Vector2 direction = new Vector2(dir.x, dir.y);
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 100, layerMask);
		if (hit.collider != null)
		{
			HealthComponent health = hit.collider.transform.root.GetComponent<HealthComponent>();
			if(health != null)
			{
				health.Damage(damage, knockback);
			}
		}
	}
}
