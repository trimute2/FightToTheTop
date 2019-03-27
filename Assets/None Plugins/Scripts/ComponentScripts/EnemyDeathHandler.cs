using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class EnemyDeathHandler : MonoBehaviour
{
	private HealthComponent healthComponent;
	private EntityControllerComp entityController;
	private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
		healthComponent = GetComponent<HealthComponent>();
		entityController = GetComponent<EntityControllerComp>();
		isDead = false;
		healthComponent.OnDeath += AtZeroHP;
    }

	private void AtZeroHP()
	{
		isDead = true;
	}

	// Update is called once per frame
	void Update()
    {
		if (isDead)
		{
			//todo: ensure that the death animation has finished
			if(entityController != null)
			{
				if (entityController.Grounded)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				Destroy(gameObject);
			}
		}
    }
}
