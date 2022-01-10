namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Utilities;

#pragma warning disable CS0414

	/// <summary>
	/// Base class of an object that can be interact with by a <see cref="AInteractorActivator"/>.
	/// It as various filter, <see cref="_restrictionList"/> and the filter function can be overriden by child classes.
	/// More precise settings needs to be done in childs classes (e.g. <see cref="MovingPlatformInteractor"/> or <see cref="CinemachineRoomCameraInteractor"/>
	/// </summary>
	[SelectionBase]
	public abstract class AInteractor : MonoBehaviour
	{
		#region Fields
		[TextArea]
		[SerializeField]
		private string _memento = null;

		[SerializeField]
		private List<AInteractorActivator> _restrictionList = null;

		[SerializeField]
		private DrawLinkUtility _drawLinkUtilityToRestricted = null;

		[SerializeField]
		private bool _autoAddPlayerToRestrictionList = false;
		#endregion Fields

		#region Events
		public struct MovingPlatformInteractorEventArgs
		{
			public Collider other;
			public PhysicsTriggerEvent physicsTriggerEvent;

			public MovingPlatformInteractorEventArgs(PhysicsTriggerEvent physicsTriggerEvent, Collider other)
			{
				this.other = other;
				this.physicsTriggerEvent = physicsTriggerEvent;
			}
		}

		public delegate void MovingPlatformInteractorEvent(AInteractor sender, MovingPlatformInteractorEventArgs args);
		public event MovingPlatformInteractorEvent TriggerEntered = null;
		public event MovingPlatformInteractorEvent TriggerStayed = null;
		public event MovingPlatformInteractorEvent TriggerExited = null;
		#endregion Events

		#region Methods
		// TODO AL : if needed, add an event for direct interaction
		public abstract void Interact();

		public virtual void InteractFromTriggerEnter(PhysicsTriggerEvent sender, Collider other)
		{
			TriggerEntered?.Invoke(this, new MovingPlatformInteractorEventArgs(sender, other));
		}

		public virtual void InteractFromTriggerStay(PhysicsTriggerEvent sender, Collider other)
		{
			TriggerStayed?.Invoke(this, new MovingPlatformInteractorEventArgs(sender, other));
		}
		public virtual void InteractFromTriggerExit(PhysicsTriggerEvent sender, Collider other)
		{
			TriggerExited?.Invoke(this, new MovingPlatformInteractorEventArgs(sender, other));
		}

		public virtual bool ShouldInteractWithRestrictedList<T>(T concreteOther) where T : MonoBehaviour
		{
			if (concreteOther == null) return false;
			var restrictionList = _restrictionList;
			bool shouldInteract = restrictionList.Count == 0;
			if (shouldInteract == false)
			{
				for (int i = 0; i < restrictionList.Count; i++)
				{
					var item = restrictionList[i];
					if (ReferenceEquals(item.gameObject, concreteOther.gameObject) == true)
					{
						shouldInteract = true;
						break;
					}
				}
			}

			return shouldInteract;
		}

		protected virtual void OnEnable()
		{
			if (_autoAddPlayerToRestrictionList == true)
			{
				var playerInteractor = LevelReferences.Instance.Player.GetComponent<AInteractorActivator>();
				if (playerInteractor != null)
				{
					_restrictionList.Add(playerInteractor);
				}
			}
		}

		protected virtual bool ShouldInteractWith<T>(Collider other) where T : MonoBehaviour
		{
			return ShouldInteractWith<T>(other, out _);
		}

		protected virtual bool ShouldInteractWith<T>(Collider other, out T concreteOther) where T : MonoBehaviour
		{
			concreteOther = other.GetComponentInParent<T>();
			return concreteOther != null && concreteOther.enabled == true && ShouldInteractWithRestrictedList(concreteOther); // If issue with interactor, check concrete other (activator) enabled state
		}

		protected virtual void OnDrawGizmos()
		{
			if (_restrictionList != null)
			{
				var startPosition = transform.position;
				for (int i = 0; i < _restrictionList.Count; i++)
				{
					AInteractorActivator restrictedObj = _restrictionList[i];
					if (restrictedObj)
					{
						_drawLinkUtilityToRestricted.DrawBezierLink(startPosition, restrictedObj.transform.position);
					}
				}
			}
		}
		#endregion Methods
	}
}