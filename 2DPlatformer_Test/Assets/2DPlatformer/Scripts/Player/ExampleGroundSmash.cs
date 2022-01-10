namespace GSGD2.Player
{
	using GSGD2.Utilities;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// This class is an example of how you can add movement behavior to the player without digging into <see cref="CubeController"/>.
	/// It can enable a mario like "ground smash" 
	/// </summary>
	public class ExampleGroundSmash : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody _rigidbody = null;

		[SerializeField]
		private CubeController _cubeController = null;

		[SerializeField]
		private PlayerController _playerController = null;

		[SerializeField]
		private float _force = 50f;

		[SerializeField]
		private Timer _enableControlsAfterTimer = null;

		[SerializeField]
		private CubeController.State _usableInState = CubeController.State.None;

		private bool _isOnGroundSmash = false;

		private void OnEnable()
		{
			_playerController.GroundSmashPerformed -= PlayerControllerOnGroundSmashPerformed;
			_playerController.GroundSmashPerformed += PlayerControllerOnGroundSmashPerformed;
		}

		private void OnDisable()
		{
			_playerController.GroundSmashPerformed -= PlayerControllerOnGroundSmashPerformed;
		}

		private void PlayerControllerOnGroundSmashPerformed(PlayerController sender, UnityEngine.InputSystem.InputAction.CallbackContext obj)
		{
			if (_usableInState.HasFlag(_cubeController.CurrentState) && _isOnGroundSmash == false)
			{
				_isOnGroundSmash = true;
				// TODO AL : maybe reset vel to 0 before applying the bump
				_rigidbody.AddForce(new Vector3(0f, _force * -1, 0f), ForceMode.Impulse);
				_cubeController.enabled = false;
				_enableControlsAfterTimer.Start();
			}
		}

		private void Update()
		{
			if (_isOnGroundSmash == true)
			{
				_cubeController.ForceCheckGround();
				if (_cubeController.CurrentState != CubeController.State.Jumping && _enableControlsAfterTimer.Update() == true)
				{
					_cubeController.enabled = true;
					_isOnGroundSmash = false;
				}
			}
		}

		private void OnValidate()
		{
			_force = Mathf.Clamp(_force, 10f, float.MaxValue);
		}
	}

}