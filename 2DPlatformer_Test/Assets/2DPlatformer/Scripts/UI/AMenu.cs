namespace GSGD2.UI
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Base class for UI "menu", a.k.a a a Facade for UI subsystem. Menu can be shown, hidden, a default show/hide behavior and has events to listen to.
	/// </summary>
	public abstract class AMenu : MonoBehaviour
	{
		[SerializeField]
		protected bool showAtStart = true;

		private bool _isAlreadyManuallyActivated = false;

		public delegate void MenuEvent(AMenu sender);
		public event MenuEvent MenuShown = null;
		public event MenuEvent MenuHidden = null;

		public virtual void SetActive(bool isActive)
		{
			_isAlreadyManuallyActivated = true;

			gameObject.SetActive(isActive);

			if (isActive)
			{
				MenuShown?.Invoke(this);
			}
			else
			{
				MenuHidden?.Invoke(this);
			}
		}

		protected virtual void Awake() { }
		protected virtual void Start()
		{
			if (_isAlreadyManuallyActivated == false)
			{
				SetActive(showAtStart);
			}
		}
		protected virtual void OnEnable()
		{
		}
	}
}