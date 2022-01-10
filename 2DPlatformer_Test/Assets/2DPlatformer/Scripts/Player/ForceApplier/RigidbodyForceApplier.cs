namespace GSGD2.Player
{
	using UnityEngine;

	/// <summary>
	/// Base class to handle force applied on a physics-based GameObject.
	/// It do not apply a force on itself, but handle enabling and maximum forces without being reset (<see cref="Jump"/> or <see cref="Dash"/>)
	/// </summary>
	[System.Serializable]
	public abstract class RigidbodyForceApplier
	{
		[SerializeField]
		public bool enabled = true;

		[SerializeField]
		private int _maximumAllowedForcesWhileInAir = 1;

		[SerializeField]
		private bool resetVelocityWhenApplyingForceWhileInAir = true;

		protected int currentAllowedForcesWhileInAir = 0;

		protected int MaximumAllowedForcesWhileInAir => _maximumAllowedForcesWhileInAir;
		protected bool ResetVelocityWhenApplyingForceWhileInAir => resetVelocityWhenApplyingForceWhileInAir;

		// Editor only
		public virtual void Validate()
		{
			_maximumAllowedForcesWhileInAir = Mathf.RoundToInt(Mathf.Clamp(_maximumAllowedForcesWhileInAir, 0, float.MaxValue));
		}

		public void AddMaximumAllowedForcesWhileInAir(int toAdd)
		{
			_maximumAllowedForcesWhileInAir = Mathf.RoundToInt(Mathf.Clamp(_maximumAllowedForcesWhileInAir + toAdd, 0f, float.MaxValue));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="forcesCountToReset">if 0, reset to the max allowed. otherwise set the number directly (clamped to maximum)</param>
		public void ResetCurrentForceCount(int forcesCountToReset = 0)
		{
			if (forcesCountToReset < 0)
			{
				forcesCountToReset = 0;
			}
			currentAllowedForcesWhileInAir =  Mathf.RoundToInt(Mathf.Clamp(forcesCountToReset == 0 ? 0 : _maximumAllowedForcesWhileInAir - forcesCountToReset, 0f, _maximumAllowedForcesWhileInAir));
		}

		public virtual bool CanApplyForce()
		{
			return enabled && currentAllowedForcesWhileInAir < _maximumAllowedForcesWhileInAir;
		}

		public virtual bool TryApplyForce(Rigidbody rigidbody)
		{
			if (CanApplyForce() == true)
			{
				DoApplyForce(rigidbody);
				currentAllowedForcesWhileInAir++;
				return true;
			}
			return false;
		}

		protected abstract void DoApplyForce(Rigidbody rigidbody);
	}
}