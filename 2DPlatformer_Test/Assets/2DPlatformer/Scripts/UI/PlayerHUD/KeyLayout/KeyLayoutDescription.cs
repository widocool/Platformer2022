namespace GSGD2.UI
{
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Encapsulation of a sprite and a description representing a control key in the <see cref="KeyLayoutReminderMenu"/>. It can be used for default keys and description, with different version for gamepads and keyboard, but also for overriding item. <see cref="GSGD2.Gameplay.ProjectileLauncher"/> for more details.
	/// </summary>
	[CreateAssetMenu(fileName = "KeyLayoutDescription", menuName = "GameSup/KeyLayoutDescription")]
	public class KeyLayoutDescription : ScriptableObject
	{
		[SerializeField]
		private Sprite _sprite = null;

		[SerializeField]
		private string _description = string.Empty;

		public Sprite Sprite => _sprite;

		/// <summary>
		/// Text shown by <see cref="KeyLayoutReminderMenu"/> on the player HUD. If no description is given, it will take the text after the last occurence of '.' (e.g. KeyLayoutDescription.Gamepad.UseItem become UseItem).
		/// </summary>
		public string Description
		{
			get
			{
				if (string.IsNullOrEmpty(_description))
				{
					string description = this.name;
					return description.Substring(description.LastIndexOf('.') + 1);
				}
				else
				{
					return _description;
				}
			}
		}

		public void UpdateMenu(KeyLayoutReminderMenu menu)
		{
			menu.SetLayout(this);
		}
	}
}