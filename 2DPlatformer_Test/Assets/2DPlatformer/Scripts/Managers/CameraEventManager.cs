namespace GSGD2
{
	using Cinemachine;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Gameplay;
	using GSGD2.Utilities;

	/// <summary>
	/// Manager component that needs to be added only once in the scene and added to <see cref="LevelReferences"/>.
	/// CameraEventManager take care of camera management, such as controlling CinemachineStateDrivenCamera, activate or deactivate main camera, shake event, confine camera into a room, etc...
	/// </summary>
	public class CameraEventManager : MonoBehaviour
	{
		public enum CameraState
		{
			Default,
			LookAtFall
		}

		private const string _defaultTriggerName = "Default";
		private const string _lookAtFallTriggerName = "LookAtFall";

		[SerializeField]
		private CinemachineStateDrivenCamera _smCamera = null;

		[SerializeField]
		private float _cameraLookAtFallStateVelocityThreshold = 25f;

		[SerializeField]
		private Timer _cameraLookAtFallStateDurationThreshold = null;

		private Animator _cameraAnimator = null;

		private CinemachineImpulseSource _globalImpulseSource = null;
		private CinemachineConfiner _confiner = null;

		private PlayerStart _playerStart = null;

		private DisplacementEstimationUpdater _displacementEstimationUpdater = null;

		private CameraState _previousCameraState = 0;
		private CameraState _cameraStateInTransition = 0;

		private CinemachineRoomCameraInteractor _currentRoomCameraInteractor = null;

		public void EnterRoomCamera(CinemachineRoomCameraInteractor currentRoomCameraInteractor)
		{
			// Warning : no protection to handle embedded RoomCamera. (using the Cheat manager will forget to disable bounding rooms)
			_currentRoomCameraInteractor = currentRoomCameraInteractor;
		}

		public void ExitRoomCamera()
		{
			if (_currentRoomCameraInteractor != null)
			{
				_currentRoomCameraInteractor.ResetBehavior();
			}
			_currentRoomCameraInteractor = null;
		}

		public bool TryGetLiveCamera(out CinemachineVirtualCamera liveCamera)
		{
			return (liveCamera = _smCamera.LiveChild as CinemachineVirtualCamera) != null;
		}

		public void ChangeCameraStateImmediate(CameraState cameraState)
		{
			if (cameraState == _previousCameraState) return;
			_cameraLookAtFallStateDurationThreshold.ForceFinishState();
			DoChangeCameraState(cameraState);
		}

		public void ChangeCameraState(CameraState cameraState)
		{
			switch (cameraState)
			{
				case CameraState.Default:
					if (cameraState != _previousCameraState)
					{
						DoChangeCameraState(cameraState);
					}
					break;
				case CameraState.LookAtFall:
				{
					bool hasDuration = _cameraLookAtFallStateDurationThreshold.Duration > 0;
					if (hasDuration == true)
					{
						if (cameraState != _cameraStateInTransition)
						{
							_cameraLookAtFallStateDurationThreshold.ForceFinishState();
							_cameraStateInTransition = cameraState;
							_cameraLookAtFallStateDurationThreshold.Start();
						}
					}
					else if (cameraState != _previousCameraState)
					{
						DoChangeCameraState(cameraState);
					}
				}
				break;
				default: break;
			}
		}

		private void DoChangeCameraState(CameraState cameraState)
		{
			if (cameraState == _previousCameraState) return;
			switch (cameraState)
			{
				case CameraState.Default:
				{
					_cameraAnimator.SetTrigger(_defaultTriggerName);
				}
				break;
				case CameraState.LookAtFall:
				{
					_cameraAnimator.SetTrigger(_lookAtFallTriggerName);
				}
				break;
				default: break;
			}
			_previousCameraState = _cameraStateInTransition = cameraState;
		}

		[ContextMenu("Shake")]
		public void Shake()
		{
			Shake(1);
		}

		public void Shake(float force)
		{
			_globalImpulseSource.GenerateImpulse(force);
		}

		public void SetActiveCameraConfiner(bool isActive, Collider boundingVolume = null)
		{
			if (LevelReferences.Instance.CameraEventManager.TryGetLiveCamera(out CinemachineVirtualCamera liveCamera) == true)
			{
				_confiner = liveCamera.GetComponent<CinemachineConfiner>();
			}

			if (isActive == true && boundingVolume != null)
			{
				_confiner.m_BoundingVolume = boundingVolume;
				_confiner.enabled = true;
			}
			else
			{
				_confiner.enabled = false;
				_confiner.m_BoundingVolume = null;
			}
		}

		public void SetActiveMainCamera(bool isActive)
		{
			if (TryGetLiveCamera(out CinemachineVirtualCamera liveCamera) == true)
			{
				SetActiveCamera(liveCamera, isActive);
			}
			else
			{
				Debug.LogErrorFormat("{0}.SetActiveMainCamera() fail to GetLiveCamera", GetType().Name);
			}
		}

		[ContextMenu("Reset Camera")]
		public void ResetCamera()
		{
			if (TryGetLiveCamera(out CinemachineVirtualCamera liveCamera) == true)
			{
				if (liveCamera.gameObject.activeSelf == false)
				{
					SetActiveCamera(liveCamera, true);
				}
			}
			else
			{
				Debug.LogErrorFormat("{0}.SetActiveMainCamera() fail to GetLiveCamera", GetType().Name);
			}
		}

		private void SetActiveCamera(CinemachineVirtualCamera camera, bool isActive)
		{
			camera.gameObject.SetActive(isActive);
		}

		private void OnEnable()
		{
			_cameraAnimator = _smCamera.GetComponent<Animator>();
			_globalImpulseSource = LevelReferences.Instance.Camera.GetComponent<CinemachineImpulseSource>();
			_playerStart = LevelReferences.Instance.PlayerStart;
			var player = LevelReferences.Instance.Player;
			_displacementEstimationUpdater = player.GetComponent<DisplacementEstimationUpdater>();

			_playerStart.BeforePlayerPositionReset -= PlayerStartOnPlayerPositionReset;
			_playerStart.BeforePlayerPositionReset += PlayerStartOnPlayerPositionReset;
		}

		private void OnDisable()
		{
			var player = LevelReferences.Instance.Player;
			_playerStart.BeforePlayerPositionReset -= PlayerStartOnPlayerPositionReset;
		}

		private void Update()
		{
			CameraState newCameraState = CameraState.Default;
			if (_displacementEstimationUpdater.AverageSpeed > _cameraLookAtFallStateVelocityThreshold && _displacementEstimationUpdater.MovementDirection.Down)
			{
				newCameraState = CameraState.LookAtFall;
			}
			ChangeCameraState(newCameraState);
			if (_cameraStateInTransition == CameraState.LookAtFall
				&& _cameraLookAtFallStateDurationThreshold.IsRunning == true
				&& _cameraLookAtFallStateDurationThreshold.Update() == true)
			{
				DoChangeCameraState(_cameraStateInTransition);
			}
		}

		private void PlayerStartOnPlayerPositionReset(PlayerStart sender, PlayerStart.PlayerStartEventArgs args)
		{
			ResetCamera();
		}

		private void OnValidate()
		{
			_cameraLookAtFallStateVelocityThreshold = Mathf.Clamp(_cameraLookAtFallStateVelocityThreshold, 0f, float.MaxValue);
		}
	}
}