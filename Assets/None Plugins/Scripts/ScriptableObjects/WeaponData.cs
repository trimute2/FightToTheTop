using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Data/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
	[SerializeField]
	private string weaponName;
	public string Name
	{
		get
		{
			return weaponName;
		}
	}
	[SerializeField]
	private List<MoveLink> moves;
	public List<MoveLink> Moves
	{
		get
		{
			return moves;
		}
	}
}
