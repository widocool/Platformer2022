namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Simple component that can be used to rotate an object on itself.
	/// It can be setup to reset rotation at OnEnable or OnDisable
	/// </summary>
	public class Rotator : MonoBehaviour
	{
		[SerializeField]
		private Vector3 _rotationForces = Vector3.up;

		[SerializeField]
		private Space _space = 0;

		[SerializeField]
		private bool _resetWorldRotationAtOnEnable = true;

		[SerializeField]
		private bool _resetWorldRotationAtOnDisable = true;

		private Quaternion _worldCachedRotationAtStart = Quaternion.identity;

		public void Play()
		{
			enabled = true;
		}

		public void Stop()
		{
			enabled = false;
		}

		private void Awake()
		{
			_worldCachedRotationAtStart = transform.rotation;
		}

		private void OnEnable()
		{
			if (_resetWorldRotationAtOnEnable == true)
			{
				transform.rotation = _worldCachedRotationAtStart;
			}
		}

		private void OnDisable()
		{
			if (_resetWorldRotationAtOnDisable == true)
			{
				transform.rotation = _worldCachedRotationAtStart;
			}
		}

		private void Update()
		{
			transform.Rotate(_rotationForces * Time.deltaTime, _space);
		}
	}
}