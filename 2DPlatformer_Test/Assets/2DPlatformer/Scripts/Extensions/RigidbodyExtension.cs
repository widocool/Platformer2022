namespace GSGD2.Extensions
{
	using UnityEngine;

#if UNITY_EDITOR
#endif //UNITY_EDITOR

	/// <summary>
	/// Extension class for Rigidbodies
	/// </summary>
	public static class RigidbodyExtension
	{
		/// <summary>
		/// Use to setup a rigidbody that needs to receive only trigger events
		/// </summary>
		/// <param name="rb"></param>
		public static void SetUpPassiveRigidbody(this Rigidbody rb, CollisionDetectionMode collisionDetectionMode = CollisionDetectionMode.Discrete)
		{
			rb.useGravity = false;
			rb.isKinematic = true;
			rb.collisionDetectionMode = collisionDetectionMode;
		}
	}
}