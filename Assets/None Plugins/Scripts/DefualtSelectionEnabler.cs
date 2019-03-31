using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefaultMenuSelectedMenu))]
public class DefualtSelectionEnabler : MonoBehaviour
{
	void OnEnable()
	{
		DefaultMenuSelectedMenu inputModule = GetComponent<DefaultMenuSelectedMenu>();
		if (inputModule != null)
			inputModule.EnableAllActions();
	}

	void OnDisable()
	{
		DefaultMenuSelectedMenu inputModule = GetComponent<DefaultMenuSelectedMenu>();
		if (inputModule != null)
			inputModule.DisableAllActions();
	}
}
