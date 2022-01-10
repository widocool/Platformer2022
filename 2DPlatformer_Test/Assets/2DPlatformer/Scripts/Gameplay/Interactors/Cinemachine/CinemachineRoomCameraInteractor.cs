namespace GSGD2.Gameplay
{
	using Cinemachine;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Interactor that trigger a new "room camera" when entering in a trigger.
	/// It can be set up to activate or deactivate the room camera.
	/// Works with any type of <see cref="AInteractorActivator"/>.
	/// </summary>
	public class CinemachineRoomCameraInteractor : AInteractor
	{
		private enum Behavior
		{
			SetFollowTarget,
			ResetFollowTargetToPlayer,
			AddCameraConfiner,
			RemoveCameraConfiner
		}

		[SerializeField]
		private Behavior _behavior = 0;

		[Header("FollowTarget behavior")]
		[SerializeField]
		private CinemachineVirtualCamera _roomCamera = null;

		[SerializeField]
		private CinemachineTargetGroup _targetGroup = null;

		[Header("Confiner behavior")]
		[SerializeField]
		private Collider _confinerCollider = null;
		private CameraEventManager _cameraEventManager = null;

		protected override void OnEnable()
		{
			base.OnEnable();
			_cameraEventManager = LevelReferences.Instance.CameraEventManager;
		}

		public override void InteractFromTriggerEnter(PhysicsTriggerEvent sender, Collider other)
		{
			if (ShouldInteractWith<AInteractorActivator>(other, out AInteractorActivator activator) == true)
			{
				base.InteractFromTriggerEnter(sender, other);
				Interact();
			}
		}

		public override void Interact()
		{
			switch (_behavior)
			{
				case Behavior.SetFollowTarget:
					_roomCamera.gameObject.SetActive(true);
					_targetGroup.gameObject.SetActive(true);
					_cameraEventManager.EnterRoomCamera(this);
					break;
				case Behavior.ResetFollowTargetToPlayer:
					_roomCamera.gameObject.SetActive(false);
					_targetGroup.gameObject.SetActive(false);
					_cameraEventManager.ExitRoomCamera();
					break;
				case Behavior.AddCameraConfiner:
				{
					_cameraEventManager.SetActiveCameraConfiner(true, _confinerCollider);
				}
				break;
				case Behavior.RemoveCameraConfiner:
				{
					_cameraEventManager.SetActiveCameraConfiner(false);
				}
				break;
				default: break;
			}
		}


		// TODO AL : Some architecture pitfalls here : duplicated and confusing ways of reset behaviour (exit room and reset behavior), not always used at the same time and some inverted controls. tb retaked
		// ideas : Interactors listen when they needs to be reset, but it will increase class coupling. Another component may take care of this, taking an interator and matching the event with the reset.
		public void ResetBehavior()
		{
			switch (_behavior)
			{
				case Behavior.SetFollowTarget:
				case Behavior.ResetFollowTargetToPlayer:
				{
					_roomCamera.gameObject.SetActive(false);
					_targetGroup.gameObject.SetActive(false);
				}
				break;
				case Behavior.AddCameraConfiner:
				case Behavior.RemoveCameraConfiner:
				{
					_cameraEventManager.SetActiveCameraConfiner(false);
				}
				break;
				default: break;
			}
		}
	}
}