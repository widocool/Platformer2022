namespace GSGD2.Editor
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Quick class used to instantiate temporary object to see trajectory.
	/// </summary>
	public class PrimitiveDebug : MonoBehaviour
	{
		[SerializeField]
		private float _size = 0.25f;

		[SerializeField]
		private float _lifeTime = 10f;

		[SerializeField]
		private int _frequency = 2;

		private void Update()
		{
			if (Time.frameCount % _frequency == 0)
			{
				InstantiateGizmos(transform.position, _lifeTime, _size);
			}
		}

		public static void InstantiateGizmos(Vector3 position, float lifeTime, float size)
		{
			var instance = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			instance.transform.position = position;
			instance.transform.localScale = Vector3.one * size;
			Destroy(instance.GetComponent<Collider>());
			Destroy(instance, lifeTime);
		}
	}
}