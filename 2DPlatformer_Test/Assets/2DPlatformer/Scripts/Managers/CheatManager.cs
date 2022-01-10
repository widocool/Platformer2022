namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.InputSystem;
	using GSGD2.Player;

	/// <summary>
	/// Class that permit to add cheat functionnality to ease production.
	/// </summary>
	public class CheatManager : MonoBehaviour
	{
		private const string GO_TO_PREVIOUS_CHECKPOINT_ACTION_NAME = "GoToPreviousCheckpoint";
		private const string GO_TO_NEXT_CHECKPOINT_ACTION_NAME = "GoToNextCheckpoint";

		[SerializeField]
		private InputActionMapWrapper _inputActionMapWrapper;

		private InputAction _goToPreviousCheckpointInputAction = null;
		private InputAction _goToNextCheckpointInputAction = null;

		private PlayerStart _playerStart = null;
		private CameraEventManager _cameraEventManager = null;

		private void Awake()
		{
			var levelReference = LevelReferences.Instance;
			_playerStart = levelReference.PlayerStart;
			_cameraEventManager = levelReference.CameraEventManager;
		}

		private void OnEnable()
		{
			if (_inputActionMapWrapper.TryFindAction(GO_TO_PREVIOUS_CHECKPOINT_ACTION_NAME, out _goToPreviousCheckpointInputAction, true) == true)
			{
				_goToPreviousCheckpointInputAction.performed -= GoToPreviousCheckpointInputActionOnPerformed;
				_goToPreviousCheckpointInputAction.performed += GoToPreviousCheckpointInputActionOnPerformed;
			}
			if (_inputActionMapWrapper.TryFindAction(GO_TO_NEXT_CHECKPOINT_ACTION_NAME, out _goToNextCheckpointInputAction, true) == true)
			{
				_goToNextCheckpointInputAction.performed += GoToNextCheckpointInputActionOnPerformed;
				_goToNextCheckpointInputAction.performed += GoToNextCheckpointInputActionOnPerformed;
			}
		}

		private void OnDisable()
		{
			_goToPreviousCheckpointInputAction.Disable();
			_goToNextCheckpointInputAction.Disable();

			_goToPreviousCheckpointInputAction.performed -= GoToPreviousCheckpointInputActionOnPerformed;
			_goToNextCheckpointInputAction.performed -= GoToPreviousCheckpointInputActionOnPerformed;

		}

		private void GoToPreviousCheckpointInputActionOnPerformed(InputAction.CallbackContext obj)
		{
			_playerStart.SetPlayerPositionToCheckpoint(false);
			ResetCameraSettings();
		}

		private void GoToNextCheckpointInputActionOnPerformed(InputAction.CallbackContext obj)
		{
			_playerStart.SetPlayerPositionToCheckpoint(true);
			ResetCameraSettings();
		}

		private void ResetCameraSettings()
		{
			_cameraEventManager.SetActiveCameraConfiner(false);
			_cameraEventManager.ExitRoomCamera();
		}
	}
}