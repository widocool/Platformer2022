namespace GSGD2.Gameplay
{
	using GSGD2.Utilities;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Receive a force from any <see cref="Bumper"/> calling OnTriggerEnter().
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class BumperReceiver : MonoBehaviour
	{
		[SerializeField]
		private Timer _durationThresholdBeforeReceiveAnotherBump = null;

		private Rigidbody _rigidbody = null;
		private IBumperReceiverListener[] _bumperReceiverListeners = null;

		private Bumper _currentBumperInRange = null;

		// maybe check the bumper to prevent disabling by another ? Or not
		public void RemoveBumperFromRange()
		{
			_currentBumperInRange = null;
		}

		public void ApplyBump(Bumper fromBumper)
		{
			if (enabled == false) return;

			if (_durationThresholdBeforeReceiveAnotherBump.IsRunning == true)
			{
				_currentBumperInRange = fromBumper;
			}
			else
			{
				DoApplyBump(fromBumper);
			}
		}

		private void DoApplyBump(Bumper fromBumper)
		{
			_durationThresholdBeforeReceiveAnotherBump.Start();
			CallEventOnListeners();
			transform.position = fromBumper.Origin.position;
			_rigidbody.AddForce(fromBumper.Origin.up * fromBumper.BumpForce, ForceMode.Impulse);
		}

		private void Update()
		{
			_durationThresholdBeforeReceiveAnotherBump.Update();
		}

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_bumperReceiverListeners = GetComponents<IBumperReceiverListener>();
			// Ensure we start with the timer at true
			_durationThresholdBeforeReceiveAnotherBump.ForceFinishState();
		}

		private void OnEnable()
		{
			_durationThresholdBeforeReceiveAnotherBump.StateChanged -= DurationThresholdBeforeReceiveAnotherBumpOnStateChanged;
			_durationThresholdBeforeReceiveAnotherBump.StateChanged += DurationThresholdBeforeReceiveAnotherBumpOnStateChanged;
		}

		private void OnDisable()
		{
			_durationThresholdBeforeReceiveAnotherBump.StateChanged -= DurationThresholdBeforeReceiveAnotherBumpOnStateChanged;
		}

		/// <summary>
		/// Callback of timer, ensure that, if we still in the range of bumper when the end of timer occur, we get bumped and do not wait another trigger enter.
		/// </summary>
		/// <param name="timer"></param>
		/// <param name="state"></param>
		private void DurationThresholdBeforeReceiveAnotherBumpOnStateChanged(Timer timer, Timer.State state)
		{
			if (state == Timer.State.Finished && _currentBumperInRange != null)
			{
				DoApplyBump(_currentBumperInRange);
				_currentBumperInRange = null;
			}
		}

		private void CallEventOnListeners()
		{
			if (_bumperReceiverListeners.Length == 0) return;
			for (int i = 0; i < _bumperReceiverListeners.Length; i++)
			{
				var listener = _bumperReceiverListeners[i];
				listener.OnBump();
			}
		}
	}
}