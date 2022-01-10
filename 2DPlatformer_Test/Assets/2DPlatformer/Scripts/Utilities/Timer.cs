namespace GSGD2.Utilities
{
	using UnityEngine;

	/// <summary>
	/// Conveniance class to quickly setup a timer.
	/// <see cref="Start"/> to start the timer, it must be <see cref="Update()"/>, and the end of the timer can be setup two ways : listening the event <see cref="StateChanged"/> or the return value of <see cref="Update()"/>.
	/// <see cref="Progress"/> is a 0-1 normalized value that can be used e.g. in AnimationCurve.
	/// </summary>
	[System.Serializable]
	public class Timer
	{
		public enum State
		{
			Stopped, // Update return false
			Running, // Update return false
			Finished // Update return true
		}

		[SerializeField]
		private float _duration = 0f;

		private float _timeElapsed = 0f;
		private State _currentState = 0;

		public float Duration => _duration;
		public float Progress => _timeElapsed / _duration;

		public State CurrentState => _currentState;

		public bool IsRunning => _currentState == State.Running;

		public delegate void TimerEvent(Timer timer, State state);
		public event TimerEvent StateChanged = null;

		public void ForceFinishState()
		{
			_currentState = State.Finished;
		}

		public void ResetTimeElapsed()
		{
			_timeElapsed = 0;
		}

		public void Start()
		{
			Start(_duration);
		}

		public void Start(float duration)
		{
			ResetTimeElapsed();
			_duration = duration;
			_currentState = State.Running;
			StateChanged?.Invoke(this, _currentState);
		}

		public bool Update()
		{
			switch (_currentState)
			{
				case State.Running:
				{
					_timeElapsed += Time.deltaTime;
					if (_timeElapsed > _duration)
					{
						_currentState = State.Finished;
						StateChanged?.Invoke(this, State.Finished);
						return true;
					}
				}
				break;
				case State.Finished:
				{
					return true;
				}
				case State.Stopped:
				default:
					break;
			}
			return false;
		}

	}
}