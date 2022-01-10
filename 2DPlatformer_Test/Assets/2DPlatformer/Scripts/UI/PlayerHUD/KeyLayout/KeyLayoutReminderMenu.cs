namespace GSGD2.UI
{
	using System.Collections;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Class representing a single key item, an image and its description. It can be overriden by item in runtime (<see cref="GSGD2.Gameplay.ProjectileLauncher"/>.
	/// </summary>
	public class KeyLayoutReminderMenu : AMenu
	{
		[SerializeField]
		private Image _icon = null;

		[SerializeField]
		private TextMeshProUGUI _descriptionText = null;

		private KeyLayoutDescription _keyLayoutDescription = null;

		private bool _isOverrided = false;
		private bool _wasActiveWhenOverrided = false;
		public bool IsOverrided => _isOverrided;

		public void SetLayout(KeyLayoutDescription description)
		{
			_icon.sprite = description.Sprite;
			_descriptionText.text = description.Description;
			_keyLayoutDescription = description;
		}

		public void OverrideLayout(KeyLayoutDescription description)
		{
			OverrideLayout(description.Sprite, description.Description);
		}

		public void OverrideLayout(Sprite icon, string description)
		{
			_isOverrided = true;
			_icon.sprite = icon;
			_descriptionText.text = description;
			_wasActiveWhenOverrided = gameObject.activeSelf;
			SetActive(true);
		}

		public void ResetOverride()
		{
			if (_isOverrided == true && _keyLayoutDescription != null)
			{
				SetLayout(_keyLayoutDescription);
				_isOverrided = false;
				SetActive(_wasActiveWhenOverrided);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			showAtStart = false;
		}
	}
}