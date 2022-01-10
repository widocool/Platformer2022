namespace GSGD2.Gameplay
{
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Class specialized for <see cref="Item"/> that can be sticked on moving platform ground. It prevent to set the item on a platform if it's held, and remove it from a platform in case of it enter in a killzone.
	/// </summary>
	public class ItemMovingPlatformStickToGroundReceiver : MovingPlatformStickToGroundReceiver
	{
		[SerializeField]
		private Item _item = null;

		protected override bool CanSetOnPlatform()
		{
			bool result = base.CanSetOnPlatform();
			result &= _item.CurrentState != Item.State.Held;
			return result;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			_item.WorldPositionReset -= ItemOnWorldPositionReset;
			_item.WorldPositionReset += ItemOnWorldPositionReset;
		}


		protected override void OnDisable()
		{
			base.OnDisable();
			_item.WorldPositionReset -= ItemOnWorldPositionReset;
		}

		private void ItemOnWorldPositionReset(Item sender, Item.ItemEventArgs args)
		{
			if (movingPlatformStickToGround != null)
			{
				movingPlatformStickToGround.RemoveOnPlatform(this, true);
			}
			ClearFromPlatform();
		}
	}
}