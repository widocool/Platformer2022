namespace GSGD2.Utilities
{
	using UnityEngine;

	/// <summary>
	/// Class providing conveniant ways to tell if a movement is Left, Right, Up, etc...
	/// It has unfinished utilities to handle walls too.
	/// </summary>
	[System.Serializable]
	public class MovementDirection
	{
		public IVelocitySource velocitySource = null;

		[Header("ReadOnly")]
		[SerializeField] private bool _up = false;
		[SerializeField] private bool _right = false;
		[SerializeField] private bool _down = false;
		[SerializeField] private bool _left = false;
		[SerializeField] private bool _upRight = false;
		[SerializeField] private bool _downRight = false;
		[SerializeField] private bool _downLeft = false;
		[SerializeField] private bool _upLeft = false;

		// Direction only for velocity
		public bool Up => _up = Vector3.Dot(Vector3.up, velocitySource.Velocity) > 0;
		public bool Right => _right = Vector3.Dot(Vector3.forward, velocitySource.Velocity) > 0;
		public bool Down => _down = Vector3.Dot(Vector3.up, velocitySource.Velocity) < 0;
		public bool Left => _left = Vector3.Dot(Vector3.forward, velocitySource.Velocity) < 0;
		public bool UpRight => _upRight = Up && Right;
		public bool DownRight => _downRight = Down && Right;
		public bool DownLeft => _downLeft = Down && Left;
		public bool UpLeft => _upLeft = Up && Left;

		// Direction only for wall
		public static bool IsWallNormalRight(Vector3 velocity)
		{
			return Vector3.Dot(Vector3.forward, velocity) > 0;
		}

		public static bool IsWallNormalLeft(Vector3 velocity)
		{
			return Vector3.Dot(Vector3.forward, velocity) < 0;
		}

		public void ForceUpdate()
		{
			bool up = Up;
			bool right = Right;
			bool down = Down;
			bool left = Left;
			bool upRight = UpRight;
			bool downRight = DownRight;
			bool downLeft = DownLeft;
			bool upLeft = UpLeft;
		}
	}
}