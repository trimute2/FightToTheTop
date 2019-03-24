using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualEffects
{
	public interface IVisualEffect 
	{
		void EffectHook(List<Vector3> points, List<Transform> transforms);
		void EffectUpdate(Vector3[] points);
		void EffectUpdate(float f);
		void EffectUpdate();
		void EndEffect();
	}
}
