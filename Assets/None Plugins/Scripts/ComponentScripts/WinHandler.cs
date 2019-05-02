using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
public class WinHandler : MonoBehaviour
{

	public List<HealthComponent> enemies;

	//number of enemies
	private int noe;
	private UIManager manager;

    // Start is called before the first frame update
    void Start()
    {
		manager = GetComponent<UIManager>();
		noe = enemies.Count;
		foreach(HealthComponent hc in enemies)
		{
			hc.OnDeath += EnemyDeathCall;
		}
    }

	private void EnemyDeathCall()
	{
		noe--;
		if(noe == 0)
		{
			manager.OnWin();
		}
	}
}
