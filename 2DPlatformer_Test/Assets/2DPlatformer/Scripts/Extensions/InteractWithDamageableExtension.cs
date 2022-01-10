namespace GSGD2.Gameplay
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Helper class for Damageable interaction policy. See <see cref="InteractWithDamageable"/>. Damageable specialized version of <see cref="TypeListExtension.CanInteractWithType(List{Type}, Transform)"/>.
	/// </summary>
	public static class InteractWithDamageableExtension
	{
		public static bool CanInteractWith(this InteractWithDamageable flags, Transform other) 
		{
			return flags.GetInteractionTypes().CanInteractWithType(other);
		}

		public static bool CanInteractWith<T>(this InteractWithDamageable flags, Transform other, out T concreteOther) where T : MonoBehaviour
		{
			return flags.GetInteractionTypes().CanInteractWithType(other, out concreteOther);
		}

		// Add new types of interaction here
		public static List<Type> GetInteractionTypes(this InteractWithDamageable flags)
		{
			List<Type> types = new List<Type>();
			if (flags.HasFlag(InteractWithDamageable.Everything) == true)
			{
				types.Add(typeof(Damageable));
			}
			if (flags.HasFlag(InteractWithDamageable.PlayerDamageable) == true)
			{
				types.Add(typeof(PlayerDamageable));
			}
			if (flags.HasFlag(InteractWithDamageable.EnemyDamageable) == true)
			{
				types.Add(typeof(EnemyDamageable));
			}
			if (flags.HasFlag(InteractWithDamageable.PropDamageable) == true)
			{
				types.Add(typeof(PropDamageable));
			}
			return types;
		}
	}
}