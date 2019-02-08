using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer {
	private static int bufferLength;
	private InputItem[] bufferList;
	private string button;

	public InputBuffer(string but)
	{
		bufferList = new InputItem[bufferLength];
		button = but;
	}

	public void Update()
	{
		for(int i = 1; i < bufferLength; i++)
		{
			bufferList[i] = bufferList[i - 1];
		}
		bufferList[0].Input(Input.GetButton(button));
	}

	public bool CanUse()
	{
		for(int i = 1; i < bufferLength; i++)
		{
			if (bufferList[i].Hold == 0)
			{
				return !bufferList[i].Used;
			}
		}
		return false;
	}

	public void Execute()
	{
		for (int i = 1; i < bufferLength; i++)
		{
			if (bufferList[i].Hold == 0)
			{
				bufferList[i].Used = true;
			}
		}
	}
}
