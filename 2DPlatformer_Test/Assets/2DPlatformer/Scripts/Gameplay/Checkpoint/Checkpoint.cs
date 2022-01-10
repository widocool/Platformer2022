namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GSGD2.Player;

	/// <summary>
	/// Register a position where the player will be reset if entering the <see cref="KillZone"/>.
	/// The index can indicate the checkpoint, but it is not used by the framework itself.
	/// Checkpoint must be on the same gameobject as the collider since we'll not using rigidbody.
	/// Therefore, a <see cref="CheckpointFeedbackHandler"/> must be added as parent.
	/// </summary>
	public class Checkpoint : MonoBehaviour
	{
		[SerializeField]
		private PhysicsTriggerEvent _physicsTriggerEvent = null;

		[SerializeField]
		private int _index = 0;

		private bool _hasBeenTriggeredYet = false;

		public int Index => _index;

		public enum EventType
		{
			FirstTimeReachedCheckpoint,
			CheckpointPassed,
			CheckpointExited
		}

		public delegate void CheckpointEvent(Checkpoint checkpoint, EventType eventType);
		public event CheckpointEvent _checkpointTriggered = null;

		public UnityEvent<Checkpoint> NewCheckpointAdded = null;
		public UnityEvent<Checkpoint> CheckpointPassed = null;
		public UnityEvent<Checkpoint> CheckpointExited = null;

		private void OnEnable()
		{
			LevelReferences.Instance.PlayerStart.AddCheckpoint(this);

			_physicsTriggerEvent._onTriggerEnter.RemoveListener(OnPhysicsTriggerEventEnter);
			_physicsTriggerEvent._onTriggerEnter.AddListener(OnPhysicsTriggerEventEnter);
			_physicsTriggerEvent._onTriggerExit.RemoveListener(OnPhysicsTriggerEventExit);
			_physicsTriggerEvent._onTriggerExit.AddListener(OnPhysicsTriggerEventExit);

			_checkpointTriggered -= OnCheckpointTriggered;
			_checkpointTriggered += OnCheckpointTriggered;
		}

		private void OnDisable()
		{
			_physicsTriggerEvent._onTriggerEnter.RemoveListener(OnPhysicsTriggerEventEnter);
			_physicsTriggerEvent._onTriggerExit.RemoveListener(OnPhysicsTriggerEventExit);

			_checkpointTriggered -= OnCheckpointTriggered;
		}

		private void OnPhysicsTriggerEventEnter(PhysicsTriggerEvent physicsTriggerEvent, Collider other)
		{
			CubeController concreteOther = other.GetComponentInParent<CubeController>();
			if (concreteOther != null)
			{
				LevelReferences.Instance.PlayerStart.UpdateLastCheckpoint(this);
				_checkpointTriggered.Invoke(this, _hasBeenTriggeredYet == true ? EventType.CheckpointPassed : EventType.FirstTimeReachedCheckpoint);
				_hasBeenTriggeredYet = true;
			}
		}

		private void OnPhysicsTriggerEventExit(PhysicsTriggerEvent physicsTriggerEvent, Collider other)
		{
			CubeController concreteOther = other.GetComponentInParent<CubeController>();
			if (concreteOther != null)
			{
				_checkpointTriggered.Invoke(this, EventType.CheckpointExited);
			}
		}

		private void OnCheckpointTriggered(Checkpoint checkpoint, EventType eventType)
		{
			switch (eventType)
			{
				case EventType.FirstTimeReachedCheckpoint:
				{
					NewCheckpointAdded.Invoke(this);
				}
				break;
				case EventType.CheckpointPassed:
				{
					CheckpointPassed.Invoke(this);
				}
				break;
				case EventType.CheckpointExited:
				{
					CheckpointExited.Invoke(this);
				}
				break;
				default: break;
			}
		}
	}
}