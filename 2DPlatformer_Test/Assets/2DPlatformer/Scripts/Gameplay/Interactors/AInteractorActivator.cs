namespace GSGD2.Gameplay
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Base component class of any object that needs to trigger a <see cref="AInteractor"/>.
	/// It can ben setup to correspond to filter interaction with all AInteractor.
	/// It respect restriction list settings in <see cref="AInteractor._restrictionList"/>.
	/// </summary>
	public class AInteractorActivator : MonoBehaviour
	{
		public enum Mode
		{
			Passive,
			Active
		}

		[Header("Passive mode will let Interactor find this component and do not needs PhysicsTriggerEvent and Active mode needs a rigidbody and a PhysicsTriggerEvent to interact.")]
		[SerializeField]
		private Mode _mode = 0;

		[SerializeField]
		private PhysicsTriggerEvent _physicsTriggerEvent = null;

		[SerializeField]
		private InteractWith _interactWithOnActiveMode = 0;

		// With vanilla Unity we can't display a Type and its child classes.
		// So we needs to keep this enum up to date
		// Do not forget to add it to GetTypes too
		[System.Flags]
		private enum InteractWith : int
		{
			Nothing = 0,
			Interactor = 1 << 0,
			CinemachineRoomCameraInteractor = 1 << 1,
			MovingPlatformInteractor = 1 << 2,
			Everything = ~(Nothing),
		}

		private void OnEnable()
		{
			if (_mode == Mode.Active)
			{
				_physicsTriggerEvent._onTriggerEnter.RemoveListener(OnTriggerEventEnter);
				_physicsTriggerEvent._onTriggerEnter.AddListener(OnTriggerEventEnter);
			}
		}

		private void OnDisable()
		{
			if (_mode == Mode.Active)
			{
				_physicsTriggerEvent._onTriggerEnter.RemoveListener(OnTriggerEventEnter);
			}
		}

		private void OnTriggerEventEnter(PhysicsTriggerEvent triggerEvent, Collider other)
		{
			if (_mode == Mode.Active)
			{
				if (CanInteract(other, out AInteractor interactor) == true)
				{
					interactor.Interact();
				}
			}
		}

		private bool CanInteract(Collider other, out AInteractor interactor)
		{
			bool result = GetTypes().CanInteractWithType(other.transform, out interactor);;

			if (result == true)
			{
				result = interactor.ShouldInteractWithRestrictedList(interactor);
			}
			return result;
		}

		// Add new types of interaction here
		private List<Type> GetTypes()
		{
			List<Type> types = new List<Type>();
			if (_interactWithOnActiveMode.HasFlag(InteractWith.Everything) == true)
			{
				types.Add(typeof(AInteractor));
			}
			else if (_interactWithOnActiveMode.HasFlag(InteractWith.Interactor) == true)
			{
				types.Add(typeof(InteractorProxy));
			}
			else if (_interactWithOnActiveMode.HasFlag(InteractWith.CinemachineRoomCameraInteractor) == true)
			{
				types.Add(typeof(CinemachineRoomCameraInteractor));

			}
			else if (_interactWithOnActiveMode.HasFlag(InteractWith.MovingPlatformInteractor) == true)
			{
				types.Add(typeof(MovingPlatformInteractor));
			}
			return types;
		}
	}
}