namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.InputSystem;
	using GSGD2.Player;
	using GSGD2.Utilities;
	using GSGD2.UI;
	using Key = GSGD2.UI.ControllerDescription.Key;

	/// <summary>
	/// Component that can be added on the same GameObject as an <see cref="Item"/>. When holding the item, ProjectileShooter can use UseItem controller event to instantiate projectile in front of it.
	/// Direction can be setup from the mouse with <see cref="MouseToWorld2D"/> or with the right joystick. It is not possible to enable both the mouse direction and the joystick direction (see cref="MouseToWorld2D"/>).
	/// </summary>
	[RequireComponent(typeof(Item))]
	public class ProjectileLauncherController : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private ProjectileLauncher _projectileLauncher = null;

		[SerializeField]
		private Item _item = null;

		[SerializeField]
		private Collider _collider = null;

		[SerializeField]
		private Raycaster _raycaster = null;

		[SerializeField]
		private LineRenderer _lineRenderer = null;

		[SerializeField]
		private Rotator _rotator = null;

		[SerializeField]
		private CameraAimController _cameraAimController = null;

		[SerializeField]
		private KeyLayoutDescription _overridingKeyLayoutDescription = null;

		[Tooltip("Only gamepad")]
		[SerializeField]
		private bool _preventUseIfNoDirection = false; // only gamepad

		private MouseToWorld2D _mouseToWorld = null;
		private PlayerController _playerController = null;
		private CameraAimController _playerCameraAimController = null;
		private LayoutReminderMenu _layoutReminder = null;
		private Vector3 _rotatorLocalPositionAtStart;

		private void Awake()
		{
			_rotatorLocalPositionAtStart = _rotator.transform.localPosition;

			_item.WorldPositionReset -= Item_WorldPositionReset;
			_item.WorldPositionReset += Item_WorldPositionReset;

			_item.StateChanged -= Item_StateChanged;
			_item.StateChanged += Item_StateChanged;

			LevelReferences levelReference = LevelReferences.Instance;
			levelReference.PlayerReferences.TryGetPlayerController(out _playerController);
			levelReference.PlayerReferences.TryGetCameraAimController(out _playerCameraAimController);

			_mouseToWorld = levelReference.MouseToWorld2D;
			_layoutReminder = levelReference.UIManager.PlayerHUD.LayoutReminder;

			//if (_cameraAimController != null)
			//{
			//	_cameraAimController.InitializeFromOthers(_playerCameraAimController);
			//	_cameraAimController.SetComponentEnabled(false);
			//}
			enabled = false;
		}

		private void OnDestroy()
		{
			_item.WorldPositionReset -= Item_WorldPositionReset;
			_item.StateChanged -= Item_StateChanged;
		}

		private void OnEnable()
		{
			_playerController.UseItemPerformed -= PlayerController_UseItemPerformed;
			_playerController.UseItemPerformed += PlayerController_UseItemPerformed;

			if (_playerCameraAimController != null)
			{
				_playerCameraAimController.SetComponentEnabled(false);
			}
			if (_cameraAimController != null)
			{
				_cameraAimController.SetComponentEnabled(true);
			}

			_lineRenderer.enabled = true;
		}

		private void OnDisable()
		{
			_playerController.UseItemPerformed -= PlayerController_UseItemPerformed;

			if (_playerCameraAimController != null)
			{
				_playerCameraAimController.SetComponentEnabled(true);
			}
			if (_cameraAimController != null)
			{
				_cameraAimController.SetComponentEnabled(false);
			}

			_lineRenderer.enabled = false;
		}

		private void Item_WorldPositionReset(Item sender, Item.ItemEventArgs args)
		{
			enabled = false;
			_rotator.Play();
		}

		private void Item_StateChanged(Item sender, Item.ItemEventArgs args)
		{
			var previousState = args.previousState;
			switch (args.currentState)
			{
				case Item.State.Idle:
				{
					if (previousState == Item.State.Held)
					{
						// Reset override
						UpdateLayoutReminder(false);
						_collider.enabled = true;
						_rotator.Play();
						_rotator.transform.localPosition = _rotatorLocalPositionAtStart;
					}
				}
				break;
				case Item.State.Highlighted:
				{
					enabled = false;
					_rotator.Play();
					_rotator.transform.localPosition = _rotatorLocalPositionAtStart;
				}
				break;
				case Item.State.Held:
				{
					enabled = true;
					_rotator.Stop();
					_rotator.transform.rotation = Quaternion.identity;

					// override menu
					UpdateLayoutReminder(true);
					_collider.enabled = false;


				}
				break;
				default: break;
			}
		}

		private void UpdateLayoutReminder(bool isActive)
		{
			if (_overridingKeyLayoutDescription == null) return;
			if (isActive == true)
			{
				_layoutReminder.OverrideLayout(Key.UseItem, _overridingKeyLayoutDescription);
				_layoutReminder.SetActiveLayoutMenu(Key.UseItem, true);
			}
			else
			{
				_layoutReminder.SetActiveLayoutMenu(Key.UseItem, false);
				_layoutReminder.ResetOverrideLayout(Key.UseItem);
			}
		}

		private void Update()
		{
			if (_item.CurrentState == Item.State.Held)
			{
				Vector3 direction = Vector3.zero;
				bool useMouseForDirection = _playerController.UseMouseForLookDirection;
				if (useMouseForDirection == true)
				{
					direction = _mouseToWorld.TargetPosition - _rotator.transform.position;
				}
				else
				{
					direction = _playerController.LookDirection;
				}
				if (direction != Vector3.zero)
				{
					if (_preventUseIfNoDirection == true && _lineRenderer.enabled == false)
					{
						_lineRenderer.enabled = true;
					}
					_rotator.transform.rotation = Quaternion.LookRotation(direction, _rotator.transform.up);
				}
				else
				{
					_rotator.transform.rotation = Quaternion.LookRotation(_playerController.transform.forward);
					if (_preventUseIfNoDirection == true && _lineRenderer.enabled == true)
					{
						_lineRenderer.enabled = false;
					}
				}
				UpdateLineRenderer();
				_projectileLauncher.UpdateTimer();
			}
		}

		private void UpdateLineRenderer()
		{
			bool result = _raycaster.Raycast(out RaycastHit hit);
			var projectileInstanceOffset = _projectileLauncher.ProjectileInstanceOffset;
			Vector3 startPosition = projectileInstanceOffset.position;
			if (result == true)
			{
				_lineRenderer.SetPositions(new Vector3[2] { startPosition, hit.point });
			}
			else
			{
				_lineRenderer.SetPositions(new Vector3[2] { startPosition, startPosition + projectileInstanceOffset.forward * _raycaster.MaxDistance });
			}
		}

		private bool CanUse()
		{
			bool isFacingOnXYPlane = Mathf.Abs(Vector3.Dot(Vector3.forward, transform.forward)) == 1;
			if (isFacingOnXYPlane == false)
			{
				return false;
			}
			if (_preventUseIfNoDirection == true && isFacingOnXYPlane)
			{
				return _playerController.LookDirection != Vector3.zero;
			}
			return true;
		}

		private void PlayerController_UseItemPerformed(PlayerController sender, InputAction.CallbackContext obj)
		{
			if (CanUse() == true)
			{
				_projectileLauncher.LaunchProjectile();
			}
		}

	}
}