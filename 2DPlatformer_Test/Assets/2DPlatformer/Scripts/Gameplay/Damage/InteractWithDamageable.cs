namespace GSGD2.Gameplay
{
	/// <summary>
	/// Bitwise enum (multiple selectable values) that can be used to customize DamageDealer policies (which DamageDealer can interact with which Damageable). See <see cref="InteractWithDamageableExtension"/> for more infos.
	/// </summary>
	// With vanilla Unity we can't display a Type and its child classes.
	// So we needs to keep this enum up to date
	// Do not forget to add it to GetTypes too
	[System.Flags]
	public enum InteractWithDamageable : int
	{
		Nothing = 0,
		PlayerDamageable = 1 << 0,
		EnemyDamageable = 1 << 1,
		PropDamageable = 1 << 2,
		Everything = ~(Nothing),
	}
}