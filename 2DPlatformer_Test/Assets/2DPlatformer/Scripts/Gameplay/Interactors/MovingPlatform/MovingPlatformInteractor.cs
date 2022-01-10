namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Utilities;

	/// <summary>
	/// Interactor that can controls a <see cref="MovingPlatform"/> with various settings.
	/// Triggered only with <see cref="MovingPlatformInteractorActivator"/>
	/// It is recommended to pass Trigger functions to a <see cref="PhysicsTriggerEvent"/> component.
	/// </summary>
	public class MovingPlatformInteractor : AInteractor
	{
		#region Internal classes
		// TODO AL : Can be derived from a base class to use the pattern elsewhere
		[System.Serializable]
		private class BoolOptionalSettings
		{
			[SerializeField]
			private bool _enabled = false;
			[SerializeField]
			private bool _value = false;

			public bool Enabled => _enabled;
			public bool Value => _value;

			public BoolOptionalSettings(bool enabled, bool value)
			{
				_enabled = enabled;
				_value = value;
			}
		}

		[System.Serializable]
		public class Settings
		{
			public enum Behavior
			{
				Play,
				PlayReverse,
				PingPong,
				Stop
			}

			[SerializeField]
			private bool _enabled = true;

			[SerializeField]
			private Behavior _behavior = 0;

			[SerializeField]
			private bool _interrupt = false;

			[SerializeField]
			private BoolOptionalSettings _stopWhenAtEndOfPath = null;

			public bool Enabled => _enabled;

			public void Apply(MovingPlatform movingPlatform)
			{
				if (_enabled == false) return;

				if (_stopWhenAtEndOfPath.Enabled == true)
				{
					movingPlatform.SetStopWhenAtEndOfPath(_stopWhenAtEndOfPath.Value);
				}

				switch (_behavior)
				{
					case Behavior.Play:
					{
						movingPlatform.Play(_interrupt, false);
					}
					break;
					case Behavior.PlayReverse:
					{
						movingPlatform.PlayReverse(_interrupt);
					}
					break;
					case Behavior.PingPong:
					{
						movingPlatform.PlayPingPong(_interrupt);
					}
					break;
					case Behavior.Stop:
					{
						movingPlatform.Stop(_interrupt);
					}
					break;
					default: break;
				}
			}
		}
		#endregion Internal classes

		#region Fields
		[Header("References")]
		[SerializeField]
		private MovingPlatform _movingPlatform = null;

		[Header("Gameplay")]
		[SerializeField]
		private Settings _enterSettings = null;

		[SerializeField]
		private Settings _staySettings = null;

		[SerializeField]
		private Settings _exitSettings = null;

		[Tooltip("Interaction that is manually triggered in code")]
		[SerializeField]
		private Settings _directInteractionSettings = null;

		[Header("Editor / ReadOnly")]
		[SerializeField]
		private DrawLinkUtility _drawLinkToPlatform = null;

		// runtime
		private List<MovingPlatformInteractorActivator> _activators = new List<MovingPlatformInteractorActivator>();
		#endregion Fields

		#region Methods

		public override void InteractFromTriggerEnter(PhysicsTriggerEvent sender, Collider other)
		{
			if (ShouldInteractWith(other, out MovingPlatformInteractorActivator activator) == true)
			{
				base.InteractFromTriggerEnter(sender, other);
				if (_activators.Contains(activator) == false)
				{
					_enterSettings.Apply(_movingPlatform);
					_activators.Add(activator);
				}
			}
		}

		public override void InteractFromTriggerStay(PhysicsTriggerEvent sender, Collider other)
		{
			if (ShouldInteractWith<MovingPlatformInteractorActivator>(other) == true)
			{
				base.InteractFromTriggerStay(sender, other);
				_staySettings.Apply(_movingPlatform);
			}
		}

		// Exit only if all activators has left range
		public override void InteractFromTriggerExit(PhysicsTriggerEvent sender, Collider other)
		{
			if (ShouldInteractWith(other, out MovingPlatformInteractorActivator activator) == true)
			{
				_activators.Remove(activator);
				if (_activators.Count == 0)
				{
					base.InteractFromTriggerExit(sender, other);
					_exitSettings.Apply(_movingPlatform);
				}
			}
		}

		public override void Interact()
		{
			_directInteractionSettings.Apply(_movingPlatform);
		}

		protected bool ShouldInteractWith(Collider other)
		{
			// enum + typeof to change type
			return ShouldInteractWith<MovingPlatformInteractorActivator>(other);
		}

#if UNITY_EDITOR
		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if (_movingPlatform != null)
			{
				_drawLinkToPlatform.DrawBezierLink(transform.position, _movingPlatform.transform.position);
			}
		}
#endif //UNITY_EDITOR
		#endregion Methods

	}
}