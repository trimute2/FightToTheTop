using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer {
	public static int bufferLength = 10;
	private InputItem[] bufferList;
	private string button;

	public string Button
	{
		get
		{
			return button;
		}
	}

	public InputBuffer(string but)
	{
		bufferList = new InputItem[bufferLength];
		for(int i = 0; i < bufferList.Length; i++)
		{
			bufferList[i] = new InputItem();
		}
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
			if (bufferList[i].Hold == 1)
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
			if (bufferList[i].Hold == 1)
			{
				bufferList[i].Used = true;
			}
		}
	}

	public int Hold()
	{
		return bufferList[0].Hold;
	}
}
