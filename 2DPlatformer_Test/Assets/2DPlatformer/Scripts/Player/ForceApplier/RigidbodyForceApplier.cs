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
		private int _maximumAllowedForces = 1;

		protected int currentAllowedForces = 0;

		public int MaximumAllowedForcesWhileInAir => _maximumAllowedForces;

		// Editor only
		public virtual void Validate()
		{
			_maximumAllowedForces = Mathf.RoundToInt(Mathf.Clamp(_maximumAllowedForces, 0, float.MaxValue));
		}

		public void AddMaximumAllowedForcesWhileInAir(int toAdd)
		{
			_maximumAllowedForces = Mathf.RoundToInt(Mathf.Clamp(_maximumAllowedForces + toAdd, 0f, float.MaxValue));
		}

		public void ResetCurrentForceCount(int forcesCountToReset = 0)
		{
			currentAllowedForces = Mathf.RoundToInt(Mathf.Clamp(forcesCountToReset, 0f, _maximumAllowedForces));
		}

		public virtual bool CanApplyForce()
		{
			return enabled && currentAllowedForces < _maximumAllowedForces;
		}

		public virtual bool TryApplyForce(Rigidbody rigidbody)
		{
			if (CanApplyForce() == true)
			{
				DoApplyForce(rigidbody);
				currentAllowedForces++;
				return true;
			}
			return false;
		}

		protected abstract void DoApplyForce(Rigidbody rigidbody);
	}
}