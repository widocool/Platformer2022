namespace GSGD2.Gameplay
{
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Item version of <see cref="AKillZoneReceiver"/>
	/// </summary>
	public class ItemKillzoneReceiver : AKillZoneReceiver
	{
		[SerializeField]
		private Item _item = null;

		public override void OnEnterKillzone(Killzone killzone)
		{
			_item.ResetWorldPosition();
		}
	}
}