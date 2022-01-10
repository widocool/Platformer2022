namespace GSGD2.UI
{
	using GSGD2;
	using GSGD2.Gameplay;
	using GSGD2.Player;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Key = ControllerDescription.Key;

	/// <summary>
	/// Quick class that controls permanent KeyLayout displayed on HUD (<see cref="_movementControlsKeys"/>).
	/// </summary>
	public class LayoutReminderUpdater : MonoBehaviour
	{
		[SerializeField]
		private LayoutReminderMenu _menu = null;

		[SerializeField]
		private Key[] _movementControlsKeys = null;

		private CubeController _player = null;
		private ItemHolder _playerItemHolder = null;

		private void Awake()
		{
			_player = LevelReferences.Instance.Player;
			_playerItemHolder = _player.GetComponent<ItemHolder>();
		}

		private void OnEnable()
		{
			_player.StateChanged -= PlayerOnStateChanged;
			_player.StateChanged += PlayerOnStateChanged;

			if (_playerItemHolder != null)
			{
				_playerItemHolder.StateChangedTransitionOut -= PlayerItemHolderOnStateChangedTransitionOut;
				_playerItemHolder.StateChangedTransitionOut += PlayerItemHolderOnStateChangedTransitionOut;
				_playerItemHolder.StateChangedTransitionIn -= PlayerItemHolderOnStateChangedTransitionIn;
				_playerItemHolder.StateChangedTransitionIn += PlayerItemHolderOnStateChangedTransitionIn;
			}

			if (_menu.IsInitialized == true)
			{
				SetActiveMovementControls(true);
			}
			else
			{
				StartCoroutine(WaitForInit());
			}
		}


		private void OnDisable()
		{
			_player.StateChanged -= PlayerOnStateChanged;
			_playerItemHolder.StateChangedTransitionOut -= PlayerItemHolderOnStateChangedTransitionOut;
			_playerItemHolder.StateChangedTransitionIn -= PlayerItemHolderOnStateChangedTransitionIn;

			SetActiveMovementControls(false);
		}

		private IEnumerator WaitForInit()
		{
			while (_menu.IsInitialized == false)
			{
				yield return null;
			}
			SetActiveMovementControls(true);
		}


		private void SetActiveMovementControls(bool isActive)
		{
			for (int i = 0, length = _movementControlsKeys.Length; i < length; i++)
			{
				Key key = _movementControlsKeys[i];
				_menu.SetActiveLayoutMenu(key, isActive);
			}
		}

		#region Callbacks

		private void PlayerOnStateChanged(CubeController cubeController, CubeController.CubeControllerEventArgs args)
		{
			// Transition Out
			switch (args.previousState)
			{
				case CubeController.State.Grounded:
					break;
				case CubeController.State.Falling:
					break;
				case CubeController.State.Bumping:
					break;
				case CubeController.State.StartJump:
					break;
				case CubeController.State.Jumping:
					break;
				case CubeController.State.EndJump:
					break;
				case CubeController.State.WallGrab:
				{
					// Show hide grab
					_menu.SetActiveLayoutMenu(Key.WallGrab, true);
					// Hide wall jump
					_menu.SetActiveLayoutMenu(Key.WallJump, false);
				}
				break;
				case CubeController.State.WallJump:
				{
				}
				break;
				case CubeController.State.Dashing:
					break;
				case CubeController.State.DamageTaken:
					break;
				default:
					break;
			}

			// Transition In
			switch (args.currentState)
			{
				case CubeController.State.Grounded:
					break;
				case CubeController.State.Falling:
					break;
				case CubeController.State.Bumping:
					break;
				case CubeController.State.StartJump:
					break;
				case CubeController.State.Jumping:
					break;
				case CubeController.State.EndJump:
					break;
				case CubeController.State.WallGrab:
				{
					// Hide wall grab
					_menu.SetActiveLayoutMenu(Key.WallGrab, false);
					// Show wall jump
					_menu.SetActiveLayoutMenu(Key.WallJump, true);
				}
				break;
				case CubeController.State.WallJump:
					break;
				case CubeController.State.Dashing:
					break;
				case CubeController.State.DamageTaken:
					break;
				default:
					break;
			}
		}


		private void PlayerItemHolderOnStateChangedTransitionIn(ItemHolder sender, ItemHolder.ItemHolderEventArgs args)
		{
			var fromState = args.fromState;
			switch (args.currentState)
			{
				case ItemHolder.State.HandFree:
				{
					if (fromState == ItemHolder.State.HighlightingAnItem)
					{
						// hide take
						_menu.SetActiveLayoutMenu(Key.TakeItem, false);
					}
					else
					{
						// hide release
						_menu.SetActiveLayoutMenu(Key.ReleaseItem, false);
					}
				}
				break;
				case ItemHolder.State.HighlightingAnItem:
				{
					if (fromState == ItemHolder.State.HoldingAnItem)
					{
						// hide release
						_menu.SetActiveLayoutMenu(Key.ReleaseItem, false);
					}

				}
				break;
				case ItemHolder.State.HoldingAnItem:
				{
					// hide take
					_menu.SetActiveLayoutMenu(Key.TakeItem, false);
				}
				break;
				default:
					break;
			}
		}

		private void PlayerItemHolderOnStateChangedTransitionOut(ItemHolder sender, ItemHolder.ItemHolderEventArgs args)
		{
			switch (args.currentState)
			{
				case ItemHolder.State.HandFree:
				{
				}
				break;
				case ItemHolder.State.HighlightingAnItem:
				{
					// show take
					_menu.SetActiveLayoutMenu(Key.TakeItem, true);
				}
				break;
				case ItemHolder.State.HoldingAnItem:
				{
					// Show release
					_menu.SetActiveLayoutMenu(Key.ReleaseItem, true);
				}
				break;
				default:
					break;
			}
		}


		#endregion Callbacks

	}

}