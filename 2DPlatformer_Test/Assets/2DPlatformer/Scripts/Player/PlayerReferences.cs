namespace GSGD2.Player
{
	using GSGD2.Gameplay;
	using GSGD2.Utilities;
	using UnityEngine;

	/// <summary>
	/// Component that can be understand as a "Service Locator" pattern, it hold all player component's hard references and can act as a proxy to give them to other components.
	/// </summary>
	public class PlayerReferences : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rigidbody = null;
		[SerializeField] private PlayerController _playerController = null;
		[SerializeField] private CharacterCollision _characterCollision = null;
		[SerializeField] private CubeController _cubeController = null;
		[SerializeField] private CubeAnimator _cubeAnimator = null;
		[SerializeField] private Animator _animator = null;
		[SerializeField] private MovingPlatformInteractorActivator _movingPlatformInteractorActivator = null;
		[SerializeField] private InteractorActivator _interactorActivator = null;
		[SerializeField] private BumperReceiver _bumperReceiver = null;
		[SerializeField] private ItemHolder _itemHolder = null;
		[SerializeField] private PlayerJumpCameraShaker _playerJumpShaker = null;
		[SerializeField] private PlayerMovingPlatformStickToGroundReceiver _playerMovingPlatformStickToGroundReceiver = null;
		[SerializeField] private DisplacementEstimationUpdater _displacementEstimationUpdater = null;
		[SerializeField] private PlayerKillzoneReceiver _playerKillzoneReceiver = null;
		[SerializeField] private PlayerDamageable _playerDamageable = null;
		[SerializeField] private PlayerDamageFeedbackHandler _playerDamageFeedbackHandler = null;
		[SerializeField] private CameraAimController _cameraAimController = null;

		public bool TryGetRigidbody(out Rigidbody rigidbody)
		{
			rigidbody = _rigidbody;
			return rigidbody != null;
		}
		
		public bool TryGetPlayerController(out PlayerController playerController)
		{
			playerController = _playerController;
			return playerController != null;
		}

		public bool TryGetCharacterCollision(out CharacterCollision characterCollision)
		{
			characterCollision = _characterCollision;
			return characterCollision != null;
		}

		public bool TryGetCubeController(out CubeController cubeController)
		{
			cubeController = _cubeController;
			return cubeController != null;
		}

		public bool TryGetCubeAnimator(out CubeAnimator cubeAnimator)
		{
			cubeAnimator = _cubeAnimator;
			return cubeAnimator != null;
		}

		public bool TryGetAnimator(out Animator animator)
		{
			animator = _animator;
			return animator != null;
		}

		public bool TryGetMovingPlatformInteractorActivator(out MovingPlatformInteractorActivator movingPlatformInteractorActivator)
		{
			movingPlatformInteractorActivator = _movingPlatformInteractorActivator;
			return movingPlatformInteractorActivator != null;
		}

		public bool TryGetInteractorActivator(out InteractorActivator interactorActivator)
		{
			interactorActivator = _interactorActivator;
			return interactorActivator != null;
		}

		public bool TryGetBumperReceiver(out BumperReceiver bumperReceiver)
		{
			bumperReceiver = _bumperReceiver;
			return bumperReceiver != null;
		}

		public bool TryGetItemHolder(out ItemHolder itemHolder)
		{
			itemHolder = _itemHolder;
			return itemHolder != null;
		}

		public bool TryGetPlayerJumpShaker(out PlayerJumpCameraShaker playerJumpShaker)
		{
			playerJumpShaker = _playerJumpShaker;
			return playerJumpShaker != null;
		}

		public bool TryGetPlayerMovingPlatformStickToGroundReceiver(out PlayerMovingPlatformStickToGroundReceiver playerMovingPlatformStickToGroundReceiver)
		{
			playerMovingPlatformStickToGroundReceiver = _playerMovingPlatformStickToGroundReceiver;
			return playerMovingPlatformStickToGroundReceiver != null;
		}

		public bool TryGetDisplacementEstimationUpdater(out DisplacementEstimationUpdater displacementEstimationUpdater)
		{
			displacementEstimationUpdater = _displacementEstimationUpdater;
			return displacementEstimationUpdater != null;
		}

		public bool TryGetPlayerKillzoneReceiver(out PlayerKillzoneReceiver playerKillzoneReceiver)
		{
			playerKillzoneReceiver = _playerKillzoneReceiver;
			return playerKillzoneReceiver != null;
		}

		public bool TryGetPlayerDamageable(out PlayerDamageable playerDamageable)
		{
			playerDamageable = _playerDamageable;
			return playerDamageable != null;
		}

		public bool TryGetPlayerDamageFeedbackHandler(out PlayerDamageFeedbackHandler playerDamageFeedbackHandler)
		{
			playerDamageFeedbackHandler = _playerDamageFeedbackHandler;
			return playerDamageFeedbackHandler != null;
		}

		public bool TryGetCameraAimController(out CameraAimController cameraAimController)
		{
			cameraAimController = _cameraAimController;
			return cameraAimController != null;
		}
	}
}