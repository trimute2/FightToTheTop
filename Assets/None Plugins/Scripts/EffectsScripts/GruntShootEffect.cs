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

		private int layerMask;

		private void Start()
		{
			layerMask = LayerMask.GetMask("Default");
			//layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);//LayerMask.GetMask("EnemyCollision");
			//LayerMask Test = LayerMask.GetMask(new string[] { "Default", "EntityCollision"});
			//layerMask = LayerMask.GetMask(new string[] { "Default", "EntityCollision" });
			//Debug.Break();
		}

		public void EffectHook(List<Vector3> points, List<Transform> transforms)
		{
			
		}

		public void EffectUpdate(Vector3[] points)
		{
			originPoint = points[0];
			aimPoint = points[1];
		}

		public void EffectUpdate(float f)
		{
			/*Vector2 direction = Vector2.zero;
			direction.x = Mathf.Cos(f);
			direction.y = Mathf.Sin(f);*/
			transform.rotation = Quaternion.Euler(0, 0, f);
			Vector3 dir = transform.right;
			//Vector3 dir = Quaternion.AngleAxis(f, Vector3.forward) * Vector3.right;
			Vector2 direction = new Vector2(dir.x, dir.y);
			RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 100, layerMask);
			//Physics2D.Raycast()
			Vector2 hitPoint;
			if (hit.collider != null)
			{
				hitPoint = Vector3.right * hit.distance;
				Debug.Break();
				//hitPoint = transform.InverseTransformPoint(hit.point);
			}
			else
			{
				direction *= 100;
				hitPoint = new Vector3(direction.x, direction.y, 0) + transform.position;
			}
			bottomBeam.SetPosition(1, hitPoint);
			midBeam.SetPosition(1, hitPoint);
			topBeam.SetPosition(1, hitPoint);
		}

		/*private void LateUpdate()
		{
			/*
			float angle = Vector3.Angle((aimPoint - originPoint).normalized, Vector3.right);
			Vector3 up = Quaternion.Euler(0, 0, angle) * Vector3.up;
			midBeam.SetPosition(0, originPoint);
			midBeam.SetPosition(1, aimPoint);
			topBeam.SetPosition(0, originPoint + up);
			topBeam.SetPosition(1, aimPoint);
			bottomBeam.SetPosition(0, originPoint-up);
			bottomBeam.SetPosition(1, aimPoint);*/
		//}

		public void EndEffect()
		{
			Destroy(gameObject);
		}
	}
}
