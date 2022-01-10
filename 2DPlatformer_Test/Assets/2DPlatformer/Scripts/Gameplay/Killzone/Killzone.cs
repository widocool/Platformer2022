namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Component that call <see cref="AKillZoneReceiver.OnEnterKillzone(Killzone)"/> on any <see cref="AKillZoneReceiver"/> that enter its trigger.
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class Killzone : MonoBehaviour
	{
		[SerializeField]
		private PhysicsTriggerEvent _physicsTriggerEvent = null;

		private void Awake()
		{
			var rb = GetComponent<Rigidbody>();
			rb.isKinematic = true;
			rb.useGravity = false;
		}

		private void OnEnable()
		{
			_physicsTriggerEvent._onTriggerEnter.RemoveListener(OnPhysicsTriggerEnter);
			_physicsTriggerEvent._onTriggerEnter.AddListener(OnPhysicsTriggerEnter);
		}

		private void OnDisable()
		{
			_physicsTriggerEvent._onTriggerEnter.RemoveListener(OnPhysicsTriggerEnter);
		}

		private void OnPhysicsTriggerEnter(PhysicsTriggerEvent physicsTriggerEvent, Collider other)
		{
			var concreteOther = other.GetComponentInParent<AKillZoneReceiver>();
			if (concreteOther != null)
			{
				concreteOther.OnEnterKillzone(this);
			}
		}
	}
}