namespace GSGD2.Player
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.InputSystem;

	/// <summary>
	/// Component that bind InputSystem InputActionMap to events and properties to be used in game. Disabling the component disable the input listening.
	/// </summary>
	public class PlayerController : MonoBehaviour
	{
		private const string HORIZONTAL_MOVE_ACTION_NAME = "HorizontalMove";
		private const string HORIZONTAL_LOOK_ACTION_NAME = "HorizontalLook";
		private const string VERTICAL_LOOK_ACTION_NAME = "VerticalLook";
		private const string JUMP_ACTION_NAME = "Jump";
		private const string DASH_ACTION_NAME = "Dash";
		private const string WALL_GRAB_ACTION_NAME = "WallGrab";
		private const string WALL_JUMP_ACTION_NAME = "WallJump";
		private const string TAKE_ITEM_ACTION_NAME = "TakeItem";
		private const string RELEASE_ITEM_ACTION_NAME = "ReleaseItem";
		private const string USE_ITEM_ACTION_NAME = "UseItem";
		private const string GROUND_SMASH_ACTION_NAME = "GroundSmash";

		[SerializeField]
		private InputActionMapWrapper _inputActionMapWrapper;

		[SerializeField]
		private bool _useMouseForLookDirection = false;

		private InputAction _horizontalMoveInputAction = null;
		private InputAction _horizontalLookInputAction = null;
		private InputAction _verticalLookInputAction = null;
		private InputAction _jumpInputAction = null;
		private InputAction _dashInputAction = null;
		private InputAction _wallGrabInputAction = null;
		private InputAction _wallJumpInputAction = null;
		private InputAction _takeItemInputAction = null;
		private InputAction _releaseItemInputAction = null;
		private InputAction _useItemInputAction = null;
		private InputAction _groundSmashInputAction = null;

		public bool UseMouseForLookDirection => _useMouseForLookDirection;
		public float HorizontalMove => _horizontalMoveInputAction.ReadValue<float>();
		public float HorizontalLook => _horizontalLookInputAction.ReadValue<float>();
		public float VerticalLook => _verticalLookInputAction.ReadValue<float>();
		public Vector3 LookDirection => new Vector3(0f, VerticalLook, HorizontalLook);
		public bool IsJumpButtonPressed => _jumpInputAction.IsPressed();

		public delegate void InputEvent(PlayerController sender, InputAction.CallbackContext obj);
		public event InputEvent JumpPerformed = null;
		public event InputEvent DashPerformed = null;
		public event InputEvent WallGrabPerformed = null;
		public event InputEvent WallJumpPerformed = null;
		public event InputEvent TakeItemPerformed = null;
		public event InputEvent ReleaseItemPerformed = null;
		public event InputEvent UseItemPerformed = null;
		public event InputEvent GroundSmashPerformed = null;

		private void OnEnable()
		{
			_inputActionMapWrapper.TryFindAction(HORIZONTAL_MOVE_ACTION_NAME, out _horizontalMoveInputAction, true);

			_inputActionMapWrapper.TryFindAction(HORIZONTAL_LOOK_ACTION_NAME, out _horizontalLookInputAction, true);
			_inputActionMapWrapper.TryFindAction(VERTICAL_LOOK_ACTION_NAME, out _verticalLookInputAction, true);

			if (_inputActionMapWrapper.TryFindAction(JUMP_ACTION_NAME, out _jumpInputAction, true) == true)
			{
				_jumpInputAction.performed -= JumpInputAction_performed;
				_jumpInputAction.performed += JumpInputAction_performed;
			}

			if (_inputActionMapWrapper.TryFindAction(DASH_ACTION_NAME, out _dashInputAction, true) == true)
			{
				_dashInputAction.performed -= DashInputAction_performed;
				_dashInputAction.performed += DashInputAction_performed;
			}

			if (_inputActionMapWrapper.TryFindAction(WALL_GRAB_ACTION_NAME, out _wallGrabInputAction, true) == true)
			{
				_wallGrabInputAction.performed -= WallGrabMoveInputAction_performed;
				_wallGrabInputAction.performed += WallGrabMoveInputAction_performed;
			}

			if (_inputActionMapWrapper.TryFindAction(WALL_JUMP_ACTION_NAME, out _wallJumpInputAction, true) == true)
			{
				_wallJumpInputAction.performed -= WallJumpMoveInputAction_performed;
				_wallJumpInputAction.performed += WallJumpMoveInputAction_performed;
			}

			if (_inputActionMapWrapper.TryFindAction(TAKE_ITEM_ACTION_NAME, out _takeItemInputAction, true) == true)
			{
				_takeItemInputAction.performed -= TakeItemInputAction_performed;
				_takeItemInputAction.performed += TakeItemInputAction_performed;
			}

			if (_inputActionMapWrapper.TryFindAction(RELEASE_ITEM_ACTION_NAME, out _releaseItemInputAction, true) == true)
			{
				_releaseItemInputAction.performed -= ReleaseItemInputAction_performed;
				_releaseItemInputAction.performed += ReleaseItemInputAction_performed;
			}

			if (_inputActionMapWrapper.TryFindAction(USE_ITEM_ACTION_NAME, out _useItemInputAction, true) == true)
			{
				_useItemInputAction.performed -= UseItemInputAction_performed;
				_useItemInputAction.performed += UseItemInputAction_performed;
			}
			
			if (_inputActionMapWrapper.TryFindAction(GROUND_SMASH_ACTION_NAME, out _groundSmashInputAction, true) == true)
			{
				_groundSmashInputAction.performed -= GroundSmashInputAction_performed;
				_groundSmashInputAction.performed += GroundSmashInputAction_performed;
			}
		}

		private void OnDisable()
		{
			_horizontalMoveInputAction.Disable();
			_jumpInputAction.Disable();
			_dashInputAction.Disable();
			_wallGrabInputAction.Disable();
			_wallJumpInputAction.Disable();
			_takeItemInputAction.Disable();
			_releaseItemInputAction.Disable();
			_horizontalLookInputAction.Disable();
			_verticalLookInputAction.Disable();
			_verticalLookInputAction.Disable();
			_useItemInputAction.Disable();
			_groundSmashInputAction.Disable();

			_jumpInputAction.performed -= JumpInputAction_performed;
			_dashInputAction.performed -= DashInputAction_performed;
			_wallGrabInputAction.performed -= WallGrabMoveInputAction_performed;
			_wallJumpInputAction.performed -= WallJumpMoveInputAction_performed;
			_takeItemInputAction.performed -= TakeItemInputAction_performed;
			_releaseItemInputAction.performed -= ReleaseItemInputAction_performed;
			_useItemInputAction.performed -= UseItemInputAction_performed;
			_groundSmashInputAction.performed -= GroundSmashInputAction_performed;
		}

		private void JumpInputAction_performed(InputAction.CallbackContext obj)
		{
			JumpPerformed?.Invoke(this, obj);
		}

		private void DashInputAction_performed(InputAction.CallbackContext obj)
		{
			DashPerformed?.Invoke(this, obj);
		}

		private void WallGrabMoveInputAction_performed(InputAction.CallbackContext obj)
		{
			WallGrabPerformed?.Invoke(this, obj);
		}

		private void WallJumpMoveInputAction_performed(InputAction.CallbackContext obj)
		{
			WallJumpPerformed?.Invoke(this, obj);
		}

		private void TakeItemInputAction_performed(InputAction.CallbackContext obj)
		{
			TakeItemPerformed?.Invoke(this, obj);
		}

		private void ReleaseItemInputAction_performed(InputAction.CallbackContext obj)
		{
			ReleaseItemPerformed?.Invoke(this, obj);
		}
		private void UseItemInputAction_performed(InputAction.CallbackContext obj)
		{
			UseItemPerformed?.Invoke(this, obj);
		}

		private void GroundSmashInputAction_performed(InputAction.CallbackContext obj)
		{
			GroundSmashPerformed?.Invoke(this, obj);
		}
	}
}