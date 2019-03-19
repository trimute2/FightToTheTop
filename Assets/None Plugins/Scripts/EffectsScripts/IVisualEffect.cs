using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualEffects
{
	public interface IVisualEffect 
	{
		void EffectHook(List<Vector3> points, List<Transform> transforms);
	}
}
