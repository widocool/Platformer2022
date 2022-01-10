namespace GSGD2.Player
{
	using UnityEngine;
	using GSGD2.Utilities;

	/// <summary>
	/// Add a force in the direction of <see cref="CubeController.LastMovementDirection"/>.
	/// <see cref="CubeController.State.Dashing"/> occurrences for more details.
	/// </summary>
	[System.Serializable]
	public class Dash : PlayerRigidbodyForceApplier
	{
		[SerializeField]
		private float _dashForce = 3f;

		[SerializeField]
		private Timer _timerUntilDashEnd = null;

		[SerializeField]
		private float _positionOffsetWhenEnteringDash = 0.5f;

		[SerializeField]
		private bool _debugMode = false;

		private float _lastMovementDirection = 0f;

		// Debug
		private Vector3 _lastPositionAtDashStart;
		private Vector3 _lastPositionAtDashEnd;
		private float _lastDistance = 0;
		private float _estimatedDistance = 0;

		public float LastMovementDirection => _lastMovementDirection;

		public void EndDash(Rigidbody rigidbody)
		{
			cubeController.SetColliderTrigger(false);
			rigidbody.velocity = Vector3.zero;
			StopTimer();

			//Debug.Break();

			if (_debugMode == true)
			{
				_estimatedDistance = _dashForce * _timerUntilDashEnd.Duration;
				_lastPositionAtDashEnd = cubeController.transform.position;
				_lastDistance = Vector3.Distance(_lastPositionAtDashStart, _lastPositionAtDashEnd);

				Debug.DrawLine(_lastPositionAtDashStart, _lastPositionAtDashStart + Vector3.up * 2, Color.yellow, 10);
				Debug.DrawLine(_lastPositionAtDashEnd, _lastPositionAtDashEnd + Vector3.up * 2, Color.yellow, 10);
				Debug.DrawLine(_lastPositionAtDashStart, _lastPositionAtDashEnd, Color.yellow, 10);
			}
		}

		public bool UpdateTimer()
		{
			return _timerUntilDashEnd.Update();
		}

		public void StopTimer() => _timerUntilDashEnd.ForceFinishState();

		protected override void DoApplyForce(Rigidbody rigidbody)
		{
			if (_debugMode == true)
			{
				_lastPositionAtDashStart = cubeController.transform.position;
			}
			if (_positionOffsetWhenEnteringDash > 0)
			{
				cubeController.transform.position += Vector3.up * _positionOffsetWhenEnteringDash;
			}

			Vector3 forward = Vector3.forward * cubeController.LastMovementDirection;
			_lastMovementDirection = forward.z >= 0 ? 1 : -1;
			cubeController.SetColliderTrigger(true);

			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce(forward * _dashForce, ForceMode.VelocityChange);

			_timerUntilDashEnd.Start();
		}

	}

}