namespace GSGD2.Gameplay
{
	using UnityEngine;
	using GSGD2.Utilities;

	/// <summary>
	/// Encapsulation of Damage that can be given along time. It can be setup on Damageable, DamageDealer or other kind of Damager, but for now it is a receiver on Damageable and apply given Damage on time on itself.
	/// </summary>
	[System.Serializable]
	public class DamageOnTime
	{
		[SerializeField]
		private bool _receiveDOT = false;

		private Damage _damage = null;
		private Timer _periodicDamageTimer = new Timer();
		private bool _isRunning = false;

		public bool IsReceiveDOT => _receiveDOT;
		public bool IsRunning => _isRunning;

		public void StartDamageOnTime(float rate, Damage damage)
		{
			if (_receiveDOT == false) return;
			if (_isRunning == true) return;
			//_periodicDamageTimer.ForceFinishState();
			_periodicDamageTimer.Start(rate);
			_damage = damage;
			_isRunning = true;
		}

		public void StopDamageOnTime()
		{
			if (_receiveDOT == false) return;
			_periodicDamageTimer.ForceFinishState();
			_isRunning = false;
		}

		public void UpdateDamageOnTimer(Damageable damageable)
		{
			if (_receiveDOT == false) return;
			if (_isRunning == true && _periodicDamageTimer.Update() == true)
			{
				// We do want to bump infinitely
				_damage.Bump.ResetCurrentForceCount();

				// If no damage was given, let the timer to State.Finish and trying to give a damage the next frame, it ensure Dot damage is given as soon as recovery time is over.
				bool hasGivenDamage = damageable.TakeDamage(_damage);
				if (hasGivenDamage == true)
				{
					_periodicDamageTimer.Start();
				}
			}
		}
	}
}