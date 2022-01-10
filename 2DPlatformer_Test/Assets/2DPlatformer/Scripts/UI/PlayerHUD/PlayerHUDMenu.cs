namespace GSGD2.UI
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Menu that group all the HUD subsystem (layout reminder, healthbar, etc...) and can show or hide all at once, or be just a proxy to pass subsystem to other object
	/// </summary>
	public class PlayerHUDMenu : AMenu
	{
		[SerializeField]
		private LayoutReminderMenu _layoutReminder = null;

		[SerializeField]
		private HealthBarHUDMenu _healthBarHUDMenu = null;

		public LayoutReminderMenu LayoutReminder => _layoutReminder;
		public HealthBarHUDMenu HealthBarHUDMenu => _healthBarHUDMenu;
	}
}