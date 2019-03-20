using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualEffects
{
	[RequireComponent(typeof(Animation))]
	public class GruntShootEffect : MonoBehaviour, IVisualEffect
	{
		public LineRenderer topBeam;
		public LineRenderer midBeam;
		public LineRenderer bottomBeam;

		private Vector3 aimPoint;

		private Vector3 originPoint;

		public void EffectHook(List<Vector3> points, List<Transform> transforms)
		{
			
		}

		public void EffectUpdate(Vector3[] points)
		{
			originPoint = points[0];
			aimPoint = points[1];
		}

		private void LateUpdate()
		{
			float angle = Vector3.Angle((aimPoint - originPoint).normalized, Vector3.right);
			Vector3 up = Quaternion.Euler(0, 0, angle) * Vector3.up;
			midBeam.SetPosition(0, originPoint);
			midBeam.SetPosition(1, aimPoint);
			topBeam.SetPosition(0, originPoint + up);
			topBeam.SetPosition(1, aimPoint);
			bottomBeam.SetPosition(0, originPoint-up);
			bottomBeam.SetPosition(1, aimPoint);
		}

		public void EndEffect()
		{
			Destroy(gameObject);
		}
	}
}
