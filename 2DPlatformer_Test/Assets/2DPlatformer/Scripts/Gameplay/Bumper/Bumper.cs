namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Extensions;

#if UNITY_EDITOR
	using UnityEditor;
#endif //UNITY_EDITOR

	/// <summary>
	/// Add a force to any <see cref="BumperReceiver"/> calling OnTriggerEnter().
	/// </summary>
	[SelectionBase]
	[RequireComponent(typeof(Rigidbody))]
	public class Bumper : MonoBehaviour
	{
		[SerializeField]
		private float _bumpForce = 3f;

		[SerializeField]
		private Transform _origin = null;

		public Transform Origin => _origin;
		public float BumpForce => _bumpForce;

		private List<BumperReceiver> _bumperReceiversInRange = new List<BumperReceiver>();

		private void Awake()
		{
			GetComponent<Rigidbody>().SetUpPassiveRigidbody();
		}

		private void OnTriggerEnter(Collider other)
		{
			BumperReceiver concreteOther = other.GetComponentInParent<BumperReceiver>();
			if (concreteOther != null && _bumperReceiversInRange.Contains(concreteOther) == false)
			{
				_bumperReceiversInRange.Add(concreteOther);
				concreteOther.ApplyBump(this);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			BumperReceiver concreteOther = other.GetComponentInParent<BumperReceiver>();
			if (concreteOther != null)
			{
				concreteOther.RemoveBumperFromRange();
				_bumperReceiversInRange.Remove(concreteOther);
			}
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Handles.DrawLine(_origin.position, _origin.position + _origin.up * _bumpForce * 0.2f);
		}
#endif //UNITY_EDITOR
	}
}