using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualEffects
{
	[RequireComponent(typeof(Animation))]
	public class GruntShootEffect : MonoBehaviour, IVisualEffect
	{
		public LineRenderer midBeam;

		private Transform aimPoint;
		public Transform AimPoint
		{
			get
			{
				return aimPoint;
			}
			set
			{
				aimPoint = value;
			}
		}

		private Transform originPoint;
		public Transform OriginPoint
		{
			get
			{
				return originPoint;
			}
			set
			{
				originPoint = value;
			}
		}

		public void EffectHook(List<Vector3> points, List<Transform> transforms)
		{
			originPoint = transforms[0];
			aimPoint = transforms[1];
		}

		private void LateUpdate()
		{
			midBeam.SetPosition(0, originPoint.position);
			midBeam.SetPosition(1, aimPoint.position);
		}

		public void EndEffect()
		{
			Destroy(gameObject);
		}
	}
}
