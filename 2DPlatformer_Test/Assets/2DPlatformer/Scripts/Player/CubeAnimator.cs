namespace GSGD2.Player
{
	using GSGD2.Utilities;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Handle basic animation based on <see cref="CubeController"/> states and velocity. Disabling the component disable the Animator component as well.
	/// </summary>
	[RequireComponent(typeof(CubeController))]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(DisplacementEstimationUpdater))]
	public class CubeAnimator : MonoBehaviour
	{
		private const string WALK_DIRECTION_PARAMETER_NAME = "WalkDirection";
		private const string START_JUMP_RIGHT_PARAMETER_NAME = "StartJumpRight";
		private const string START_JUMP_LEFT_TPARAMETER_NAME = "StartJumpLeft";
		private const string END_JUMP_SOFT_PARAMETER_NAME = "EndJumpSoft";
		private const string END_JUMP_HARD_PARAMETER_NAME = "EndJumpHard";
		private const string BUMP_PARAMETER_NAME = "Bump";

		[SerializeField]
		private PlayerReferences _playerReferences = null;

		[SerializeField]
		private float _endJumpDownwardSpeedThresholdWhenGrounded = 5f;

		[SerializeField]
		private float _hardJumpThreshold = 10f;

		private CubeController _cubeController = null;
		private Animator _animator = null;
		private Rigidbody _rigidbody = null; // TODO AL : can be removed and its velocity replaced by _displacementEstimationUpdater
		private DisplacementEstimationUpdater _displacementEstimationUpdater = null;

		private void Awake()
		{
			_playerReferences.TryGetCubeController(out _cubeController);
			_playerReferences.TryGetAnimator(out _animator);
			_playerReferences.TryGetRigidbody(out _rigidbody);
			_playerReferences.TryGetDisplacementEstimationUpdater(out _displacementEstimationUpdater);
		}

		private void OnEnable()
		{
			_cubeController.StateChanged -= OnCubeControllerStateChanged;
			_cubeController.StateChanged += OnCubeControllerStateChanged;
			_animator.enabled = true;
		}

		private void OnDisable()
		{
			_cubeController.StateChanged -= OnCubeControllerStateChanged;
			_animator.enabled = false;
		}

		private void OnCubeControllerStateChanged(CubeController cubeController, CubeController.CubeControllerEventArgs args)
		{
			switch (args.currentState)
			{
				case CubeController.State.Grounded:
				{
					var downwardVelocityBelowThreshold = Vector3.Dot(_displacementEstimationUpdater.Velocity, -transform.up) > _endJumpDownwardSpeedThresholdWhenGrounded;
					if (downwardVelocityBelowThreshold == true)
					{
						PlayJump();
					}
				}
				break;
				case CubeController.State.Falling:
					break;
				case CubeController.State.Bumping:
				{
					_animator.SetTrigger(BUMP_PARAMETER_NAME);
				}
				break;
				case CubeController.State.StartJump:
				case CubeController.State.Jumping:
				{
					float dotProduct = Vector3.Dot(_rigidbody.velocity, Vector3.forward);
					// right
					if (dotProduct >= 0)
					{
						_animator.SetTrigger(START_JUMP_RIGHT_PARAMETER_NAME);
					}
					else
					{
						_animator.SetTrigger(START_JUMP_LEFT_TPARAMETER_NAME);
					}
				}
				break;
				case CubeController.State.EndJump:
				{
					PlayJump();
				}
				break;
				case CubeController.State.WallGrab:
				case CubeController.State.WallJump:
				case CubeController.State.Dashing:
				default:
					break;
			}
		}

		private void PlayJump()
		{
			_animator.SetTrigger(_displacementEstimationUpdater.AverageSpeed > _hardJumpThreshold ? END_JUMP_HARD_PARAMETER_NAME : END_JUMP_SOFT_PARAMETER_NAME);
		}


		private void Update()
		{
			_animator.SetFloat(WALK_DIRECTION_PARAMETER_NAME, Mathf.Abs(_rigidbody.velocity.z));
		}
	}

}