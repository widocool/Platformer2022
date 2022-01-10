namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Utilities;

	/// <summary>
	/// Component that put on a object being translated (such as a <see cref="MovingPlatform"/>), transmit its translation to object that its in its trigger area.  Disabling the <see cref="MovingPlatformStickToGroundReceiver"/> component disable its ability to be moved.
	/// Following the <see cref="MovingPlatformStickToGroundReceiver.MoveMode"/>, it will be parenting the object or transmit its deltaPosition to it.
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class MovingPlatformStickToGround : MonoBehaviour
	{
		[Header("Only MovingPlatformStickRigidbodyToGroundReceiver will follow the platform")]
		[SerializeField]
		private DisplacementEstimationUpdater _displacementEstimationUpdater = null;

		[Header("ReadOnly")]
		[SerializeField]
		private List<MovingPlatformStickToGroundReceiver> _receivers = null;

		[SerializeField]
		private PhysicsTriggerEvent _physicsTriggerEvent = null;

		public void AddOnPlatform(MovingPlatformStickToGroundReceiver receiver)
		{
			bool result = receiver.SetOnPlatform(this, true);

			if (result == true)
			{
				_receivers.Add(receiver);
				switch (receiver.MoveMode)
				{
					case MovingPlatformStickToGroundReceiver.PlatformMoveMode.Parenting:
					{
						receiver.transform.SetParent(transform);
					}
					break;
					case MovingPlatformStickToGroundReceiver.PlatformMoveMode.DeltaPosition:
					{
					}
					break;
					default: break;
				}
			}
		}

		public void RemoveOnPlatform(MovingPlatformStickToGroundReceiver receiver, bool force = false)
		{
			bool result = force == true ? true : receiver.SetOnPlatform(this, false);

			if (result == true)
			{
				_receivers.Remove(receiver);
				switch (receiver.MoveMode)
				{
					case MovingPlatformStickToGroundReceiver.PlatformMoveMode.Parenting:
					{
						if (ReferenceEquals(receiver.transform.parent, transform) == true)
						{
							receiver.transform.SetParent(null);
						}
					}
					break;
					case MovingPlatformStickToGroundReceiver.PlatformMoveMode.DeltaPosition:
					{
					}
					break;
					default: break;
				}
			}
		}

		private void Awake()
		{
			Rigidbody rb = GetComponent<Rigidbody>();
			rb.isKinematic = true;
		}

		private void OnEnable()
		{
			_physicsTriggerEvent._onTriggerEnter.RemoveListener(OnPhysicsTriggerEnter);
			_physicsTriggerEvent._onTriggerEnter.AddListener(OnPhysicsTriggerEnter);
			_physicsTriggerEvent._onTriggerExit.RemoveListener(OnPhysicsTriggerExit);
			_physicsTriggerEvent._onTriggerExit.AddListener(OnPhysicsTriggerExit);
		}

		private void OnDisable()
		{
			_physicsTriggerEvent._onTriggerEnter.RemoveListener(OnPhysicsTriggerEnter);
			_physicsTriggerEvent._onTriggerExit.RemoveListener(OnPhysicsTriggerExit);
		}

		private void OnPhysicsTriggerEnter(PhysicsTriggerEvent physicsTriggerEvent, Collider other)
		{
			MovingPlatformStickToGroundReceiver receiver = other.GetComponentInParent<MovingPlatformStickToGroundReceiver>();
			if (receiver != null && receiver.enabled == true && _receivers.Contains(receiver) == false)
			{
				AddOnPlatform(receiver);
			}
		}

		private void OnPhysicsTriggerExit(PhysicsTriggerEvent physicsTriggerEvent, Collider other)
		{
			MovingPlatformStickToGroundReceiver receiver = other.GetComponentInParent<MovingPlatformStickToGroundReceiver>();

			if (receiver != null && _receivers.Contains(receiver) == true)
			{
				RemoveOnPlatform(receiver);
			}
		}

		private void FixedUpdate()
		{
			Vector3 deltaPosition = _displacementEstimationUpdater.DeltaPosition;
			for (int i = 0; i < _receivers.Count; i++)
			{
				MovingPlatformStickToGroundReceiver receiver = _receivers[i];
				if (receiver.MoveMode == MovingPlatformStickToGroundReceiver.PlatformMoveMode.DeltaPosition)
				{
					receiver.transform.position += deltaPosition;
					receiver.SetPlatformMoving(deltaPosition != Vector3.zero);
				}
			}
		}
	}
}