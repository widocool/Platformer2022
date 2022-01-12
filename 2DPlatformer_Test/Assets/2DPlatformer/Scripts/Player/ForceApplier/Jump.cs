namespace GSGD2.Player
{
	using GSGD2.Utilities;
	using UnityEngine;

	/// <summary>
	/// Add a force on the y component
	/// <see cref="CubeController.State.StartJump"/>, <see cref="CubeController.State.Jumping"/> <see cref="CubeController.State.EndJump"/> occurrences for more details.
	/// </summary>
	[System.Serializable]
	public class Jump : PlayerRigidbodyForceApplier
	{
		[SerializeField]
		private float _gravityScale = 5f;

		[SerializeField]
		private float _jumpHeight = 3f;

		[SerializeField]
		private float _jumpHeightBonusWhenFalling = 1f;

		[SerializeField]
		private float _jumpHeightBonusSpeedThreshold = 10f;

		protected override void DoApplyForce(Rigidbody rigidbody)
		{
			// We zeroing out the velocity in bonus jump, otherwise the character falling velocity can be too high
			if (currentAllowedForces > 0)
			{
				rigidbody.velocity = Vector3.zero;
			}

			float height = displacementEstimationUpdater.MovementDirection.Down &&  displacementEstimationUpdater.AverageSpeed > _jumpHeightBonusSpeedThreshold ? _jumpHeight + _jumpHeightBonusWhenFalling : _jumpHeight;
			var vel = rigidbody.velocity;
			vel.y = 0f;
			rigidbody.velocity = vel;

			// Can't divide a square root by a negative number, so we remember the direction then inverse it if so
			bool inverseY = false;
			if (height < 0)
			{
				inverseY = true;
				height *= -1;
			}

			Vector3 force = new Vector3
			(
				0f, 
				Mathf.Sqrt(2 * (height * _gravityScale) * Mathf.Abs(Physics.gravity.y)), 
				0f//_reflectRigidbodyVelocity == true ? -vel.z : 0f
			);

			if (inverseY == true)
			{
				force.y *= -1;
			}

			rigidbody.AddForce(force, ForceMode.Impulse);
		}

		public override void Validate()
		{
			base.Validate();
			_jumpHeightBonusWhenFalling = Mathf.Clamp(_jumpHeightBonusWhenFalling, 0, float.MaxValue); ;
		}
	}
}