namespace GSGD2.Gameplay
{
	using GSGD2.Player;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Damageable specialized for player. It's used by <see cref="DamageDealer"/> to filter player damageable from other. It react to the death of player, restore its health and reset it.
	/// </summary>
	[RequireComponent(typeof(CubeController))]
	public class PlayerDamageable : Damageable
	{
		[SerializeField]
		private PlayerReferences _playerReferences = null;

		[SerializeField]
		private bool _resetPositionWhenHealthBelowZero = false;

		[SerializeField]
		private bool _disableTakeDamageDuringDash = false;

		private CubeController _cubeController = null;

		protected override void Awake()
		{
			base.Awake();
			_playerReferences.TryGetDisplacementEstimationUpdater(out displacementEstimationUpdater);
			_playerReferences.TryGetCubeController(out _cubeController);
			_playerReferences.TryGetRigidbody(out rigidbody);
		}

		protected override bool CanTakeDamage(Damage damage)
		{
			bool result = base.CanTakeDamage(damage);
			result &= (_cubeController.CurrentState == CubeController.State.DamageTaken) == false; // do not take damage if we are in take damage state
			result &= (_disableTakeDamageDuringDash == true && _cubeController.CurrentState == CubeController.State.Dashing) == false;
			// TODO AL : redo condition before, not good for dash because of the unsynced physics and event.
			// When the rigidbody is reset by the end of the dash, it can take an update frame or two to be applied, so we can be hit by spikes and see our character collider not touching the spikes at the end of a dash.
			// lazy solution : add a recovery timer after the dash to avoid "frame sliced" hits.

			//Debug.LogFormat("{0}.CanTakeDamage() current state == {1}", GetType().Name, _cubeController.CurrentState);

			return result;
		}

		protected override void OnTakeDamage(Damage damage)
		{
			base.OnTakeDamage(damage);
			_cubeController.ChangeState(CubeController.State.DamageTaken);
		}

		protected override void OnTakeDamageAndHealthBelowZero(Damage damage)
		{
			base.OnTakeDamageAndHealthBelowZero(damage);
			if (_resetPositionWhenHealthBelowZero == true)
			{
				RestoreHealth(healthAtStart);
				LevelReferences.Instance.PlayerStart.ResetPlayerPosition();
			}
		}

	}
}