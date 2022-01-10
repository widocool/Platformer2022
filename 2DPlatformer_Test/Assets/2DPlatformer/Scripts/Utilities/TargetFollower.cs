namespace GSGD2.Utilities
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Simple utility component that permit to follow a target, nullifying its z axis. Obsolete since Cinemachine.
	/// </summary>
	public class TargetFollower : MonoBehaviour
	{
		[SerializeField]
		private float _speed = 1f;

		[SerializeField]
		private Transform _target = null;

		private void LateUpdate()
		{
			Vector3 nextPosition = transform.position;
			nextPosition.z = _target.transform.position.z;
			transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * _speed);
		}
	}
}