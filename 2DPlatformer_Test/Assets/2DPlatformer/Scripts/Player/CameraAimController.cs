namespace GSGD2.Player
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Animations;

	/// <summary>
	/// Class that can listen to horizontal and vertical look inputs and move <see cref="CameraAim"/>, the gameobject that being aim at by cinemachine's vcam. It can shift the CameraAim position in Y, Z or YZ axis and can be placed on the player as well as on an <see cref="Item"/> (See ProjectileLauncher prefab for an example).
	/// </summary>
	public class CameraAimController : MonoBehaviour
	{
		private enum Axis
		{
			Y,
			Z,
			YZ
		}

		[SerializeField]
		private bool _enabledAtStart = true;

		[SerializeField]
		private PositionConstraint _cameraAimTransformParent = null;

		[SerializeField]
		private CameraAim _cameraAim = null;

		[SerializeField]
		private Axis _axis = 0;

		[SerializeField]
		private float _distance = 10f;

		[SerializeField]
		private float _speed = 10f;

		private PlayerController _playerController = null;

		public PositionConstraint CameraAimTransformParent => _cameraAimTransformParent;
		public CameraAim CameraAim => _cameraAim;

		public void InitializeFromOthers(CameraAimController other)
		{
			_playerController = other._playerController;
			_cameraAimTransformParent = other._cameraAimTransformParent;
			_cameraAim = other._cameraAim;
		}

		public void SetComponentEnabled(bool isEnabled, bool resetCameraPosition = false)
		{
			if (resetCameraPosition == true)
			{
				_cameraAim.transform.localPosition = Vector3.zero;
			}
			enabled = isEnabled;
		}

		private void Awake()
		{
			PlayerReferences playerReferences = GetComponent<PlayerReferences>();

			bool isPlayer = playerReferences != null;
			if (isPlayer == false)
			{
				playerReferences = LevelReferences.Instance.PlayerReferences;
				if (playerReferences.TryGetCameraAimController(out CameraAimController cameraAimController) == true)
				{
					this.InitializeFromOthers(cameraAimController);
				}
			}

			playerReferences.TryGetPlayerController(out _playerController);
			enabled = _enabledAtStart;
		}

		private void Start()
		{
			_cameraAimTransformParent.transform.SetParent(null);
		}

		private void Update()
		{
			bool hasInput = false;
			switch (_axis)
			{
				case Axis.Y:
				{
					var hLook = _playerController.VerticalLook;
					if (Mathf.Approximately(hLook, 0) == false)
					{
						DoMovement(Vector3.up * hLook);
						hasInput = true;
					}
				}
				break;
				case Axis.Z:
				{
					var vLook = _playerController.HorizontalLook;
					if (Mathf.Approximately(vLook, 0) == false)
					{
						DoMovement(Vector3.forward * vLook);
						hasInput = true;
					}
				}
				break;
				case Axis.YZ:
				{
					var lookDirection = _playerController.LookDirection;
					if (lookDirection != Vector3.zero)
					{
						DoMovement(lookDirection);
						hasInput = true;
					}
				}
				break;
				default: break;
			}

			if (hasInput == false)
			{
				DoMovement(Vector3.zero);
			}
		}

		private void DoMovement(Vector3 direction)
		{

			_cameraAim.transform.localPosition = Vector3.MoveTowards(_cameraAim.transform.localPosition, direction * _distance, Time.deltaTime * _speed);
		}
	}
}