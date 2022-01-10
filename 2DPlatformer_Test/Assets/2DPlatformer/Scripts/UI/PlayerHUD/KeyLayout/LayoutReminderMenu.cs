namespace GSGD2.UI
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Key = ControllerDescription.Key;

	/// <summary>
	/// "Brain" class of KeyLayout hud. It instantiate given <see cref="ControllerDescription"/> into <see cref="KeyLayoutReminderMenu"/>, update them, and give access to them to show / hide / overriden them.
	/// </summary>
	public class LayoutReminderMenu : AMenu
	{
		[Header("Controllers references")]
		[SerializeField]
		private ControllerDescription _gamepadController = null;

		[SerializeField]
		private ControllerDescription _keyboardController = null;

		[Header("UI references")]
		[SerializeField]
		private KeyLayoutReminderMenu _keyLayoutMenuPrefab = null;

		[Header("Settings")]
		[SerializeField]
		private bool _useGamepad = true;

		[SerializeField]
		private bool _setActiveAllLayout = false;

		private Dictionary<Key, KeyLayoutReminderMenu> _keyLayoutReminderMenus = new Dictionary<Key, KeyLayoutReminderMenu>();
		private bool _isInitialized = false;

		public bool IsInitialized => _isInitialized;

		private delegate void LayoutReminderMenuEvent(LayoutReminderMenu sender);
		private event LayoutReminderMenuEvent Initialized = null;

		public KeyLayoutDescription _testDesc;

		public void InstantiateLayouts()
		{
			int count = (int)Key.Count;
			for (int i = 0; i < count; i++)
			{
				var key = (Key)i;
				KeyLayoutDescription layoutDescription = GetLayoutDescription(key);

				if (layoutDescription != null)
				{
					KeyLayoutReminderMenu instance = Instantiate(_keyLayoutMenuPrefab, transform);
					_keyLayoutReminderMenus.Add(key, instance);
					layoutDescription.UpdateMenu(_keyLayoutReminderMenus[key]);
				}
				else
				{
					Debug.LogErrorFormat("{0}.InstantiateLayout() Cannot instantiate key {1} because the description is null.", GetType().Name, key);
				}
			}
		}

		[ContextMenu("Update Layouts")]
		public void UpdateLayouts()
		{
			int count = (int)Key.Count;
			for (int i = 0; i < count; i++)
			{
				var key = (Key)i;
				KeyLayoutDescription layoutDescription = GetLayoutDescription(key);
				if (layoutDescription != null)
				{
					layoutDescription.UpdateMenu(_keyLayoutReminderMenus[key]);
				}
			}
		}

		public bool OverrideLayout(Key key, KeyLayoutDescription overridingLayout)
		{
			if (overridingLayout != null && _keyLayoutReminderMenus.TryGetValue(key, out KeyLayoutReminderMenu menu) == true)
			{
				menu.OverrideLayout(overridingLayout);
				return true;
			}
			return false;
		}


		public bool ResetOverrideLayout(Key key)
		{
			if (_keyLayoutReminderMenus.TryGetValue(key, out KeyLayoutReminderMenu menu) == true)
			{
				menu.ResetOverride();
			}
			return false;
		}

		public void SetActiveLayoutMenu(Key key, bool isActive)
		{
			if (_keyLayoutReminderMenus.TryGetValue(key, out KeyLayoutReminderMenu menu) == true)
			{
				menu.SetActive(isActive);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			InstantiateLayouts();
			if (_setActiveAllLayout == true)
			{
				SetActiveAllLayouts(true);
			}
			Initialized?.Invoke(this);
			_isInitialized = true;
		}

		private KeyLayoutDescription GetLayoutDescription(Key key)
		{
			return _useGamepad == true ? _gamepadController.GetLayoutDescription(key) : _keyboardController.GetLayoutDescription(key);
		}

		private void SetActiveAllLayouts(bool isActive)
		{
			var values = _keyLayoutReminderMenus.Values;
			foreach (var item in values)
			{
				item.SetActive(isActive);
			}
		}
	}
}