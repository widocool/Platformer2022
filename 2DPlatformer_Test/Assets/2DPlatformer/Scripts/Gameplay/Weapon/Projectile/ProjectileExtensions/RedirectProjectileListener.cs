namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Component class can redirect <see cref="Projectile"/>. It must be added at the parent GameObject of a trigger.
	/// </summary>
	public class RedirectProjectileListener : MonoBehaviour
	{
		private enum Axis
		{
			X = 0,
			Y,
			Z,
			MinusX,
			MinusY,
			MinusZ
		}

		[SerializeField]
		private Transform _origin = null; // rename to _redirectionOffset 

		[SerializeField]
		private Axis _axis = 0;

		public Transform Origin => _origin;

		public Vector3 GetDirection()
		{
			switch (_axis)
			{
				case Axis.X: return _origin.right;
				case Axis.Y: return _origin.up;
				case Axis.Z: return _origin.forward;
				case Axis.MinusX: return -_origin.right;
				case Axis.MinusY: return -_origin.up;
				case Axis.MinusZ: return -_origin.forward;
				default: return Vector3.zero;
			}
		}
	}
}