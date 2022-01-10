namespace GSGD2.Gameplay
{
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Component that can be put on an object that needs to be moved by a object being translated (such as a <see cref="MovingPlatform"/>). Disabling the component disable its ability to be moved by <see cref="MovingPlatformStickToGround"/> .
	/// <see cref="MovingPlatformStickToGround"/> will handle its position if it enter in its trigger.
	/// </summary>
	public class MovingPlatformStickToGroundReceiver : MonoBehaviour
	{
		public enum PlatformMoveMode
		{
			Parenting,
			DeltaPosition
		}

		[SerializeField]
		private PlatformMoveMode _moveMode = 0;

		private bool _isOnPlatform = false;
		private bool _isPlatformMoving = false;
		protected MovingPlatformStickToGround movingPlatformStickToGround = null;

		public PlatformMoveMode MoveMode => _moveMode;

		public bool IsOnPlatform => _isOnPlatform;
		public bool IsPlatformMoving => _isPlatformMoving;
		public MovingPlatformStickToGround MovingPlatformStickToGround => movingPlatformStickToGround;

		public bool SetOnPlatform(MovingPlatformStickToGround movingPlatformStickToGround, bool isOnPlatform, bool force = false)
		{
			bool result = force == true ? true : CanSetOnPlatform();
			if (result == true)
			{
				this.movingPlatformStickToGround = movingPlatformStickToGround;
				_isOnPlatform = isOnPlatform;
				OnSetOnPlatform(isOnPlatform);
				return true;
			}
			return false;
		}

		public void ClearFromPlatform()
		{
			movingPlatformStickToGround = null;
			_isOnPlatform = false;
		}

		public void SetPlatformMoving(bool isPlatformMoving)
		{
			_isPlatformMoving = isPlatformMoving;
			OnSetIsPlatformMoving(isPlatformMoving);
		}

		protected virtual bool CanSetOnPlatform() { return true; }
		protected virtual void OnSetOnPlatform(bool isOnPlatform) { }
		protected virtual void OnSetIsPlatformMoving(bool isPlatformMoving) { }

		protected virtual void Awake() { }
		protected virtual void OnEnable() { }
		protected virtual void OnDisable() { }
	}
}