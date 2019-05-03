using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoiderAECaller : MonoBehaviour
{
	private Avoider avoider;

	private void Start()
	{
		avoider = GetComponentInChildren<Avoider>();
	}

	public void SetAvoiderIgnorable(AnimationEvent animationEvent)
	{
		bool b = (animationEvent.intParameter == 1);
		avoider.Ignore = b;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		Avoider.AvoiderValidationCheck(gameObject);
	}
#endif
}
