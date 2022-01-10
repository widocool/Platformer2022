namespace GSGD2.Player
{
	using GSGD2.Utilities;
	using UnityEngine;

	/// <summary>
	/// Intermediate abstract class that handle <see cref="RigidbodyForceApplier"/> on a <see cref="CubeController"/>.
	/// </summary>
	public abstract class PlayerRigidbodyForceApplier : RigidbodyForceApplier
	{
		[HideInInspector] public CubeController cubeController = null;
		[HideInInspector] public DisplacementEstimationUpdater displacementEstimationUpdater = null;
	}
}