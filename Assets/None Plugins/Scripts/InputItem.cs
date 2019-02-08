using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputItem {
	//largely based off of this tutorial: https://www.youtube.com/watch?v=3ZDHhr50fIY
	private int hold;

	public int Hold
	{
		get
		{
			return hold;
		}
	}

	private bool used;

	public bool Used
	{
		get
		{
			return used;
		}
		set
		{
			used = value;
		}
	}
	
	public void InputDown()
	{
		if(hold < 0)
		{
			hold = 1;
		}
		else
		{
			hold += 1;
		}
	}

	public void InputUp()
	{
		if(hold > 0)
		{
			hold = -1;
			used = false;
		}
		else
		{
			hold = 0;
		}
	}

	public void Input(bool input)
	{
		if (input)
		{
			InputDown();
		}
		else
		{
			InputUp();
		}
	}
}
