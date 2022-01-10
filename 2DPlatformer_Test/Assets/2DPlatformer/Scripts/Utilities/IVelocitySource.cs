namespace GSGD2.Utilities
{
	using UnityEngine;

	/// <summary>
	/// Interface that can be added to a component to automatically fill <see cref="MovementDirection"/> properties like Up, Right, Down, Left, etc...
	/// This component must be added to <see cref="MovementDirection.velocitySource"/> first.
	/// </summary>
	public interface IVelocitySource
	{
		public Vector3 Velocity { get; }
	}
}