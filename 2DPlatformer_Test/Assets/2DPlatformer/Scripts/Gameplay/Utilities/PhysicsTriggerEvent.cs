namespace GSGD2.Gameplay
{
	using UnityEngine;
	using UnityEngine.Events;

	/// <summary>
	/// Bind native Trigger event's to UnityEvent. It can be used with any type, but some classes are specialized to work with (such as <see cref="AInteractor"/>.
	/// If setup in a "passive" way (without rigidbody), it MUST be on the same GameObject as the trigger Collider. Multiple <see cref="PhysicsTriggerEvent"/> can be setup on multiple colliders, each giving which trigger is been used.
	/// If setup in a "active" way, it MUST be on the same GameObject as the Rigidbody, but will be triggered by every triger beneath it and will not be capable of determine which trigger has been touched.
	/// </summary>
	public class PhysicsTriggerEvent : MonoBehaviour
	{
		[System.Serializable]
		public class Trigger_UnityEvent : UnityEvent<PhysicsTriggerEvent, Collider> { }

		public Trigger_UnityEvent _onTriggerEnter = null;
		public Trigger_UnityEvent _onTriggerStay = null;
		public Trigger_UnityEvent _onTriggerExit = null;

		private void OnTriggerEnter(Collider other)
		{
			_onTriggerEnter.Invoke(this, other);
		}

		private void OnTriggerStay(Collider other)
		{
			_onTriggerStay.Invoke(this, other);
		}

		private void OnTriggerExit(Collider other)
		{
			_onTriggerExit.Invoke(this, other);
		}
	}
}