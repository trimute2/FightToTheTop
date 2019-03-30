using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class EnemyDeathHandler : MonoBehaviour
{
	private HealthComponent healthComponent;
	private EntityControllerComp entityController;
	private Animator anim;
	private FlagHandler flagHandler;
	private bool isDead;
	private bool inDeathAnimation;
    // Start is called before the first frame update
    void Start()
    {
		healthComponent = GetComponent<HealthComponent>();
		entityController = GetComponent<EntityControllerComp>();
		flagHandler = GetComponent<FlagHandler>();
		anim = GetComponent<Animator>();
		isDead = false;
		inDeathAnimation = false;
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
					anim.Play("Death");
					flagHandler.CommonFlags = CommonFlags.None;
				}
			}
			else
			{
				Destroy(gameObject);
			}
		}
    }

	public void Kill()
	{
		Destroy(gameObject);
	}
}
