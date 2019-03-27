using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagHandler : MonoBehaviour {
	private FlagData flagData;
	public FlagData Flags
	{
		get
		{
			return flagData;
		}
		set
		{
			flagData = value;
		}
	}
	public CommonFlags CommonFlags
	{
		get
		{
			return flagData.commonFlags;
		}
		set
		{
			flagData.commonFlags = value;
		}
	}
	public ValueFlags ValueFlags
	{
		get
		{
			return flagData.valueFlags;
		}
		set
		{
			flagData.valueFlags = value;
		}
	}
	// Use this for initialization
	void Start () {
		if(flagData.commonFlags == CommonFlags.None && flagData.valueFlags == ValueFlags.None)
		{
			flagData = new FlagData();
		}
	}

	public void TurnCommonFlagsOff(CommonFlags flags)
	{
		flagData.commonFlags &= ~flags;
	}

	public void TurnCommonFlagsOn(CommonFlags flags)
	{
		flagData.commonFlags |= flags;
	}

	public bool CheckCommonFlag(CommonFlags flag)
	{
		return ((flagData.commonFlags & flag) != CommonFlags.None);
	}
}
