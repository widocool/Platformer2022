namespace GSGD2.Player
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Utilities;

	/// <summary>
	/// Component class that permit to handle camera shake event based on the player state.  Disabling the component disable the states listening.
	/// Put on the player GameObject, it listen to <see cref="CubeController"/> bumping state and its velocity when falling to shake the camera accordingly.
	/// No custom shake settings for now.
	/// </summary>
	[RequireComponent(typeof(CubeController))]
	[RequireComponent(typeof(DisplacementEstimationUpdater))]
	public class PlayerJumpCameraShaker : MonoBehaviour
	{
		[SerializeField]
		private PlayerReferences _playerReferences = null;
	
		[SerializeField]
		private float _speedThreshold = 1f;

		[SerializeField]
		private float _maxSpeedThreshold = 30f; // roughly estimated max speed at 30, so we never reach 100

		[SerializeField]
		private AnimationCurve _speedToShakeForceCurve = null;

		[SerializeField]
		private float _bumpForce = 1f;

		private CubeController _cubeController = null;
		private DisplacementEstimationUpdater _displacementEstimationUpdater = null;
		private CameraEventManager _cameraEventManager = null;

		private void Awake()
		{
			_playerReferences.TryGetCubeController(out _cubeController);
			_playerReferences.TryGetDisplacementEstimationUpdater(out _displacementEstimationUpdater);
		}

		private void OnEnable()
		{
			_cameraEventManager = LevelReferences.Instance.CameraEventManager;
			_cubeController.StateChanged -= CubeControllerOnStateChanged;
			_cubeController.StateChanged += CubeControllerOnStateChanged;
		}

		private void OnDisable()
		{
			_cubeController.StateChanged -= CubeControllerOnStateChanged;
		}

		private void CubeControllerOnStateChanged(CubeController cubeController, CubeController.CubeControllerEventArgs args)
		{
			float speed = _displacementEstimationUpdater.AverageSpeed;
			CubeController.State state = args.currentState;

			if (state == CubeController.State.EndJump || state == CubeController.State.Grounded)
			{
				if (speed > _speedThreshold)
				{
					float normalizedSpeed = Mathf.Clamp01(speed / _maxSpeedThreshold);
					var result = _speedToShakeForceCurve.Evaluate(normalizedSpeed);
					//Debug.LogFormat("{0} | n : {1} | r : {2}", speed, normalizedSpeed, result);
					_cameraEventManager.Shake(result);
				}
			}

			if (state == CubeController.State.Bumping)
			{
				_cameraEventManager.Shake(_bumpForce);
			}
		}
	}

}