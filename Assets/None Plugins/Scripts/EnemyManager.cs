using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///  A struct representing a group of enemies in relation to the player
/// </summary>
[Serializable]
public struct EnemyGroup
{
	/// <summary>
	/// the distance from the player in which the range starts
	/// </summary>
	public float startingRange;
	/// <summary>
	/// the distance from the player in which the range ends
	/// </summary>
	public float endingRange;
	/// <summary>
	/// The current number of enemies to the left of the player within the range
	/// </summary>
	[HideInInspector]
	public int currentEnemiesLeft;
	/// <summary>
	/// The current number of enemies to the right of the player within the range
	/// </summary>
	[HideInInspector]
	public int currentEnemiesRight;
	/// <summary>
	/// The number of enemies trying to enter from the left
	/// </summary>
	[HideInInspector]
	public int enteringLeft;
	/// <summary>
	/// The number of enemies trying to enter from the right
	/// </summary>
	[HideInInspector]
	public int enteringRight;
	/// <summary>
	/// The number of enemies trying to exit from the left
	/// </summary>
	[HideInInspector]
	public int exitingLeft;
	/// <summary>
	/// The number of enemies trying to exit from the right
	/// </summary>
	[HideInInspector]
	public int exitingRight;
	[HideInInspector]
	public List<int> enemiesInRange;

	public bool InRange(float distance)
	{
		distance = Mathf.Abs(distance);
		return endingRange >= distance && distance > startingRange;
	}
}
public class EnemyManager : MonoBehaviour {
	public EnemyGroup[] ranges = new EnemyGroup[Enum.GetValues(typeof(AttackRange)).Length];


	private static EnemyManager _instance = null;
	private Dictionary<int, Enemy> enemies;
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
		enemies = new Dictionary<int, Enemy>();
	}

	public EnemyGroup DetailsOnRange(AttackRange Range)
	{
		return ranges[(int)Range];
	}

	public int GetTargetEntitiesInRange(AttackRange range)
	{
		int index = (int)range;
		return ranges[index].enemiesInRange.Count;

	}

	/// <summary>
	/// Get the number of entities trying to exist with in a range
	/// </summary>
	/// <param name="range"></param>
	/// <param name="direction"></param>
	/// <returns></returns>
	public int GetTargetEntitiesInRange(AttackRange range, float direction)
	{
		int index = (int)range;
		direction = Mathf.Sign(direction);
		int count = 0;
		count = ranges[index].enemiesInRange.Count;
		foreach(int id in ranges[index].enemiesInRange)
		{
			Enemy enemy = enemies[id];
			if (Mathf.Sign(enemy.XDistance) == direction)
			{
				EnemyCommands ec = enemy.CurrentCommand;
				if (ec != EnemyCommands.MoveIn && ec != EnemyCommands.MakeSpace)
				{
					count--;
				}
			}
			else
			{
				count--;
			}
		}
		if(direction == 1)
		{
			count += ranges[index].enteringLeft;
		}
		else
		{
			count += ranges[index].enteringRight;
		}
		return count;
	}

	public void UpdateCommand(AttackRange range, EnemyCommands previousCommand, EnemyCommands CurrentCommand, float direction)
	{
		if(previousCommand == CurrentCommand)
		{
			return;
		}
		int index = (int)range;
		direction = Mathf.Sign(direction);
		if(previousCommand == EnemyCommands.MakeSpace)
		{
			UpdateRangeEntering(index + 1, direction, -1);
		}else if(previousCommand == EnemyCommands.MoveIn)
		{
			UpdateRangeEntering(index - 1, direction, -1);
		}

		if (CurrentCommand == EnemyCommands.MakeSpace)
		{
			UpdateRangeEntering(index + 1, direction, 1);
		}
		else if (CurrentCommand == EnemyCommands.MoveIn)
		{
			UpdateRangeEntering(index - 1, direction, 1);
		}

	}

	private void UpdateRangeEntering(int index, float direction, int amount)
	{
		if(index < 0 || index >= ranges.Length)
		{
			return;
		}
		if(direction == 1)
		{
			ranges[index].enteringLeft += amount;
		}
		else
		{
			ranges[index].enteringRight += amount;
		}
	}

	private void UpdateDecisions()
	{
		
	}


	
	private void LateUpdate()
	{
		for(int i = 0; i < ranges.Length; i++)
		{
			ranges[i].enemiesInRange.Clear();
			ranges[i].enteringLeft = 0;
			ranges[i].enteringRight = 0;
		}
		foreach (KeyValuePair<int, Enemy> KVP in enemies)
		{
			Enemy e = KVP.Value;
			for(int i = 0; i < ranges.Length; i++)
			{
				if (ranges[i].InRange(e.XDistance))
				{
					ranges[i].enemiesInRange.Add(e.EntityID);
					e.CurrentRange = (AttackRange)i;
					float direction = Mathf.Sign(e.XDistance);
					if (e.CurrentCommand == EnemyCommands.MakeSpace)
					{
						UpdateRangeEntering(i + 1, direction, 1);
					}
					else if (e.CurrentCommand == EnemyCommands.MoveIn)
					{
						UpdateRangeEntering(i - 1, direction, 1);
					}
				}
			}
		}
	}

	/* An old method for commanding enemies i decided to scrap
	//At the end of the frame figure out enemies movements for next frame
	private void LateUpdate()
	{
		//TODO: clear the ranges of enemies before going of enemies

		//the list of enemies to send commands to
		List<Enemy> currentEnemies = new List<Enemy>();

		//iterate over each enemy
		foreach(KeyValuePair<int,Enemy> KVP in enemies)
		{
			Enemy e = KVP.Value;

			//TODO: update its position in the ranges
			
			//check if the enemy needs the manager to update its command if so add it to the list of enemies to send command to 
			if (e.ManagerDecision)
			{
				//set the enemies command to None and add it to the list
				e.CurrentCommand = EnemyCommands.None;
				currentEnemies.Add(e);
			}
		}
		//organize if more than one enemy
		if (currentEnemies.Count > 1)
		{
			//organize by agro
			currentEnemies.Sort(delegate (Enemy a, Enemy b)
			{
				return (a.Agro.CompareTo(b.Agro));
			});
		}

		foreach(Enemy e in currentEnemies)
		{
			//get the enemies prefered location
			AttackRange preference
			//check if it can move to that location
		}
	}*/



}
