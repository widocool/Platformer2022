namespace GSGD2.Gameplay
{
	using GSGD2.Utilities;
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Generic class that can instantiate projectile at a given fire rate. It can be used by an item (<see cref="ProjectileLauncherController"/> or a world entity <see cref="ProjectileBox"/>.
	/// </summary>
	public class ProjectileLauncher : MonoBehaviour, IDamageInstigator
	{
		[SerializeField]
		private AProjectile _projectilePrefab = null;

		[SerializeField]
		private Transform _projectileInstanceOffset = null;

		[SerializeField]
		private Timer _projectileFireRate = null;

		/// <summary>
		/// Will set whether or not projectile should be destroyed by a given collider type
		/// </summary>
		[SerializeField]
		private InteractWithDamageable _projectileInteractWith = 0f;

		/// <summary>
		/// Will set whether or not projectile should apply damage to a given damageable type
		/// </summary>
		[SerializeField]
		private InteractWithDamageable _damageDealerInteractWith = 0f;

		public Timer ProjectileFireRate => _projectileFireRate;
		public Transform ProjectileInstanceOffset => _projectileInstanceOffset;
		public void StartTimer() => _projectileFireRate.Start();
		public bool UpdateTimer() => _projectileFireRate.Update();

		public bool CanUse()
		{
			if (_projectileFireRate.CurrentState == Timer.State.Running)
			{
				return false;
			}
			return true;
		}

		public void LaunchProjectile()
		{
			if (CanUse() == true)
			{
				AProjectile instance = Instantiate(_projectilePrefab, _projectileInstanceOffset.transform.position, _projectileInstanceOffset.transform.rotation);
				_projectileFireRate.Start();

				instance.SetInteractWith(_projectileInteractWith);
				if (instance.TryGetComponent(out DamageDealer damageDealer) == true)
				{
					damageDealer.SetInstigator(this);
					damageDealer.SetInteractWith(_damageDealerInteractWith);
				}
			}
		}

		Transform IDamageInstigator.GetTransform() => transform;
	}
}