namespace GSGD2.Utilities
{
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;

	/// <summary>
	/// Component that calculate delta position, velocity, speed and a utility class <see cref="MovementDirection"/> to facilitate direction stuffs.
	/// It is necessary even for rigidbody since the velocity given by the rigidbody is before the resolution of collisions.
	/// </summary>
	public class DisplacementEstimationUpdater : MonoBehaviour, IVelocitySource
	{
		[SerializeField]
		private int _capacity = 20;

		[Header("ReadOnly")]
		[SerializeField]
		private float _averageSpeed = 0;

		[SerializeField]
		private Vector3 _velocity;

		[SerializeField]
		private bool _showDebug = false;

		[SerializeField]
		private MovementDirection _movementDirection = null;

		private Queue<float> _previousSpeedQueue;
		private Vector3 _lastPosition;
		private Vector3 _deltaPosition;

		public Vector3 DeltaPosition => _deltaPosition;
		public Vector3 Velocity => _velocity;
		public float AverageSpeed => _averageSpeed;
		public MovementDirection MovementDirection => _movementDirection;

		private void Awake()
		{
			_previousSpeedQueue = new Queue<float>(_capacity);
			_movementDirection.velocitySource = this;
		}

		private void Update()
		{
			_movementDirection.ForceUpdate();
		}

		private void FixedUpdate()
		{
			Vector3 position = transform.position;
			Vector3 deltaPosition = (position - _lastPosition);
			Vector3 velocity = deltaPosition / Time.deltaTime;
			float currentSpeed = velocity.magnitude;
			bool canEnqueue = Mathf.Approximately(currentSpeed, 0f) == false;
			if (canEnqueue)
			{
				_previousSpeedQueue.Enqueue(currentSpeed);
			}
			int count = _previousSpeedQueue.Count;
			if (_previousSpeedQueue.Count > 0 && (count > _capacity || canEnqueue == false))
			{
				_previousSpeedQueue.Dequeue();
			}
			float averageSpeed = 0f;
			foreach (float item in _previousSpeedQueue)
			{
				averageSpeed += item;
			}
			averageSpeed = count != 0 ? averageSpeed / count : 0f;
			if (_showDebug == true)
			{
				Debug.LogFormat("AverageSpeed : {0} | Velocity : {1}", averageSpeed, velocity);
			}
			_lastPosition = position;
			_deltaPosition = deltaPosition;
			_velocity = velocity;
			_averageSpeed = averageSpeed;
		}
	}
}