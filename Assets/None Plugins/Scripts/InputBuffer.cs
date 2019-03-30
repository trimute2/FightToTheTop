using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class InputBuffer {
	//public static int bufferLength = 23;
	public static int bufferLength = 15;
	private InputItem[] bufferList;
	private string button;
	private bool press = false;

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
		bufferList[0].Input(press);
		//bufferList[0].Input(Input.GetButton(button));
	}

	public bool CanUse()
	{
		for(int i = 0; i < bufferLength; i++)
		{
			if (bufferList[i].Hold == 1 && !bufferList[i].Used)
			{
				//return !bufferList[i].Used;
				return true;
			}
		}
		return false;
	}

	public void Execute()
	{
		for (int i = 0; i < bufferLength; i++)
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

	public void ReadInput(InputAction.CallbackContext ctx)
	{
		press = ctx.ReadValue<float>() > 0;
	}
	
}
