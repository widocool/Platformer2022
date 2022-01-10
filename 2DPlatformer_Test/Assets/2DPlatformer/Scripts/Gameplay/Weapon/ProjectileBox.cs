namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Very simple class that launch a projectile as soon as the <see cref="ProjectileLauncher"/> fire rate permit it.
	/// </summary>
	[SelectionBase]
	public class ProjectileBox : MonoBehaviour
	{
		[SerializeField]
		private ProjectileLauncher _projectileLauncher = null;

		private void OnEnable()
		{
			_projectileLauncher.ProjectileFireRate.Start();
		}

		private void Update()
		{
			if (_projectileLauncher.UpdateTimer() == true)
			{
				_projectileLauncher.LaunchProjectile();
			}
		}
	}
}