namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Player;
	using GSGD2.Utilities;
	using GSGD2.Extensions;

	/// <summary>
	/// Component that can hold an <see cref="Item"/>.
	/// It highlight it when near, and can grab it.
	/// </summary>
	[RequireComponent(typeof(PlayerController))]
	[RequireComponent(typeof(CubeController))]
	public class ItemHolder : MonoBehaviour
	{
		public enum State
		{
			HandFree,
			HighlightingAnItem,
			HoldingAnItem
		}

		[SerializeField]
		private PlayerReferences _playerReferences = null;

		[SerializeField]
		private Transform _itemHeldParent = null;

		[SerializeField]
		private Raycaster _itemRaycaster = null;

		private PlayerController _playerController = null;
		private CubeController _cubeController = null;

		private State _currentState = 0f;
		private Item _currentHeldItem = null;
		private Item _currentHighlightedItem = null;
		private bool _hasSwitchedHighlightedItem = false;
		private MovingPlatformStickToGroundReceiver _movingPlatformStickToGroundReceiver = null;

		private bool _willTakeItem = false;
		private bool _willReleaseItem = false;

		public Item CurrentHeldItem => _currentHeldItem;

		public struct ItemHolderEventArgs
		{
			public State fromState;
			public State currentState;
			public Item item;

			public ItemHolderEventArgs(State fromState, State currentState, Item item)
			{
				this.fromState = fromState;
				this.currentState = currentState;
				this.item = item;
			}
		}

		public delegate void ItemHolderEvent(ItemHolder sender, ItemHolderEventArgs args);
		public event ItemHolderEvent StateChangedTransitionIn = null;
		public event ItemHolderEvent StateChangedTransitionOut = null;

		private void Awake()
		{
			_playerReferences.TryGetPlayerController(out _playerController);
			_playerReferences.TryGetCubeController(out _cubeController);
		}

		private void OnEnable()
		{
			_movingPlatformStickToGroundReceiver = GetComponent<MovingPlatformStickToGroundReceiver>();

			LevelReferences.Instance.PlayerStart.BeforePlayerPositionReset -= PlayerStartOnPlayerPositionReset;
			LevelReferences.Instance.PlayerStart.BeforePlayerPositionReset += PlayerStartOnPlayerPositionReset;

			_playerController.TakeItemPerformed -= PlayerController_TakeItemPerformed;
			_playerController.TakeItemPerformed += PlayerController_TakeItemPerformed;

			_playerController.ReleaseItemPerformed -= PlayerController_ReleaseItemPerformed;
			_playerController.ReleaseItemPerformed += PlayerController_ReleaseItemPerformed;

			_cubeController.AddToCanChangeToMovementState(CubeController_CanChangeToMovementState);
		}

		private void OnDisable()
		{
			if (ApplicationExtension.IsPlaying) // prevent from leaking objects when stopping editor play
			{
				// Release held item before disabling the component
				if (_currentState != State.HandFree)
				{
					ChangeState(State.HandFree);
				}

				if (LevelReferences.HasInstance == true)
				{
					LevelReferences.Instance.PlayerStart.BeforePlayerPositionReset -= PlayerStartOnPlayerPositionReset;
				}

				_playerController.TakeItemPerformed -= PlayerController_TakeItemPerformed;
				_playerController.ReleaseItemPerformed -= PlayerController_ReleaseItemPerformed;

				_cubeController.RemoveFromCanChangeToMovementState(CubeController_CanChangeToMovementState);
			}
		}

		private bool CubeController_CanChangeToMovementState(CubeController.State currentState, CubeController.State newState)
		{
			switch (_currentState)
			{
				case State.HandFree: return true;
				case State.HighlightingAnItem: return true;
				case State.HoldingAnItem:
				{
					if (_currentHeldItem != null)
					{
						return _currentHeldItem.PreventMovementStatesWhenHoldingAnItem.HasFlag(newState) == false;
					}
				}
				return true;
				default: return true;
			}
		}

		private void PlayerController_TakeItemPerformed(PlayerController sender, UnityEngine.InputSystem.InputAction.CallbackContext obj)
		{
			switch (_currentState)
			{
				case State.HighlightingAnItem:
				{
					_willTakeItem = true;
				}
				break;
				case State.HandFree:
				case State.HoldingAnItem:
				default: break;
			}
		}

		private void PlayerController_ReleaseItemPerformed(PlayerController sender, UnityEngine.InputSystem.InputAction.CallbackContext obj)
		{
			switch (_currentState)
			{
				case State.HoldingAnItem:
				{
					_willReleaseItem = true;
				}
				break;
				case State.HandFree:
				case State.HighlightingAnItem:
				default: break;
			}
		}

		private void PlayerStartOnPlayerPositionReset(PlayerStart sender, PlayerStart.PlayerStartEventArgs args)
		{
			if (_currentHeldItem != null)
			{
				var heldItem = _currentHeldItem;
				ChangeState(State.HandFree);
				heldItem.ResetWorldPosition();
			}
		}

		private void Update()
		{
			UpdateState();
		}

		// raycast to highlight item and remember the highlighted item
		private void DoRaycast(bool forceChangeStateOnHighlight = false, bool forceChangeStateOnHandFree = false)
		{
			bool result = _itemRaycaster.RaycastAll(out RaycastHit[] hits, QueryTriggerInteraction.Collide);
			if (result == true)
			{
				// Add only items that is closest that already added items.
				List<Item> items = new List<Item>();
				float closestDistance = 99999;
				int closestDistanceIndex = -1; // it could be the same item already highlighted raycasted twice
				for (int i = 0; i < hits.Length; i++)
				{
					var hit = hits[i];
					var item = hit.collider.transform.GetComponentInParent<Item>();
					if (item != null)
					{
						float itemDistance = Vector3.Distance(item.transform.position, transform.position);
						if (closestDistance > itemDistance)
						{
							closestDistance = itemDistance;
							items.Add(item);
							closestDistanceIndex = items.Count - 1;
						}
					}
				}

				int itemsCount = items.Count;
				if (itemsCount > 0)
				{
					if (itemsCount > 1)
					{
						// Then re check item distance and highlight the closest
						for (int i = 0; i < items.Count; i++)
						{
							var item = items[i];
							float itemDistance = Vector3.Distance(item.transform.position, transform.position);
							if (itemDistance < closestDistance)
							{
								closestDistance = itemDistance;
								closestDistanceIndex = i;
							}
						}
						if (closestDistanceIndex != -1)
						{
							var item = items[closestDistanceIndex];
							if (ReferenceEquals(_currentHighlightedItem, item) == false)
							{
								HighlightItem(item);
							}
						}
					}
					else
					{
						// If there are one item, don't need to check the distance
						HighlightItem(items[0]);
					}
				}

			}
			else
			{
				ChangeState(State.HandFree, forceChangeStateOnHandFree);
			}

			void HighlightItem(Item item) // rename
			{
				if (_currentHighlightedItem != null)
				{
					_currentHighlightedItem.ChangeMaterial(false);
					_currentHighlightedItem.ChangeState(Item.State.Idle);
					_hasSwitchedHighlightedItem = true;
				}
				_currentHighlightedItem = item;
				ChangeState(State.HighlightingAnItem, forceChangeStateOnHighlight);
			}
		}

		private void ChangeState(State newState, bool forceChange = false)
		{
			if (forceChange == false && _currentState == newState && _hasSwitchedHighlightedItem == false) return;
			State previousState = _currentState;

			switch (previousState)
			{
				case State.HandFree:
				{
				}
				break;
				case State.HighlightingAnItem:
				{
				}
				break;
				case State.HoldingAnItem:
				{
					if (_currentHeldItem.IsFalling == false)
					{
						HandleItemStickToGround(_currentHeldItem, true);
					}
				}
				break;
				default: break;
			}


			StateChangedTransitionIn?.Invoke(this, new ItemHolderEventArgs(previousState, newState, _currentHighlightedItem));



			_currentState = newState;

			// State In Transition
			switch (_currentState)
			{
				case State.HandFree:
				{
					if (_currentHeldItem != null)
					{
						_currentHeldItem.transform.SetParent(null);
						_currentHeldItem.ChangeMaterial(false);
						_currentHeldItem.ChangeState(Item.State.Idle);
					}

					_itemRaycaster.SetResultFalse();

					if (ReferenceEquals(_currentHeldItem, _currentHighlightedItem) == false && _currentHighlightedItem != null)
					{
						_currentHighlightedItem.ChangeMaterial(false);
						_currentHighlightedItem.ChangeState(Item.State.Idle);
					}

					_currentHighlightedItem = null;
					_currentHeldItem = null;
				}
				break;
				case State.HighlightingAnItem:
				{
					_currentHighlightedItem.ChangeMaterial(true);
					_currentHighlightedItem.ChangeState(Item.State.Highlighted);

				}
				break;
				case State.HoldingAnItem:
				{
					_currentHeldItem.ChangeState(Item.State.Held);
					HandleItemStickToGround(_currentHeldItem, false);
				}
				break;
				default: break;
			}

			StateChangedTransitionOut?.Invoke(this, new ItemHolderEventArgs(previousState, newState, _currentHighlightedItem));
		}

		private void UpdateState()
		{
			switch (_currentState)
			{
				case State.HandFree:
				{
					DoRaycast();

				}
				break;
				case State.HighlightingAnItem:
				{
					DoRaycast();

					// DoRaycast may change the state so better be sure to have an item before trying to take it
					if (_currentState == State.HighlightingAnItem && _willTakeItem == true)
					{
						_willTakeItem = false;
						_currentHighlightedItem.SetParent(_itemHeldParent);
						_currentHeldItem = _currentHighlightedItem;
						ChangeState(State.HoldingAnItem);
					}
				}
				break;
				case State.HoldingAnItem:
				{
					// lose item
					if (_willReleaseItem == true)
					{
						_willReleaseItem = false;
						ChangeState(State.HandFree);
					}
				}
				break;
				default: break;
			}
		}

		private void OnDrawGizmos()
		{
			_itemRaycaster.DrawGizmos();
		}

		private void HandleItemStickToGround(Item item, bool add)
		{
			if (_movingPlatformStickToGroundReceiver != null)
			{
				var currentStickToGround = _movingPlatformStickToGroundReceiver.MovingPlatformStickToGround;
				if (currentStickToGround != null)
				{
					var itemStickToGroundReceiver = item.GetComponent<MovingPlatformStickToGroundReceiver>();

					if (add == true)
					{
						currentStickToGround.AddOnPlatform(itemStickToGroundReceiver);
					}
					else
					{
						currentStickToGround.RemoveOnPlatform(itemStickToGroundReceiver, true);
					}
				}
			}
		}
	}
}