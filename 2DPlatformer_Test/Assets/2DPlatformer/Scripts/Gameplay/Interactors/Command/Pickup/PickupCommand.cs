namespace GSGD2.Gameplay
{
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Command specialized for Pickups. It should be used exclusively by <see cref="PickupInteractor"/> and permit to generelize pickups behavior (e.g. destroying it when picking it, but it can be add an effect or tell something to the HUD).
	/// It is abstract and can be used by overriding it (<see cref="HealthPickupCommand"/>.
	/// </summary>
	public abstract class PickupCommand : ACommand
	{
		[SerializeField]
		private bool _destroyPickupOnApply = false;

		// We prevent Pickup child class to override Apply in order to ensure it can't remove the destroy effect
		public sealed override void Apply(ICommandSender from)
		{
			bool result = ApplyPickup(from);

			if (result == true && _destroyPickupOnApply == true)
			{
				Destroy(from.GetGameObject());
			}
		}

		/// <summary>
		/// Method to override for a PickupCommand child class to specialize the pickup behavior
		/// </summary>
		/// <param name="from"></param>
		protected abstract bool ApplyPickup(ICommandSender from);
	}
}