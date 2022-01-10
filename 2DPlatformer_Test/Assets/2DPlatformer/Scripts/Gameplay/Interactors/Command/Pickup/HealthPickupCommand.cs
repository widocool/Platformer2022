namespace GSGD2.Gameplay
{
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// PickupCommand used to modify player health. It can be workarounded to poison the player, but it shouldn't be used that way, since it will not call TakeDamage() and trigger the chain of events.
	/// </summary>
	[CreateAssetMenu(menuName = "GameSup/HealthPickupCommand", fileName = "HealthPickupCommand")]
	public class HealthPickupCommand : PickupCommand
	{
		[SerializeField]
		private int _healthToChange = 1;

		protected override bool ApplyPickup(ICommandSender from)
		{
			if (LevelReferences.Instance.PlayerReferences.TryGetPlayerDamageable(out PlayerDamageable playerDamageable) == true)
			{
				if (_healthToChange > 0)
				{
					return playerDamageable.RestoreHealth(_healthToChange);
				}
				else if (_healthToChange < 0)
				{
					return playerDamageable.TakeDamage(new Damage(-1 * _healthToChange, from as IDamageInstigator));
				}
			}
			return false;
		}
	}
}