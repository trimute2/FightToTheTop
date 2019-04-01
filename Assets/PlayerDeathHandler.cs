using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
	public void Kill()
	{
		Time.timeScale = 0;
	}
}
