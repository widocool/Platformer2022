namespace GSGD2.Gameplay
{
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Player version of <see cref="AKillZoneReceiver"/>
	/// </summary>
	public class PlayerKillzoneReceiver : AKillZoneReceiver
	{
		public override void OnEnterKillzone(Killzone killzone)
		{
			LevelReferences.Instance.PlayerStart.ResetPlayerPosition();
		}
	}
}