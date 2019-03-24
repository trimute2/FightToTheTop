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

		private void Start()
		{
		}

		public void EffectHook(List<Vector3> points, List<Transform> transforms)
		{
			
		}

		public void EffectUpdate(Vector3[] points)
		{

		}

		public void EffectUpdate(float f)
		{

		}

		public void EffectUpdate()
		{
			int layerMask = LayerMask.GetMask("Default");
			Vector3 dir = transform.right * transform.root.localScale.x;
			Vector2 direction = new Vector2(dir.x, dir.y);
			RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 100, layerMask);
			Vector2 hitPoint;
			if (hit.collider != null)
			{
				hitPoint = Vector3.right * hit.distance;
			}
			else
			{
				direction *= 100;
				hitPoint = new Vector3(direction.x, 0, 0) + transform.position;
			}
			bottomBeam.SetPosition(1, hitPoint);
			midBeam.SetPosition(1, hitPoint);
			topBeam.SetPosition(1, hitPoint);
		}

		public void EndEffect()
		{
			Destroy(gameObject);
		}
	}
}
