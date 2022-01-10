namespace GSGD2.UI
{
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Encapsulation of a controls schemes for a specific controller (gamepad or keyboard). ControllerDescription can be switch in runtime <see cref="LayoutReminderMenu"/>
	/// </summary>
	[CreateAssetMenu(menuName = "GameSup/ControllerDescription", fileName = "ControllerDescription")]
	public class ControllerDescription : ScriptableObject
	{
		public enum Key
		{
			HorizontalMove = 0,
			VerticalMove,  // TODO AL add input
			VerticalLook,
			HorizontalLook,
			Jump,
			Dash,
			WallGrab,
			WallJump,
			TakeItem,
			ReleaseItem,
			UseItem,

			Count // Keep it as last element
		}

		[SerializeField]
		private KeyLayoutDescription _horizontalMove = null;

		[SerializeField]
		private KeyLayoutDescription _verticalMove = null;

		[SerializeField]
		private KeyLayoutDescription _verticalLook = null;

		[SerializeField]
		private KeyLayoutDescription _horizontalLook = null;

		[SerializeField]
		private KeyLayoutDescription _jump = null;

		[SerializeField]
		private KeyLayoutDescription _dash = null;

		[SerializeField]
		private KeyLayoutDescription _wallGrab = null;

		[SerializeField]
		private KeyLayoutDescription _wallJump = null;

		[SerializeField]
		private KeyLayoutDescription _takeItem = null;

		[SerializeField]
		private KeyLayoutDescription _releaseItem = null;

		[SerializeField]
		private KeyLayoutDescription _useItem = null;

		public KeyLayoutDescription GetLayoutDescription(Key key)
		{
			switch (key)
			{
				case Key.HorizontalMove: return _horizontalMove;
				case Key.VerticalMove: return _verticalMove;
				case Key.VerticalLook: return _verticalLook;
				case Key.HorizontalLook: return _horizontalLook;
				case Key.Jump: return _jump;
				case Key.Dash: return _dash;
				case Key.WallGrab: return _wallGrab;
				case Key.WallJump: return _wallJump;
				case Key.TakeItem: return _takeItem;
				case Key.ReleaseItem: return _releaseItem;
				case Key.UseItem: return _useItem;
				default: return null;
			}
		}
	}
}